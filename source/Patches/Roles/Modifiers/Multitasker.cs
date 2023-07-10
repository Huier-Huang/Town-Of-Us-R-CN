using UnityEngine;

namespace TownOfUs.Roles.Modifiers
{
    public class Multitasker : Modifier
    {
        public Multitasker(PlayerControl player) : base(player)
        {
            Name = "多线程";
            TaskText = () => "多线程=一心多用懂？";
            Color = Patches.Colors.Multitasker;
            ModifierType = ModifierEnum.Multitasker;
        }
    }
}