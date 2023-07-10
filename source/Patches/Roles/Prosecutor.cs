namespace TownOfUs.Roles
{
    public class Prosecutor : Role
    {
        public Prosecutor(PlayerControl player) : base(player)
        {
            Name = "检察官";
            ImpostorText = () => "检察官的权利很大诶";
            TaskText = () => "选择放逐任何你想放逐的人";
            Color = Patches.Colors.Prosecutor;
            RoleType = RoleEnum.Prosecutor;
            AddToRoleHistory(RoleType);
            StartProsecute = false;
            Prosecuted = false;
            ProsecuteThisMeeting = false;
        }
        public bool ProsecuteThisMeeting { get; set; }
        public bool Prosecuted { get; set; }
        public bool StartProsecute { get; set; }
        public PlayerVoteArea Prosecute { get; set; }
    }
}
