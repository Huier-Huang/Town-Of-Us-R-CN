﻿using Hazel;
using System;
using System.Linq;
using TownOfUs.Extensions;

namespace TownOfUs.Roles
{
    public class Juggernaut : Role
    {
        public Juggernaut(PlayerControl owner) : base(owner)
        {
            Name = "剑圣";
            Color = Patches.Colors.Juggernaut;
            LastKill = DateTime.UtcNow;
            RoleType = RoleEnum.Juggernaut;
            AddToRoleHistory(RoleType);
            ImpostorText = () => "你的力量随着每一次杀戮而增长";
            TaskText = () => "每次击杀，你的击杀冷却时间都会减少\n假任务:";
            Faction = Faction.NeutralKilling;
        }

        public PlayerControl ClosestPlayer;
        public DateTime LastKill { get; set; }
        public bool JuggernautWins { get; set; }
        public int JuggKills { get; set; } = 0;

        internal override bool NeutralWin(LogicGameFlowNormal __instance)
        {
            if (Player.Data.IsDead || Player.Data.Disconnected) return true;

            if (PlayerControl.AllPlayerControls.ToArray().Count(x => !x.Data.IsDead && !x.Data.Disconnected) <= 2 &&
                    PlayerControl.AllPlayerControls.ToArray().Count(x => !x.Data.IsDead && !x.Data.Disconnected &&
                    (x.Data.IsImpostor() || x.Is(Faction.NeutralKilling))) == 1)
            {
                Utils.Rpc(CustomRPC.JuggernautWin, Player.PlayerId);
                Wins();
                Utils.EndGame();
                return false;
            }

            return false;
        }

        public void Wins()
        {
            JuggernautWins = true;
        }

        public float KillTimer()
        {
            var utcNow = DateTime.UtcNow;
            var timeSpan = utcNow - LastKill;
            var num = (CustomGameOptions.JuggKCd - CustomGameOptions.ReducedKCdPerKill * JuggKills) * 1000f;
            var flag2 = num - (float)timeSpan.TotalMilliseconds < 0f;
            if (flag2) return 0;
            return (num - (float)timeSpan.TotalMilliseconds) / 1000f;
        }

        protected override void IntroPrefix(IntroCutscene._ShowTeam_d__36 __instance)
        {
            var juggTeam = new Il2CppSystem.Collections.Generic.List<PlayerControl>();
            juggTeam.Add(PlayerControl.LocalPlayer);
            __instance.teamToShow = juggTeam;
        }
    }
}