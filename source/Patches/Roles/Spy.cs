namespace TownOfUs.Roles
{
    public class Spy : Role
    {
        public Spy(PlayerControl player) : base(player)
        {
            Name = "特工";
            ImpostorText = () => "四处窥探，发现一些东西";
            TaskText = () => "获得情报.....";
            Color = Patches.Colors.Spy;
            RoleType = RoleEnum.Spy;
            AddToRoleHistory(RoleType);
        }
    }
}