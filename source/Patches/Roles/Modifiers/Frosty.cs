using System;

namespace TownOfUs.Roles.Modifiers
{
    public class Frosty : Modifier
    {
        public PlayerControl Chilled;
        public DateTime LastChilled { get; set; }
        public bool IsChilled = false;

        public Frosty(PlayerControl player) : base(player)
        {
            Name = "圣诞老人";
            TaskText = () => "在寒冷的冬天带来惊喜";
            Color = Patches.Colors.Frosty;
            ModifierType = ModifierEnum.Frosty;
        }
    }
}