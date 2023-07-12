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
        private Dictionary<RoleEnum, string> ColorMapRoleKey = new Dictionary<RoleEnum, string>();
        private Dictionary<ModifierEnum, string> ColorMapModifierKey = new Dictionary<ModifierEnum, string>();
        private Dictionary<AbilityEnum, string> ColorMapAbilityKey = new Dictionary<AbilityEnum, string>();

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
                AddToMap(RoleEnum.Impostor, "内鬼", Colors.Impostor);
                if (CustomGameOptions.JanitorOn > 0) AddToMap(RoleEnum.Janitor, "清洁工", Colors.Impostor);
                if (CustomGameOptions.MorphlingOn > 0) AddToMap(RoleEnum.Morphling, "化形者", Colors.Impostor);
                if (CustomGameOptions.MinerOn > 0) AddToMap(RoleEnum.Miner, "矿工", Colors.Impostor);
                if (CustomGameOptions.SwooperOn > 0) AddToMap(RoleEnum.Swapper, "隐身人", Colors.Impostor);
                if (CustomGameOptions.UndertakerOn > 0) AddToMap(RoleEnum.Undertaker, "送葬者", Colors.Impostor);
                if (CustomGameOptions.EscapistOn > 0) AddToMap(RoleEnum.Escapist, "逃逸者", Colors.Impostor);
                if (CustomGameOptions.GrenadierOn > 0) AddToMap(RoleEnum.Grenadier, "掷弹兵", Colors.Impostor);
                if (CustomGameOptions.TraitorOn > 0) AddToMap(RoleEnum.Traitor, "背叛者", Colors.Impostor);
                if (CustomGameOptions.BlackmailerOn > 0) AddToMap(RoleEnum.Blackmailer, "勒索者", Colors.Impostor);
                if (CustomGameOptions.BomberOn > 0) AddToMap(RoleEnum.Bomber, "爆破手", Colors.Impostor);
                if (CustomGameOptions.WarlockOn > 0) AddToMap(RoleEnum.Warlock, "术士", Colors.Impostor);
                if (CustomGameOptions.VenererOn > 0) AddToMap(RoleEnum.Venerer, "终末猎手", Colors.Impostor);

                if (CustomGameOptions.VigilanteGuessNeutralBenign)
                {
                    if (CustomGameOptions.AmnesiacOn > 0 || (CustomGameOptions.ExecutionerOn > 0 && CustomGameOptions.OnTargetDead == OnTargetDead.Amnesiac) || (CustomGameOptions.GuardianAngelOn > 0 && CustomGameOptions.GaOnTargetDeath == BecomeOptions.Amnesiac)) AddToMap(RoleEnum.Amnesiac, "失忆者", Colors.Amnesiac);
                    if (CustomGameOptions.GuardianAngelOn > 0) AddToMap(RoleEnum.GuardianAngel, "守护天使", Colors.GuardianAngel);
                    if (CustomGameOptions.SurvivorOn > 0 || (CustomGameOptions.ExecutionerOn > 0 && CustomGameOptions.OnTargetDead == OnTargetDead.Survivor) || (CustomGameOptions.GuardianAngelOn > 0 && CustomGameOptions.GaOnTargetDeath == BecomeOptions.Survivor)) AddToMap(RoleEnum.Survivor, "幸存者", Colors.Survivor);
                }

                if (CustomGameOptions.VigilanteGuessNeutralEvil)
                {
                    if (CustomGameOptions.DoomsayerOn > 0) AddToMap(RoleEnum.Doomsayer, "末日预示者", Colors.Doomsayer);
                    if (CustomGameOptions.ExecutionerOn > 0) AddToMap(RoleEnum.Executioner, "处刑人", Colors.Executioner);
                    if (CustomGameOptions.JesterOn > 0 || (CustomGameOptions.ExecutionerOn > 0 && CustomGameOptions.OnTargetDead == OnTargetDead.Jester) || (CustomGameOptions.GuardianAngelOn > 0 && CustomGameOptions.GaOnTargetDeath == BecomeOptions.Jester)) AddToMap(RoleEnum.Jester, "小丑", Colors.Jester);
                }

                if (CustomGameOptions.VigilanteGuessNeutralKilling)
                {
                    if (CustomGameOptions.ArsonistOn > 0) AddToMap(RoleEnum.Arsonist, "纵火犯", Colors.Arsonist);
                    if (CustomGameOptions.GlitchOn > 0) AddToMap(RoleEnum.Glitch, "混沌", Colors.Glitch);
                    if (CustomGameOptions.PlaguebearerOn > 0) AddToMap(RoleEnum.Plaguebearer, "瘟疫之源", Colors.Plaguebearer);
                    if (CustomGameOptions.GameMode == GameMode.Classic && CustomGameOptions.VampireOn > 0) AddToMap(RoleEnum.Vampire, "吸血鬼", Colors.Vampire);
                    if (CustomGameOptions.WerewolfOn > 0) AddToMap(RoleEnum.Werewolf, "月下狼人", Colors.Werewolf);
                    if (CustomGameOptions.HiddenRoles) AddToMap(RoleEnum.Juggernaut, "剑圣", Colors.Juggernaut);
                }

                if (CustomGameOptions.VigilanteGuessLovers && CustomGameOptions.LoversOn > 0) AddToMap(ModifierEnum.Lover,"恋人", Colors.Lovers);
            }
            else if (CustomGameOptions.GameMode == GameMode.KillingOnly)
            {
                AddToMap(RoleEnum.Morphling,"化形者", Colors.Impostor);
                AddToMap(RoleEnum.Miner,"矿工", Colors.Impostor);
                AddToMap(RoleEnum.Swooper, "隐身人", Colors.Impostor);
                AddToMap(RoleEnum.Undertaker, "送葬者", Colors.Impostor);
                AddToMap(RoleEnum.Escapist, "逃逸者", Colors.Impostor);
                AddToMap(RoleEnum.Grenadier, "掷弹兵", Colors.Impostor);
                AddToMap(RoleEnum.Traitor, "背叛者", Colors.Impostor);

                if (CustomGameOptions.VigilanteGuessNeutralKilling)
                {
                    if (CustomGameOptions.AddArsonist) AddToMap(RoleEnum.Arsonist, "纵火犯", Colors.Arsonist);
                    if (CustomGameOptions.AddPlaguebearer) AddToMap(RoleEnum.Plaguebearer, "瘟疫之源", Colors.Plaguebearer);
                    AddToMap(RoleEnum.Glitch, "混沌", Colors.Glitch);
                    AddToMap(RoleEnum.Werewolf, "月下狼人", Colors.Werewolf);
                    if (CustomGameOptions.HiddenRoles) AddToMap(RoleEnum.Juggernaut, "剑圣", Colors.Juggernaut);
                }
            }
            else
            {
                AddToMap(RoleEnum.Necromancer,"死灵法师", Colors.Impostor);
                AddToMap(RoleEnum.Whisperer,"耳语者", Colors.Impostor);
                if (CustomGameOptions.MaxChameleons > 0) AddToMap(RoleEnum.Swooper,"隐身人", Colors.Impostor);
                if (CustomGameOptions.MaxEngineers > 0) AddToMap(RoleEnum.Engineer,"破坏者", Colors.Impostor);
                if (CustomGameOptions.MaxInvestigators > 0) AddToMap(RoleEnum.Investigator,"参谋", Colors.Impostor);
                if (CustomGameOptions.MaxMystics > 0) AddToMap(RoleEnum.CultistMystic,"先觉者", Colors.Impostor);
                if (CustomGameOptions.MaxSnitches > 0) AddToMap(RoleEnum.CultistSnitch,"远见之人", Colors.Impostor);
                if (CustomGameOptions.MaxSpies > 0) AddToMap(RoleEnum.Spy,"流氓", Colors.Impostor);
                if (CustomGameOptions.MaxTransporters > 0) AddToMap(RoleEnum.Escapist,"逃逸者", Colors.Impostor);
                if (CustomGameOptions.MaxVigilantes > 1) AddToMap(AbilityEnum.Assassin,"刺客", Colors.Impostor);
                AddToMap(RoleEnum.Impostor,"内鬼", Colors.Impostor);
            }

            SortedColorMapping = ColorMapping.OrderBy(x => x.Key).ToDictionary(x => x.Key, x => x.Value);
        }

        private void AddToMap(RoleEnum type, string name, Color color)
        {
            ColorMapping.Add(name, color);
            ColorMapRoleKey.Add(type, name);
        }

        private void AddToMap(ModifierEnum type, string name, Color color)
        {
            ColorMapping.Add(name, color);
            ColorMapModifierKey.Add(type, name);
        }

        private void AddToMap(AbilityEnum type, string name, Color color)
        {
            ColorMapping.Add(name, color);
            ColorMapAbilityKey.Add(type, name);
        }

        public RoleEnum GetRoleEnum(string name)
        {
            var list = ColorMapRoleKey.Where(n => n.Value == name);
            if(list == null) return RoleEnum.None;
            return list.FirstOrDefault().Key;
        }

        public ModifierEnum GetModifierEnum(string name)
        {
            var list = ColorMapModifierKey.Where(n => n.Value == name);
            if(list == null) return ModifierEnum.Null;
            return list.FirstOrDefault().Key;
        }
        public AbilityEnum GetAbilityEnum(string name)
        {
            var list = ColorMapAbilityKey.Where(n => n.Value == name);
            if(list == null) return AbilityEnum.Null;
            return list.FirstOrDefault().Key;
        }

        public bool GuessedThisMeeting { get; set; } = false;

        public int RemainingKills { get; set; }

        public List<string> PossibleGuesses => SortedColorMapping.Keys.ToList();
    }
}
