using UnityEngine;

namespace TownOfUs.Roles.Modifiers
{
    public class Tiebreaker : Modifier
    {
        public Tiebreaker(PlayerControl player) : base(player)
        {
            Name = "破局者";
            TaskText = () => "你的一票能破局";
            Color = Patches.Colors.Tiebreaker;
            ModifierType = ModifierEnum.Tiebreaker;
        }
    }
}