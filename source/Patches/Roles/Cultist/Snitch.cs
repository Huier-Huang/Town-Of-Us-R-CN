namespace TownOfUs.Roles.Cultist
{
    public class CultistSnitch : Role
    {
        public bool CompletedTasks;
        public PlayerControl RevealedPlayer;
        public CultistSnitch(PlayerControl player) : base(player)
        {
            Name = "告密者";
            ImpostorText = () => "完成所有任务来找出内鬼";
            TaskText = () => "完成你所有的任务来找出内鬼";
            Color = Patches.Colors.Snitch;
            RoleType = RoleEnum.CultistSnitch;
            AddToRoleHistory(RoleType);
        }
    }
}