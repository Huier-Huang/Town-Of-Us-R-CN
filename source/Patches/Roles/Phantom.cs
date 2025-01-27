using TownOfUs.Extensions;
using UnityEngine;

namespace TownOfUs.Roles
{
    public class Phantom : Role
    {
        public RoleEnum formerRole = new RoleEnum();
        public bool Caught;
        public bool CompletedTasks;
        public bool Faded;

        public Phantom(PlayerControl player) : base(player)
        {
            Name = "幻影";
            ImpostorText = () => "";
            TaskText = () => "在不被抓到的情况下完成所有任务!";
            Color = Patches.Colors.Phantom;
            RoleType = RoleEnum.Phantom;
            AddToRoleHistory(RoleType);
            Faction = Faction.NeutralEvil;
        }

        public void Fade()
        {
            Faded = true;
            Player.Visible = true;
            var color = new Color(1f, 1f, 1f, 0f);

            var maxDistance = ShipStatus.Instance.MaxLightRadius * GameOptionsManager.Instance.currentNormalGameOptions.CrewLightMod;

            if (PlayerControl.LocalPlayer == null)
                return;

            var distance = (PlayerControl.LocalPlayer.GetTruePosition() - Player.GetTruePosition()).magnitude;

            var distPercent = distance / maxDistance;
            distPercent = Mathf.Max(0, distPercent - 1);

            var velocity = Player.gameObject.GetComponent<Rigidbody2D>().velocity.magnitude;
            color.a = 0.07f + velocity / Player.MyPhysics.GhostSpeed * 0.13f;
            color.a = Mathf.Lerp(color.a, 0, distPercent);

            if (Player.GetCustomOutfitType() != CustomPlayerOutfitType.PlayerNameOnly)
            {
                Player.SetOutfit(CustomPlayerOutfitType.PlayerNameOnly, new GameData.PlayerOutfit()
                {
                    ColorId = Player.GetDefaultOutfit().ColorId,
                    HatId = "",
                    SkinId = "",
                    VisorId = "",
                    PlayerName = " ",
                    PetId = " "
                });
            }
            Player.myRend().color = color;
            Player.nameText().color = Color.clear;
            Player.cosmetics.colorBlindText.color = Color.clear;
            Player.cosmetics.SetBodyCosmeticsVisible(false);
        }
    }
}