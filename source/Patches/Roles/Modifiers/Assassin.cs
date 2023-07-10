using System.Collections.Generic;
using System.Linq;
using TMPro;
using TownOfUs.Patches;
using UnityEngine;
using TownOfUs.NeutralRoles.ExecutionerMod;
using TownOfUs.NeutralRoles.GuardianAngelMod;
using TownOfUs.CrewmateRoles.VampireHunterMod;

namespace TownOfUs.Roles.Modifiers
{
    public class Assassin : Ability
    {
        public Dictionary<byte, (GameObject, GameObject, GameObject, TMP_Text)> Buttons = new Dictionary<byte, (GameObject, GameObject, GameObject, TMP_Text)>();


        private Dictionary<string, Color> ColorMapping = new Dictionary<string, Color>();

        public Dictionary<string, Color> SortedColorMapping;

        public Dictionary<byte, string> Guesses = new Dictionary<byte, string>();


        public Assassin(PlayerControl player) : base(player)
        {
            Name = "刺客";
            TaskText = () => "对他人进行猜测";
            Color = Patches.Colors.Impostor;
            AbilityType = AbilityEnum.Assassin;

            RemainingKills = CustomGameOptions.AssassinKills;

            // Adds all the roles that have a non-zero chance of being in the game.
            if (CustomGameOptions.MayorOn > 0) ColorMapping.Add("市长", Colors.Mayor);
            if (CustomGameOptions.SheriffOn > 0 || (CustomGameOptions.VampireHunterOn > 0 && CustomGameOptions.GameMode == GameMode.Classic && CustomGameOptions.VampireOn > 0 && CustomGameOptions.BecomeOnVampDeaths == BecomeEnum.Sheriff)) ColorMapping.Add("警长", Colors.Sheriff);
            if (CustomGameOptions.EngineerOn > 0) ColorMapping.Add("工程师", Colors.Engineer);
            if (CustomGameOptions.SwapperOn > 0) ColorMapping.Add("换票师", Colors.Swapper);
            if (CustomGameOptions.InvestigatorOn > 0) ColorMapping.Add("调查员", Colors.Investigator);
            if (CustomGameOptions.MedicOn > 0) ColorMapping.Add("医生", Colors.Medic);
            if (CustomGameOptions.SeerOn > 0) ColorMapping.Add("预言家", Colors.Seer);
            if (CustomGameOptions.SpyOn > 0) ColorMapping.Add("间谍", Colors.Spy);
            if (CustomGameOptions.SnitchOn > 0) ColorMapping.Add("告密者", Colors.Snitch);
            if (CustomGameOptions.AltruistOn > 0) ColorMapping.Add("殉道者", Colors.Altruist);
            if (CustomGameOptions.VigilanteOn > 0 || (CustomGameOptions.VampireHunterOn > 0 && CustomGameOptions.GameMode == GameMode.Classic && CustomGameOptions.VampireOn > 0 && CustomGameOptions.BecomeOnVampDeaths == BecomeEnum.Vigilante)) ColorMapping.Add("侠客", Colors.Vigilante);
            if (CustomGameOptions.VeteranOn > 0 || (CustomGameOptions.VampireHunterOn > 0 && CustomGameOptions.GameMode == GameMode.Classic && CustomGameOptions.VampireOn > 0 && CustomGameOptions.BecomeOnVampDeaths == BecomeEnum.Veteran)) ColorMapping.Add("老兵", Colors.Veteran);
            if (CustomGameOptions.TrackerOn > 0) ColorMapping.Add("追踪者", Colors.Tracker);
            if (CustomGameOptions.TrapperOn > 0) ColorMapping.Add("陷阱师", Colors.Trapper);
            if (CustomGameOptions.TransporterOn > 0) ColorMapping.Add("传送师", Colors.Transporter);
            if (CustomGameOptions.MediumOn > 0) ColorMapping.Add("灵媒", Colors.Medium);
            if (CustomGameOptions.MysticOn > 0) ColorMapping.Add("招魂", Colors.Mystic);
            if (CustomGameOptions.DetectiveOn > 0) ColorMapping.Add("侧写师", Colors.Detective);
            if (CustomGameOptions.ImitatorOn > 0) ColorMapping.Add("效颦模仿者", Colors.Imitator);
            if (CustomGameOptions.VampireHunterOn > 0 && CustomGameOptions.GameMode == GameMode.Classic && CustomGameOptions.VampireOn > 0) ColorMapping.Add("吸血鬼杀手", Colors.VampireHunter);
            if (CustomGameOptions.ProsecutorOn > 0) ColorMapping.Add("检察官", Colors.Prosecutor);
            if (CustomGameOptions.OracleOn > 0) ColorMapping.Add("神之使徒", Colors.Oracle);
            if (CustomGameOptions.AurialOn > 0) ColorMapping.Add("修仙者", Colors.Aurial);

            // Add Neutral roles if enabled
            if (CustomGameOptions.AssassinGuessNeutralBenign)
            {
                if (CustomGameOptions.AmnesiacOn > 0 || (CustomGameOptions.ExecutionerOn > 0 && CustomGameOptions.OnTargetDead == OnTargetDead.Amnesiac) || (CustomGameOptions.GuardianAngelOn > 0 && CustomGameOptions.GaOnTargetDeath == BecomeOptions.Amnesiac)) ColorMapping.Add("失忆者", Colors.Amnesiac);
                if (CustomGameOptions.GuardianAngelOn > 0) ColorMapping.Add("守护天使", Colors.GuardianAngel);
                if (CustomGameOptions.SurvivorOn > 0 || (CustomGameOptions.ExecutionerOn > 0 && CustomGameOptions.OnTargetDead == OnTargetDead.Survivor) || (CustomGameOptions.GuardianAngelOn > 0 && CustomGameOptions.GaOnTargetDeath == BecomeOptions.Survivor)) ColorMapping.Add("幸存者", Colors.Survivor);
            }
            if (CustomGameOptions.AssassinGuessNeutralEvil)
            {
                if (CustomGameOptions.DoomsayerOn > 0) ColorMapping.Add("末日预示者", Colors.Doomsayer);
                if (CustomGameOptions.ExecutionerOn > 0) ColorMapping.Add("处刑人", Colors.Executioner);
                if (CustomGameOptions.JesterOn > 0 || (CustomGameOptions.ExecutionerOn > 0 && CustomGameOptions.OnTargetDead == OnTargetDead.Jester) || (CustomGameOptions.GuardianAngelOn > 0 && CustomGameOptions.GaOnTargetDeath == BecomeOptions.Jester)) ColorMapping.Add("小丑", Colors.Jester);
            }
            if (CustomGameOptions.AssassinGuessNeutralKilling)
            {
                if (CustomGameOptions.ArsonistOn > 0 && !PlayerControl.LocalPlayer.Is(RoleEnum.Arsonist)) ColorMapping.Add("纵火犯", Colors.Arsonist);
                if (CustomGameOptions.GlitchOn > 0 && !PlayerControl.LocalPlayer.Is(RoleEnum.Glitch)) ColorMapping.Add("混沌", Colors.Glitch);
                if (CustomGameOptions.PlaguebearerOn > 0 && !PlayerControl.LocalPlayer.Is(RoleEnum.Plaguebearer)) ColorMapping.Add("瘟疫之源", Colors.Plaguebearer);
                if (CustomGameOptions.GameMode == GameMode.Classic && CustomGameOptions.VampireOn > 0 && !PlayerControl.LocalPlayer.Is(RoleEnum.Vampire)) ColorMapping.Add("吸血鬼", Colors.Vampire);
                if (CustomGameOptions.WerewolfOn > 0 && !PlayerControl.LocalPlayer.Is(RoleEnum.Werewolf)) ColorMapping.Add("狼人", Colors.Werewolf);
                if (!PlayerControl.LocalPlayer.Is(RoleEnum.Juggernaut) && CustomGameOptions.HiddenRoles) ColorMapping.Add("剑圣", Colors.Juggernaut);
            }
            if (CustomGameOptions.AssassinGuessImpostors && !PlayerControl.LocalPlayer.Is(Faction.Impostors))
            {
                ColorMapping.Add("Impostor", Colors.Impostor);
                if (CustomGameOptions.JanitorOn > 0) ColorMapping.Add("清洁工", Colors.Impostor);
                if (CustomGameOptions.MorphlingOn > 0) ColorMapping.Add("化形者", Colors.Impostor);
                if (CustomGameOptions.MinerOn > 0) ColorMapping.Add("矿工", Colors.Impostor);
                if (CustomGameOptions.SwooperOn > 0) ColorMapping.Add("隐身人", Colors.Impostor);
                if (CustomGameOptions.UndertakerOn > 0) ColorMapping.Add("送葬者", Colors.Impostor);
                if (CustomGameOptions.EscapistOn > 0) ColorMapping.Add("逃逸者", Colors.Impostor);
                if (CustomGameOptions.GrenadierOn > 0) ColorMapping.Add("掷弹兵", Colors.Impostor);
                if (CustomGameOptions.TraitorOn > 0) ColorMapping.Add("Traitor", Colors.Impostor);
                if (CustomGameOptions.BlackmailerOn > 0) ColorMapping.Add("Blackmailer", Colors.Impostor);
                if (CustomGameOptions.BomberOn > 0) ColorMapping.Add("Bomber", Colors.Impostor);
                if (CustomGameOptions.WarlockOn > 0) ColorMapping.Add("Warlock", Colors.Impostor);
                if (CustomGameOptions.VenererOn > 0) ColorMapping.Add("Venerer", Colors.Impostor);
            }

            // Add vanilla crewmate if enabled
            if (CustomGameOptions.AssassinCrewmateGuess) ColorMapping.Add("Crewmate", Colors.Crewmate);
            //Add modifiers if enabled
            if (CustomGameOptions.AssassinGuessModifiers && CustomGameOptions.BaitOn > 0) ColorMapping.Add("Bait", Colors.Bait);
            if (CustomGameOptions.AssassinGuessModifiers && CustomGameOptions.AftermathOn > 0) ColorMapping.Add("Aftermath", Colors.Aftermath);
            if (CustomGameOptions.AssassinGuessModifiers && CustomGameOptions.DiseasedOn > 0) ColorMapping.Add("Diseased", Colors.Diseased);
            if (CustomGameOptions.AssassinGuessModifiers && CustomGameOptions.FrostyOn > 0) ColorMapping.Add("Frosty", Colors.Frosty);
            if (CustomGameOptions.AssassinGuessModifiers && CustomGameOptions.MultitaskerOn > 0) ColorMapping.Add("Multitasker", Colors.Multitasker);
            if (CustomGameOptions.AssassinGuessModifiers && CustomGameOptions.TorchOn > 0) ColorMapping.Add("Torch", Colors.Torch);
            if (CustomGameOptions.AssassinGuessLovers && CustomGameOptions.LoversOn > 0) ColorMapping.Add("Lover", Colors.Lovers);

            // Sorts the list alphabetically. 
            SortedColorMapping = ColorMapping.OrderBy(x => x.Key).ToDictionary(x => x.Key, x => x.Value);
        }

        public bool GuessedThisMeeting { get; set; } = false;

        public int RemainingKills { get; set; }

        public List<string> PossibleGuesses => SortedColorMapping.Keys.ToList();
    }
}
