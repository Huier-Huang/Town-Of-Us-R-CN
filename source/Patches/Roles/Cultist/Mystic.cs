namespace TownOfUs.Roles.Cultist
{
    public class CultistMystic : Role
    {
        public CultistMystic(PlayerControl player) : base(player)
        {
            Name = "招魂师";
            ImpostorText = () => "当有人皈依时，要理解";
            TaskText = () => "知道别人什么时候会转变";
            Color = Patches.Colors.Mystic;
            RoleType = RoleEnum.CultistMystic;
            AddToRoleHistory(RoleType);
        }
    }
}