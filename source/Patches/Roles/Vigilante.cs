using System.Collections.Generic;
using System.Linq;
using TMPro;
using TownOfUs.Patches;
using UnityEngine;
using TownOfUs.NeutralRoles.ExecutionerMod;
using TownOfUs.NeutralRoles.GuardianAngelMod;

namespace TownOfUs.Roles
{
    public class Vigilante : Role
    {
        public Dictionary<byte, (GameObject, GameObject, GameObject, TMP_Text)> Buttons = new Dictionary<byte, (GameObject, GameObject, GameObject, TMP_Text)>();

        private Dictionary<string, Color> ColorMapping = new Dictionary<string, Color>();

        public Dictionary<string, Color> SortedColorMapping;

        public Dictionary<byte, string> Guesses = new Dictionary<byte, string>();

        public Vigilante(PlayerControl player) : base(player)
        {
            Name = "侠客";
            ImpostorText = () => "猜测坏人身份";
            TaskText = () => "朋友这是一场见义勇为的豪赌!(青雀.jpg)";
            Color = Patches.Colors.Vigilante;
            RoleType = RoleEnum.Vigilante;
            AddToRoleHistory(RoleType);

            RemainingKills = CustomGameOptions.VigilanteKills;

            if (CustomGameOptions.GameMode == GameMode.Classic || CustomGameOptions.GameMode == GameMode.AllAny)
            {
                ColorMapping.Add("内鬼", Colors.Impostor);
                if (CustomGameOptions.JanitorOn > 0) ColorMapping.Add("清洁工", Colors.Impostor);
                if (CustomGameOptions.MorphlingOn > 0) ColorMapping.Add("化形者", Colors.Impostor);
                if (CustomGameOptions.MinerOn > 0) ColorMapping.Add("矿工", Colors.Impostor);
                if (CustomGameOptions.SwooperOn > 0) ColorMapping.Add("隐身人", Colors.Impostor);
                if (CustomGameOptions.UndertakerOn > 0) ColorMapping.Add("送葬者", Colors.Impostor);
                if (CustomGameOptions.EscapistOn > 0) ColorMapping.Add("逃逸者", Colors.Impostor);
                if (CustomGameOptions.GrenadierOn > 0) ColorMapping.Add("掷弹兵", Colors.Impostor);
                if (CustomGameOptions.TraitorOn > 0) ColorMapping.Add("背叛者", Colors.Impostor);
                if (CustomGameOptions.BlackmailerOn > 0) ColorMapping.Add("勒索者", Colors.Impostor);
                if (CustomGameOptions.BomberOn > 0) ColorMapping.Add("爆破手", Colors.Impostor);
                if (CustomGameOptions.WarlockOn > 0) ColorMapping.Add("术士", Colors.Impostor);
                if (CustomGameOptions.VenererOn > 0) ColorMapping.Add("终末猎手", Colors.Impostor);

                if (CustomGameOptions.VigilanteGuessNeutralBenign)
                {
                    if (CustomGameOptions.AmnesiacOn > 0 || (CustomGameOptions.ExecutionerOn > 0 && CustomGameOptions.OnTargetDead == OnTargetDead.Amnesiac) || (CustomGameOptions.GuardianAngelOn > 0 && CustomGameOptions.GaOnTargetDeath == BecomeOptions.Amnesiac)) ColorMapping.Add("失忆者", Colors.Amnesiac);
                    if (CustomGameOptions.GuardianAngelOn > 0) ColorMapping.Add("守护天使", Colors.GuardianAngel);
                    if (CustomGameOptions.SurvivorOn > 0 || (CustomGameOptions.ExecutionerOn > 0 && CustomGameOptions.OnTargetDead == OnTargetDead.Survivor) || (CustomGameOptions.GuardianAngelOn > 0 && CustomGameOptions.GaOnTargetDeath == BecomeOptions.Survivor)) ColorMapping.Add("幸存者", Colors.Survivor);
                }
                if (CustomGameOptions.VigilanteGuessNeutralEvil)
                {
                    if (CustomGameOptions.DoomsayerOn > 0) ColorMapping.Add("末日预示者", Colors.Doomsayer);
                    if (CustomGameOptions.ExecutionerOn > 0) ColorMapping.Add("处刑人", Colors.Executioner);
                    if (CustomGameOptions.JesterOn > 0 || (CustomGameOptions.ExecutionerOn > 0 && CustomGameOptions.OnTargetDead == OnTargetDead.Jester) || (CustomGameOptions.GuardianAngelOn > 0 && CustomGameOptions.GaOnTargetDeath == BecomeOptions.Jester)) ColorMapping.Add("小丑", Colors.Jester);
                }
                if (CustomGameOptions.VigilanteGuessNeutralKilling)
                {
                    if (CustomGameOptions.ArsonistOn > 0) ColorMapping.Add("纵火犯", Colors.Arsonist);
                    if (CustomGameOptions.GlitchOn > 0) ColorMapping.Add("混沌", Colors.Glitch);
                    if (CustomGameOptions.PlaguebearerOn > 0) ColorMapping.Add("瘟疫之源", Colors.Plaguebearer);
                    if (CustomGameOptions.GameMode == GameMode.Classic && CustomGameOptions.VampireOn > 0) ColorMapping.Add("吸血鬼", Colors.Vampire);
                    if (CustomGameOptions.WerewolfOn > 0) ColorMapping.Add("狼人", Colors.Werewolf);
                    if (CustomGameOptions.HiddenRoles) ColorMapping.Add("剑圣", Colors.Juggernaut);
                }
                if (CustomGameOptions.VigilanteGuessLovers && CustomGameOptions.LoversOn > 0) ColorMapping.Add("恋人", Colors.Lovers);
            }
            else if (CustomGameOptions.GameMode == GameMode.KillingOnly)
            {
                ColorMapping.Add("化形者", Colors.Impostor);
                ColorMapping.Add("矿工", Colors.Impostor);
                ColorMapping.Add("隐身人", Colors.Impostor);
                ColorMapping.Add("送葬者", Colors.Impostor);
                ColorMapping.Add("掷弹兵", Colors.Impostor);
                ColorMapping.Add("背叛者", Colors.Impostor);
                ColorMapping.Add("逃逸者", Colors.Impostor);

                if (CustomGameOptions.VigilanteGuessNeutralKilling)
                {
                    if (CustomGameOptions.AddArsonist) ColorMapping.Add("纵火犯", Colors.Arsonist);
                    if (CustomGameOptions.AddPlaguebearer) ColorMapping.Add("瘟疫之源", Colors.Plaguebearer);
                    ColorMapping.Add("混沌", Colors.Glitch);
                    ColorMapping.Add("月下狼人", Colors.Werewolf);
                    if (CustomGameOptions.HiddenRoles) ColorMapping.Add("剑圣", Colors.Juggernaut);
                }
            }
            else
            {
                ColorMapping.Add("死灵法师", Colors.Impostor);
                ColorMapping.Add("耳语者", Colors.Impostor);
                if (CustomGameOptions.MaxChameleons > 0) ColorMapping.Add("隐身人", Colors.Impostor);
                if (CustomGameOptions.MaxEngineers > 0) ColorMapping.Add("破坏者", Colors.Impostor);
                if (CustomGameOptions.MaxInvestigators > 0) ColorMapping.Add("参谋", Colors.Impostor);
                if (CustomGameOptions.MaxMystics > 0) ColorMapping.Add("先觉者", Colors.Impostor);
                if (CustomGameOptions.MaxSnitches > 0) ColorMapping.Add("远见之人", Colors.Impostor);
                if (CustomGameOptions.MaxSpies > 0) ColorMapping.Add("流氓", Colors.Impostor);
                if (CustomGameOptions.MaxTransporters > 0) ColorMapping.Add("逃逸者", Colors.Impostor);
                if (CustomGameOptions.MaxVigilantes > 1) ColorMapping.Add("刺客", Colors.Impostor);
                ColorMapping.Add("内鬼", Colors.Impostor);
            }

            SortedColorMapping = ColorMapping.OrderBy(x => x.Key).ToDictionary(x => x.Key, x => x.Value);
        }

        public bool GuessedThisMeeting { get; set; } = false;

        public int RemainingKills { get; set; }

        public List<string> PossibleGuesses => SortedColorMapping.Keys.ToList();
    }
}
