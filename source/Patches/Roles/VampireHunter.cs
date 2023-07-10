using System;
using TMPro;

namespace TownOfUs.Roles
{
    public class VampireHunter : Role
    {
        public PlayerControl ClosestPlayer;
        public DateTime LastStaked { get; set; }

        public int UsesLeft;
        public TextMeshPro UsesText;
        public bool AddedStakes;

        public bool ButtonUsable => UsesLeft != 0;

        public VampireHunter(PlayerControl player) : base(player)
        {
            Name = "吸血鬼杀手";
            ImpostorText = () => "我是让吸血鬼瑟瑟发抖的存在";
            TaskText = () => "可恶的吸血鬼,看老子杀光你们!";
            Color = Patches.Colors.VampireHunter;
            LastStaked = DateTime.UtcNow;
            RoleType = RoleEnum.VampireHunter;
            AddToRoleHistory(RoleType);

            UsesLeft = 0;
            AddedStakes = false;
        }

        public float StakeTimer()
        {
            var utcNow = DateTime.UtcNow;
            var timeSpan = utcNow - LastStaked;
            var num = CustomGameOptions.StakeCd * 1000f;
            var flag2 = num - (float) timeSpan.TotalMilliseconds < 0f;
            if (flag2) return 0;
            return (num - (float) timeSpan.TotalMilliseconds) / 1000f;
        }
    }
}