using System;
using System.Collections.Generic;

namespace TownOfUs.Roles
{
    public class Seer : Role
    {
        public List<byte> Investigated = new List<byte>();

        public Seer(PlayerControl player) : base(player)
        {
            Name = "预言家";
            ImpostorText = () => "一个很神奇的职业";
            TaskText = () => "让我看看是好人还是坏人";
            Color = Patches.Colors.Seer;
            LastInvestigated = DateTime.UtcNow;
            RoleType = RoleEnum.Seer;
            AddToRoleHistory(RoleType);
        }

        public PlayerControl ClosestPlayer;
        public DateTime LastInvestigated { get; set; }

        public float SeerTimer()
        {
            var utcNow = DateTime.UtcNow;
            var timeSpan = utcNow - LastInvestigated;
            var num = CustomGameOptions.SeerCd * 1000f;
            var flag2 = num - (float) timeSpan.TotalMilliseconds < 0f;
            if (flag2) return 0;
            return (num - (float) timeSpan.TotalMilliseconds) / 1000f;
        }
    }
}