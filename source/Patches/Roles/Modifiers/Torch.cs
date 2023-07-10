using UnityEngine;

namespace TownOfUs.Roles.Modifiers
{
    public class Torch : Modifier
    {
        public Torch(PlayerControl player) : base(player)
        {
            Name = "火炬手";
            TaskText = () => "你能照亮黑暗";
            Color = Patches.Colors.Torch;
            ModifierType = ModifierEnum.Torch;
        }
    }
}