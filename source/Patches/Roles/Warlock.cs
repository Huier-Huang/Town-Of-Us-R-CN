using TMPro;
using System;

namespace TownOfUs.Roles
{
    public class Warlock : Role
    {
        public Warlock(PlayerControl player) : base(player)
        {
            Name = "术士";
            ImpostorText = () => "你是术士(ps:我也不知道干嘛的)";
            TaskText = () => "在小范围内杀人";
            Color = Patches.Colors.Impostor;
            RoleType = RoleEnum.Warlock;
            AddToRoleHistory(RoleType);
            Faction = Faction.Impostors;
            ChargePercent = 0;
        }

        public TextMeshPro ChargeText;
        public int ChargePercent;
        public bool Charging;
        public bool UsingCharge;
        public float ChargeUseDuration;
        public DateTime StartChargeTime;
        public DateTime StartUseTime;

        public int ChargeUpTimer()
        {
            var utcNow = DateTime.UtcNow;
            var timeSpan = utcNow - StartChargeTime;
            var num = CustomGameOptions.ChargeUpDuration * 1000f;
            var result = (float)timeSpan.TotalMilliseconds/num * 100f;
            if (result > 100f) result = 100f;
            return Convert.ToInt32(Math.Round(result));
        }

        public int ChargeUseTimer()
        {
            var utcNow = DateTime.UtcNow;
            var timeSpan = StartUseTime - utcNow;
            var num = ChargeUseDuration * 1000f;
            var result = ((float)timeSpan.TotalMilliseconds / num + 1) * ChargeUseDuration / CustomGameOptions.ChargeUseDuration * 100f;
            if (result < 0f) result = 0f;
            return Convert.ToInt32(Math.Round(result));
        }
    }
}