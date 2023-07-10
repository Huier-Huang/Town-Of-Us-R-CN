using System.Collections.Generic;
using UnityEngine;
using System;

namespace TownOfUs.Roles
{
    public class Medic : Role
    {
        public readonly List<GameObject> Buttons = new List<GameObject>();
        public Dictionary<int, string> LightDarkColors = new Dictionary<int, string>();
        public DateTime StartingCooldown { get; set; }
        public Medic(PlayerControl player) : base(player)
        {
            Name = "医生";
            ImpostorText = () => "创造一个护盾来保护一名船员";
            TaskText = () => "用盾牌保护船员";
            Color = Patches.Colors.Medic;
            StartingCooldown = DateTime.UtcNow;
            RoleType = RoleEnum.Medic;
            AddToRoleHistory(RoleType);
            ShieldedPlayer = null;

            LightDarkColors.Add(0, "深色"); // Red
            LightDarkColors.Add(1, "深色"); // Blue
            LightDarkColors.Add(2, "深色"); // Green
            LightDarkColors.Add(3, "浅色"); // Pink
            LightDarkColors.Add(4, "浅色"); // Orange
            LightDarkColors.Add(5, "浅色"); // Yellow
            LightDarkColors.Add(6, "深色"); // Black
            LightDarkColors.Add(7, "浅色"); // White
            LightDarkColors.Add(8, "深色"); // Purple
            LightDarkColors.Add(9, "深色"); // Brown
            LightDarkColors.Add(10, "浅色"); // Cyan
            LightDarkColors.Add(11, "浅色"); // Lime
            LightDarkColors.Add(12, "深色"); // Maroon
            LightDarkColors.Add(13, "浅色"); // Rose
            LightDarkColors.Add(14, "浅色"); // Banana
            LightDarkColors.Add(15, "深色"); // Grey
            LightDarkColors.Add(16, "深色"); // Tan
            LightDarkColors.Add(17, "浅色"); // Coral
            LightDarkColors.Add(18, "深色"); // Watermelon
            LightDarkColors.Add(19, "深色"); // Chocolate
            LightDarkColors.Add(20, "浅色"); // Sky Blue
            LightDarkColors.Add(21, "浅色"); // Biege
            LightDarkColors.Add(22, "深色"); // Magenta
            LightDarkColors.Add(23, "浅色"); // Turquoise
            LightDarkColors.Add(24, "浅色"); // Lilac
            LightDarkColors.Add(25, "深色"); // Olive
            LightDarkColors.Add(26, "浅色"); // Azure
            LightDarkColors.Add(27, "深色"); // Plum
            LightDarkColors.Add(28, "深色"); // Jungle
            LightDarkColors.Add(29, "浅色"); // Mint
            LightDarkColors.Add(30, "浅色"); // Chartreuse
            LightDarkColors.Add(31, "深色"); // Macau
            LightDarkColors.Add(32, "深色"); // Tawny
            LightDarkColors.Add(33, "浅色"); // Gold
            LightDarkColors.Add(34, "浅色"); // Rainbow
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

        public PlayerControl ClosestPlayer;
        public bool UsedAbility { get; set; } = false;
        public PlayerControl ShieldedPlayer { get; set; }
        public PlayerControl exShielded { get; set; }
    }
}