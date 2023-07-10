using TownOfUs.Modifiers.UnderdogMod;

namespace TownOfUs.Roles.Modifiers
{
    public class Underdog : Modifier
    {
        public Underdog(PlayerControl player) : base(player)
        {
            Name = "失败者";
            TaskText = () => "当你孤生一人之时你有无限潜能\n你将不是失败者";
            Color = Patches.Colors.Impostor;
            ModifierType = ModifierEnum.Underdog;
        }

        public float MaxTimer() => PerformKill.LastImp() ? GameOptionsManager.Instance.currentNormalGameOptions.KillCooldown - CustomGameOptions.UnderdogKillBonus : (PerformKill.IncreasedKC() ? GameOptionsManager.Instance.currentNormalGameOptions.KillCooldown : GameOptionsManager.Instance.currentNormalGameOptions.KillCooldown + CustomGameOptions.UnderdogKillBonus);

        public void SetKillTimer()
        {
            Player.SetKillTimer(MaxTimer());
        }
    }
}
