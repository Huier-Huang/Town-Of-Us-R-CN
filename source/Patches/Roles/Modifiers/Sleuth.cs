using UnityEngine;
using System.Collections.Generic;

namespace TownOfUs.Roles.Modifiers
{
    public class Sleuth : Modifier
    {
        public List<byte> Reported = new List<byte>();
        public Sleuth(PlayerControl player) : base(player)
        {
            Name = "撅暮者";
            TaskText = () => "你能从死人上获取信息";
            Color = Patches.Colors.Sleuth;
            ModifierType = ModifierEnum.Sleuth;
        }
    }
}