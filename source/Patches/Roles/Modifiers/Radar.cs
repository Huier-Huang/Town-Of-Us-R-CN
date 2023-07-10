using System.Collections.Generic;

namespace TownOfUs.Roles.Modifiers
{
    public class Radar : Modifier
    {
        public List<ArrowBehaviour> RadarArrow = new List<ArrowBehaviour>();
        public PlayerControl ClosestPlayer;
        public Radar(PlayerControl player) : base(player)
        {
            Name = "雷达";
            TaskText = () => "雷达开始侦测！！";
            Color = Patches.Colors.Radar;
            ModifierType = ModifierEnum.Radar;
        }
    }
}