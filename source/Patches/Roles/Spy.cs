namespace TownOfUs.Roles
{
    public class Spy : Role
    {
        public Spy(PlayerControl player) : base(player)
        {
            Name = "间谍";
            ImpostorText = () => "Snoop Around And Find Stuff Out";
            TaskText = () => "Gain extra information on the Admin Table";
            Color = Patches.Colors.Spy;
            RoleType = RoleEnum.Spy;
            AddToRoleHistory(RoleType);
        }
    }
}