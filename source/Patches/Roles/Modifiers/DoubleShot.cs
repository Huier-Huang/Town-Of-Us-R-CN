namespace TownOfUs.Roles.Modifiers
{
    public class DoubleShot : Modifier
    {
        public bool LifeUsed;
        public DoubleShot(PlayerControl player) : base(player)
        {
            Name = "专业刺客";
            TaskText = () => "你有两次机会来刺杀他";
            Color = Patches.Colors.Impostor;
            ModifierType = ModifierEnum.DoubleShot;
            LifeUsed = false;
        }
    }
}