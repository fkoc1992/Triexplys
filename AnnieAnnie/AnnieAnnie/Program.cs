using System;
using System.Linq;
using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Events;
using EloBuddy.SDK.Menu;
using EloBuddy.SDK.Menu.Values;
using EloBuddy.SDK.Rendering;
using Color = System.Drawing.Color;
using EloBuddy.SDK.Enumerations;
using SharpDX;

namespace AnnieAnnie
{
    class Program
    {
        public static Spell.Targeted Q;
        public static Spell.Skillshot W;
        public static Spell.Active E;
        public static Spell.Skillshot R;
        public static Menu Menu, SkillMenu, MiscMenu, DrawMenu;
        public static string Version = "1.0";
        private object draw;
        private object combo;

        static void Main(string[] args)
        {
            Loading.OnLoadingComplete += Loading_OnLoadingComplete;
        }

        public static AIHeroClient Player
        {
            get { return ObjectManager.Player; }
        }

        private static void Loading_OnLoadingComplete(EventArgs args)
        {
            if (Player.ChampionName != "Annie")
                return;

            Q = new Spell.Targeted(SpellSlot.Q, 625);
            W = new Spell.Skillshot(SpellSlot.W, 625, SkillShotType.Cone, 250, int.MaxValue, 210);
            E = new Spell.Active(SpellSlot.E);
            R = new Spell.Skillshot(SpellSlot.R, 600, SkillShotType.Circular, 50, int.MaxValue, 250);

            #region Menu
            Menu = MainMenu.AddMenu("AnnieAnnie", "annieannie");
            Menu.AddGroupLabel("AnnieAnnie X" + Version);
            Menu.AddSeparator();
            Menu.AddLabel("Made By Triexplys");

            SkillMenu = Menu.AddSubMenu("Skills", "Skills");
            SkillMenu.AddGroupLabel("Skills");
            SkillMenu.AddLabel("Combo");
            SkillMenu.Add("Qcombo", new CheckBox("Use Q on Combo"));
            SkillMenu.Add("Wcombo", new CheckBox("Use W on Combo"));
            SkillMenu.Add("Ecombo", new CheckBox("Use E on Combo"));
            SkillMenu.Add("Rcombo", new CheckBox("Use R on Combo"));
            SkillMenu.AddLabel("Harass");
            SkillMenu.Add("Qharass", new CheckBox("Use Q on Harass"));
            SkillMenu.Add("Wharass", new CheckBox("Use W on Harass"));
            SkillMenu.Add("manaLane", new Slider("Mana % To Use Q", 30));
            SkillMenu.Add("manaLane", new Slider("Mana % To Use W", 30));
            SkillMenu.AddLabel("LastHit");
            SkillMenu.Add("Qlast", new CheckBox("Use Q on LastHit"));
            SkillMenu.Add("manaLast", new Slider("Mana % To Use Q", 30));
            SkillMenu.AddLabel("LaneClear");
            SkillMenu.Add("Qlane", new CheckBox("Use Q on LaneClear"));
            SkillMenu.Add("Wlane", new CheckBox("Use W on LaneClear"));
            SkillMenu.Add("manaLane", new Slider("Mana % To Use Q", 30));
            SkillMenu.Add("manaLane", new Slider("Mana % To Use W", 30));

            MiscMenu = Menu.AddSubMenu("Misc", "Misc");
            MiscMenu.AddGroupLabel("Misc");
            MiscMenu.AddLabel("KillSteal");
            MiscMenu.Add("Qks", new CheckBox("Use Q KillSteal"));
            MiscMenu.Add("Rks", new CheckBox("Use R KillSteal"));
            MiscMenu.AddLabel("Ult Manager");
            MiscMenu.Add("Rks", new CheckBox("Use R KillSteal"));

            DrawMenu = Menu.AddSubMenu("Draw", "Draw");
            DrawMenu.AddGroupLabel("Draw");
            DrawMenu.AddLabel("Draw");
            DrawMenu.Add("Qdraw", new CheckBox("Draw Q"));
            DrawMenu.Add("Wdraw", new CheckBox("Draw W"));
            DrawMenu.Add("Edraw", new CheckBox("Draw E"));
            DrawMenu.Add("Rdraw", new CheckBox("Draw R"));
            DrawMenu.Add("drawWhenReady", new CheckBox("Draw When SKills Are Ready"));
            #endregion Menu

            Game.OnTick += Game_OnTick;
            Drawing.OnDraw += Drawing_OnDraw;
            Orbwalker.OnPreAttack += Orbwalker_OnPreAttack;

        }

        private static void Orbwalker_OnPreAttack(AttackableUnit target, Orbwalker.PreAttackArgs args)
        {
            throw new NotImplementedException();
        }

        private static void Drawing_OnDraw(EventArgs args)
        {
            throw new NotImplementedException();
        }

        private static void Game_OnTick(EventArgs args)
        {
            if (Player.IsDead || MenuGUI.IsChatOpen || Player.IsRecalling()) return;

            KillSteal();

            if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Combo))
            {
                Combo();
            }
            if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Harass))
            {
                Harass();
            }
            if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.LastHit))
            {
                LastHit();
            }
            if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.LaneClear))
            {
                LaneClear();
            }
        }

        private static void LaneClear()
        {
            throw new NotImplementedException();
        }

        private static void LastHit()
        {
            throw new NotImplementedException();
        }

        private static void Harass()
        {
            throw new NotImplementedException();
        }

        private static void Combo()
        {
            throw new NotImplementedException();
        }

        private static void KillSteal()
        {
            var useQ = MiscMenu["Qks"].Cast<CheckBox>().CurrentValue;
            var useR = MiscMenu["Rks"].Cast<CheckBox>().CurrentValue;
            var target = TargetSelector.GetTarget(R.Range, DamageType.Magical);

            if (useQ && Q.IsReady() && target.IsValidTarget(Q.Range) && !target.IsZombie && target.Health <= Player.GetSpellDamage(target, SpellSlot.Q))
            {
                Q.Cast(target);
            }


            if (useR && R.IsReady() && target.IsValidTarget(Q.Range) && !target.IsZombie && target.Health <= Player.GetSpellDamage(target, SpellSlot.R))
            {
                R.Cast(target);
            }

        }

        private static void Comboz()
        {
            var useQ = SkillMenu["Qcombo"].Cast<CheckBox>().CurrentValue;
            var useW = SkillMenu["Wcombo"].Cast<CheckBox>().CurrentValue;
            var useE = SkillMenu["Ecombo"].Cast<CheckBox>().CurrentValue;
            var useR = SkillMenu["Rcombo"].Cast<CheckBox>().CurrentValue;
            var target = TargetSelector.GetTarget(R.Range, DamageType.Magical);

            if (useQ && Q.IsReady() && target.IsValidTarget(Q.Range) && !target.IsZombie)
            {
                Q.Cast(target);
            }
            if (useE && E.IsReady() && target.IsValidTarget(E.Range) && !target.IsZombie)
            {
                E.Cast();
            }
            if (useW && W.IsReady() && target.IsValidTarget(W.Range) && !target.IsZombie)
            {
                W.Cast(target);
            }
            if (useR && R.IsReady() && target.IsValidTarget(R.Range) && !target.IsZombie && target.Health <= Player.GetSpellDamage(target, SpellSlot.R))
            {
                R.Cast(target);
            }

        }
        private static void Harasss()
        {
            var useQ = SkillMenu["Qharass"].Cast<CheckBox>().CurrentValue;
            var useE = SkillMenu["Wharass"].Cast<CheckBox>().CurrentValue;
            var target = TargetSelector.GetTarget(Q.Range, DamageType.Magical);

            if (useQ && Q.IsReady() && target.IsValidTarget(Q.Range) && !target.IsZombie)
            {
                Q.Cast(target);
            }
            if (useE && W.IsReady() && target.IsValidTarget(W.Range) && !target.IsZombie)
            {
                W.Cast(target);
            }
        }

        private static void LaneClears()
        {
            var useQ = SkillMenu["Qlane"].Cast<CheckBox>().CurrentValue;
            var useE = SkillMenu["Wlane"].Cast<CheckBox>().CurrentValue;
            var mana = SkillMenu["manaLane"].Cast<Slider>().CurrentValue;
            var minions = ObjectManager.Get<Obj_AI_Base>().OrderBy(m => m.Health).Where(m => m.IsMinion && m.IsEnemy);
            if (useQ && Q.IsReady() && Player.ManaPercent >= mana)
            {
                foreach (var minion in minions)
                {
                    if (minion.IsValidTarget(Q.Range) && minion.Health <= Player.GetSpellDamage(minion, SpellSlot.Q))
                    {
                        Q.Cast(minion);
                    }
                }
            }
            if (useE && W.IsReady() && Player.ManaPercent >= mana)
            {
                foreach (var minion in minions)
                {
                    if (minion.IsValidTarget(W.Range))
                    {
                        W.Cast();
                    }
                }
            }
        }

        private static void LastHits()
        {
            var useQ = SkillMenu["Qlast"].Cast<CheckBox>().CurrentValue;
            var mana = SkillMenu["manaLast"].Cast<Slider>().CurrentValue;
            var minions = ObjectManager.Get<Obj_AI_Minion>().OrderBy(m => m.Health).Where(m => m.IsEnemy);

            if (useQ && Q.IsReady() && Player.ManaPercent >= mana)
            {
                foreach (var minion in minions)
                {
                    if (minion.IsValidTarget(Q.Range) && minion.Health <= Player.GetSpellDamage(minion, SpellSlot.Q))
                    {
                        Q.Cast(minion);

                    }
                }

            }
        }

        private static void Orbwalker_OnPreAttacks(AttackableUnit target, Orbwalker.PreAttackArgs args)
        {
            if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.LastHit)
                || Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Harass)
                || Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.LaneClear))
            {
                var minion = target as Obj_AI_Base;
                var useQ = SkillMenu["Qlast"].Cast<CheckBox>().CurrentValue;
                var mana = SkillMenu["manaLast"].Cast<Slider>().CurrentValue;

                if (minion != null)

                {



                }
            }
        }

    }
}
