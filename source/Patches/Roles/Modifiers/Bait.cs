using UnityEngine;

namespace TownOfUs.Roles.Modifiers
{
    public class Bait : Modifier
    {
        public Bait(PlayerControl player) : base(player)
        {
            Name = "诱饵";
            TaskText = () => "来刀我啊 没事的 诶嘿嘿嘿";
            Color = Patches.Colors.Bait;
            ModifierType = ModifierEnum.Bait;
        }
    }
}