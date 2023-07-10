using System;

namespace TownOfUs.Roles.Modifiers
{
    public class ButtonBarry : Modifier
    {
        public KillButton ButtonButton;

        public bool ButtonUsed;
        public DateTime StartingCooldown { get; set; }

        public ButtonBarry(PlayerControl player) : base(player)
        {
            Name = "执钮人";
            TaskText = () => "远程紧急召开会议!";
            Color = Patches.Colors.ButtonBarry;
            StartingCooldown = DateTime.UtcNow;
            ModifierType = ModifierEnum.ButtonBarry;
        }
        public float StartTimer()
        {
            var utcNow = DateTime.UtcNow;
            var timeSpan = utcNow - StartingCooldown;
            var num = 10000f;
            var flag2 = num - (float)timeSpan.TotalMilliseconds < 0f;
            if (flag2) return 0;
            return (num - (float)timeSpan.TotalMilliseconds) / 1000f;
        }
    }
}