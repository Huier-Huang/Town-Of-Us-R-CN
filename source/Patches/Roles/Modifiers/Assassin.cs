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
        private Dictionary<RoleEnum, string> ColorMapRoleKey = new Dictionary<RoleEnum, string>();
        private Dictionary<ModifierEnum, string> ColorMapModifierKey = new Dictionary<ModifierEnum, string>();

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
            if (CustomGameOptions.MayorOn > 0) AddToMap(RoleEnum.Mayor, "市长", Colors.Mayor);
            if (CustomGameOptions.SheriffOn > 0 || (CustomGameOptions.VampireHunterOn > 0 && CustomGameOptions.GameMode == GameMode.Classic && CustomGameOptions.VampireOn > 0 && CustomGameOptions.BecomeOnVampDeaths == BecomeEnum.Sheriff)) AddToMap(RoleEnum.Sheriff,"警长", Colors.Sheriff);
            if (CustomGameOptions.EngineerOn > 0) AddToMap(RoleEnum.Engineer,"工程师", Colors.Engineer);
            if (CustomGameOptions.SwapperOn > 0) AddToMap(RoleEnum.Swapper,"换票师", Colors.Swapper);
            if (CustomGameOptions.InvestigatorOn > 0) AddToMap(RoleEnum.Investigator,"调查员", Colors.Investigator);
            if (CustomGameOptions.MedicOn > 0) AddToMap(RoleEnum.Medic,"法医", Colors.Medic);
            if (CustomGameOptions.SeerOn > 0) AddToMap(RoleEnum.Seer,"预言家", Colors.Seer);
            if (CustomGameOptions.SpyOn > 0) AddToMap(RoleEnum.Spy,"特工", Colors.Spy);
            if (CustomGameOptions.SnitchOn > 0) AddToMap(RoleEnum.Snitch, "告密者", Colors.Snitch);
            if (CustomGameOptions.AltruistOn > 0) AddToMap(RoleEnum.Altruist,"殉道者", Colors.Altruist);
            if (CustomGameOptions.VigilanteOn > 0 || (CustomGameOptions.VampireHunterOn > 0 && CustomGameOptions.GameMode == GameMode.Classic && CustomGameOptions.VampireOn > 0 && CustomGameOptions.BecomeOnVampDeaths == BecomeEnum.Vigilante)) AddToMap(RoleEnum.Vigilante,"侠客", Colors.Vigilante);
            if (CustomGameOptions.VeteranOn > 0 || (CustomGameOptions.VampireHunterOn > 0 && CustomGameOptions.GameMode == GameMode.Classic && CustomGameOptions.VampireOn > 0 && CustomGameOptions.BecomeOnVampDeaths == BecomeEnum.Veteran)) AddToMap(RoleEnum.Veteran,"老兵", Colors.Veteran);
            if (CustomGameOptions.TrackerOn > 0) AddToMap(RoleEnum.Tracker,"追踪者", Colors.Tracker);
            if (CustomGameOptions.TrapperOn > 0) AddToMap(RoleEnum.Trapper,"陷阱师", Colors.Trapper);
            if (CustomGameOptions.TransporterOn > 0) AddToMap(RoleEnum.Transporter,"传送师", Colors.Transporter);
            if (CustomGameOptions.MediumOn > 0) AddToMap(RoleEnum.Medium,"招魂师", Colors.Medium);
            if (CustomGameOptions.MysticOn > 0) AddToMap(RoleEnum.Mystic,"招魂", Colors.Mystic);
            if (CustomGameOptions.DetectiveOn > 0) AddToMap(RoleEnum.Detective,"侧写师", Colors.Detective);
            if (CustomGameOptions.ImitatorOn > 0) AddToMap(RoleEnum.Imitator,"效颦者", Colors.Imitator);
            if (CustomGameOptions.VampireHunterOn > 0 && CustomGameOptions.GameMode == GameMode.Classic && CustomGameOptions.VampireOn > 0) AddToMap(RoleEnum.VampireHunter,"吸血鬼杀手", Colors.VampireHunter);
            if (CustomGameOptions.ProsecutorOn > 0) AddToMap(RoleEnum.Prosecutor,"检察官", Colors.Prosecutor);
            if (CustomGameOptions.OracleOn > 0) AddToMap(RoleEnum.Oracle,"神之使徒", Colors.Oracle);
            if (CustomGameOptions.AurialOn > 0) AddToMap(RoleEnum.Aurial,"修仙者", Colors.Aurial);

            // Add Neutral roles if enabled
            if (CustomGameOptions.AssassinGuessNeutralBenign)
            {
                if (CustomGameOptions.AmnesiacOn > 0 || (CustomGameOptions.ExecutionerOn > 0 && CustomGameOptions.OnTargetDead == OnTargetDead.Amnesiac) || (CustomGameOptions.GuardianAngelOn > 0 && CustomGameOptions.GaOnTargetDeath == BecomeOptions.Amnesiac)) AddToMap(RoleEnum.Amnesiac,"失忆者", Colors.Amnesiac);
                if (CustomGameOptions.GuardianAngelOn > 0) AddToMap(RoleEnum.GuardianAngel,"守护天使", Colors.GuardianAngel);
                if (CustomGameOptions.SurvivorOn > 0 || (CustomGameOptions.ExecutionerOn > 0 && CustomGameOptions.OnTargetDead == OnTargetDead.Survivor) || (CustomGameOptions.GuardianAngelOn > 0 && CustomGameOptions.GaOnTargetDeath == BecomeOptions.Survivor)) AddToMap(RoleEnum.Survivor,"幸存者", Colors.Survivor);
            }

            if (CustomGameOptions.AssassinGuessNeutralEvil)
            {
                if (CustomGameOptions.DoomsayerOn > 0) AddToMap(RoleEnum.Doomsayer,"末日预示者", Colors.Doomsayer);
                if (CustomGameOptions.ExecutionerOn > 0) AddToMap(RoleEnum.Executioner,"处刑人", Colors.Executioner);
                if (CustomGameOptions.JesterOn > 0 || (CustomGameOptions.ExecutionerOn > 0 && CustomGameOptions.OnTargetDead == OnTargetDead.Jester) || (CustomGameOptions.GuardianAngelOn > 0 && CustomGameOptions.GaOnTargetDeath == BecomeOptions.Jester)) AddToMap(RoleEnum.Jester,"小丑", Colors.Jester);
            }

            if (CustomGameOptions.AssassinGuessNeutralKilling)
            {
                if (CustomGameOptions.ArsonistOn > 0 && !PlayerControl.LocalPlayer.Is(RoleEnum.Arsonist)) AddToMap(RoleEnum.Arsonist,"纵火犯", Colors.Arsonist);
                if (CustomGameOptions.GlitchOn > 0 && !PlayerControl.LocalPlayer.Is(RoleEnum.Glitch)) AddToMap(RoleEnum.Glitch,"混沌", Colors.Glitch);
                if (CustomGameOptions.PlaguebearerOn > 0 && !PlayerControl.LocalPlayer.Is(RoleEnum.Plaguebearer)) AddToMap(RoleEnum.Plaguebearer,"瘟疫之源", Colors.Plaguebearer);
                if (CustomGameOptions.GameMode == GameMode.Classic && CustomGameOptions.VampireOn > 0 && !PlayerControl.LocalPlayer.Is(RoleEnum.Vampire)) AddToMap(RoleEnum.Vampire,"吸血鬼", Colors.Vampire);
                if (CustomGameOptions.WerewolfOn > 0 && !PlayerControl.LocalPlayer.Is(RoleEnum.Werewolf)) AddToMap(RoleEnum.Werewolf,"月下狼人", Colors.Werewolf);
                if (!PlayerControl.LocalPlayer.Is(RoleEnum.Juggernaut) && CustomGameOptions.HiddenRoles) AddToMap(RoleEnum.Juggernaut,"剑圣", Colors.Juggernaut);
            }

            if (CustomGameOptions.AssassinGuessImpostors && !PlayerControl.LocalPlayer.Is(Faction.Impostors))
            {
                AddToMap(RoleEnum.Impostor,"内鬼", Colors.Impostor);
                if (CustomGameOptions.JanitorOn > 0) AddToMap(RoleEnum.Janitor,"清洁工", Colors.Impostor);
                if (CustomGameOptions.MorphlingOn > 0) AddToMap(RoleEnum.Morphling,"化形者", Colors.Impostor);
                if (CustomGameOptions.MinerOn > 0) AddToMap(RoleEnum.Miner,"矿工", Colors.Impostor);
                if (CustomGameOptions.SwooperOn > 0) AddToMap(RoleEnum.Swapper,"隐身人", Colors.Impostor);
                if (CustomGameOptions.UndertakerOn > 0) AddToMap(RoleEnum.Undertaker,"送葬者", Colors.Impostor);
                if (CustomGameOptions.EscapistOn > 0) AddToMap(RoleEnum.Escapist,"逃逸者", Colors.Impostor);
                if (CustomGameOptions.GrenadierOn > 0) AddToMap(RoleEnum.Grenadier,"掷弹兵", Colors.Impostor);
                if (CustomGameOptions.TraitorOn > 0) AddToMap(RoleEnum.Traitor,"背叛者", Colors.Impostor);
                if (CustomGameOptions.BlackmailerOn > 0) AddToMap(RoleEnum.Blackmailer,"勒索者", Colors.Impostor);
                if (CustomGameOptions.BomberOn > 0) AddToMap(RoleEnum.Bomber,"爆破手", Colors.Impostor);
                if (CustomGameOptions.WarlockOn > 0) AddToMap(RoleEnum.Warlock,"术士", Colors.Impostor);
                if (CustomGameOptions.VenererOn > 0) AddToMap(RoleEnum.Venerer,"终末猎手", Colors.Impostor);
            }

            // Add vanilla crewmate if enabled
            if (CustomGameOptions.AssassinCrewmateGuess) AddToMap(RoleEnum.Crewmate,"船员", Colors.Crewmate);
            //Add modifiers if enabled
            if (CustomGameOptions.AssassinGuessModifiers && CustomGameOptions.BaitOn > 0) AddToMap(ModifierEnum.Bait,"诱饵", Colors.Bait);
            if (CustomGameOptions.AssassinGuessModifiers && CustomGameOptions.AftermathOn > 0) AddToMap(ModifierEnum.Aftermath,"惹不起的人", Colors.Aftermath);
            if (CustomGameOptions.AssassinGuessModifiers && CustomGameOptions.DiseasedOn > 0) AddToMap(ModifierEnum.Diseased,"病人", Colors.Diseased);
            if (CustomGameOptions.AssassinGuessModifiers && CustomGameOptions.FrostyOn > 0) AddToMap(ModifierEnum.Frosty,"圣诞老人", Colors.Frosty);
            if (CustomGameOptions.AssassinGuessModifiers && CustomGameOptions.MultitaskerOn > 0) AddToMap(ModifierEnum.Multitasker,"多线程", Colors.Multitasker);
            if (CustomGameOptions.AssassinGuessModifiers && CustomGameOptions.TorchOn > 0) AddToMap(ModifierEnum.Torch,"火炬手", Colors.Torch);
            if (CustomGameOptions.AssassinGuessLovers && CustomGameOptions.LoversOn > 0) AddToMap(ModifierEnum.Lover,"恋人", Colors.Lovers);

            // Sorts the list alphabetically. 
            SortedColorMapping = ColorMapping.OrderBy(x => x.Key).ToDictionary(x => x.Key, x => x.Value);
        }

        private void AddToMap(RoleEnum type, string name, Color color)
        {
            ColorMapping.Add(name,color);
            ColorMapRoleKey.Add(type, name);
        }

        private void AddToMap(ModifierEnum type, string name, Color color)
        {
            ColorMapping.Add(name, color);
            ColorMapModifierKey.Add(type, name);
        }

        public RoleEnum GetRoleEnum(string name)
        {
            var list = ColorMapRoleKey.Where(n => n.Value == name);
            if (list == null) return RoleEnum.None;
            return list.FirstOrDefault().Key;
        }

        public ModifierEnum GetModifierEnum(string name)
        {
            var list = ColorMapModifierKey.Where(n => n.Value == name);
            if (list == null) return ModifierEnum.Null;
            return list.FirstOrDefault().Key;
        }

        public bool GuessedThisMeeting { get; set; } = false;

        public int RemainingKills { get; set; }

        public List<string> PossibleGuesses => SortedColorMapping.Keys.ToList();
    }
}
