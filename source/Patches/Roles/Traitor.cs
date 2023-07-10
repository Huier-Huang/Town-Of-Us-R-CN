namespace TownOfUs.Roles
{
    public class Traitor : Role
    {
        public RoleEnum formerRole = new RoleEnum();
        public Traitor(PlayerControl player) : base(player)
        {
            Name = "叛徒";
            ImpostorText = () => "";
            TaskText = () => "为装好身份不要被发现!";
            Color = Patches.Colors.Impostor;
            RoleType = RoleEnum.Traitor;
            AddToRoleHistory(RoleType);
            Faction = Faction.Impostors;
        }
    }
}