using UnityEngine;

namespace TownOfUs.Roles.Modifiers
{
    public class Diseased : Modifier
    {
        public Diseased(PlayerControl player) : base(player)
        {
            Name = "病人";
            TaskText = () => "我有病! 你敢来杀我吗!";
            Color = Patches.Colors.Diseased;
            ModifierType = ModifierEnum.Diseased;
        }
    }
}