using System.Collections.Generic;
using System.Linq;
using TMPro;
using TownOfUs.Patches;
using UnityEngine;
using TownOfUs.NeutralRoles.ExecutionerMod;
using TownOfUs.NeutralRoles.GuardianAngelMod;
using System;
using TownOfUs.CrewmateRoles.VampireHunterMod;

namespace TownOfUs.Roles
{
    public class Doomsayer : Role
    {
        public Dictionary<byte, (GameObject, GameObject, GameObject, TMP_Text)> Buttons = new Dictionary<byte, (GameObject, GameObject, GameObject, TMP_Text)>();

        private Dictionary<string, Color> ColorMapping = new Dictionary<string, Color>();
        private Dictionary<RoleEnum, string> ColorMapRoleKey = new Dictionary<RoleEnum, string>();
        private Dictionary<ModifierEnum, string> ColorMapModifierKey = new Dictionary<ModifierEnum, string>();

        public Dictionary<string, Color> SortedColorMapping;

        public Dictionary<byte, string> Guesses = new Dictionary<byte, string>();
        public DateTime LastObserved;
        public PlayerControl ClosestPlayer;
        public PlayerControl LastObservedPlayer;

        public Doomsayer(PlayerControl player) : base(player)
        {
            Name = "末日预示者";
            ImpostorText = () => "猜测人们的角色来获胜!";
            TaskText = () => "通过猜测玩家的角色获胜\n假任务:";
            Color = Patches.Colors.Doomsayer;
            RoleType = RoleEnum.Doomsayer;
            LastObserved = DateTime.UtcNow;
            AddToRoleHistory(RoleType);
            Faction = Faction.NeutralEvil;

            if (CustomGameOptions.GameMode == GameMode.Classic || CustomGameOptions.GameMode == GameMode.AllAny)
            {
                AddToMap(RoleEnum.Crewmate,"船员", Colors.Crewmate);
                if (CustomGameOptions.MayorOn > 0) AddToMap(RoleEnum.Mayor, "市长", Colors.Mayor);
                if (CustomGameOptions.SheriffOn > 0 || (CustomGameOptions.VampireHunterOn > 0 && CustomGameOptions.GameMode == GameMode.Classic && CustomGameOptions.VampireOn > 0 && CustomGameOptions.BecomeOnVampDeaths == BecomeEnum.Sheriff)) AddToMap(RoleEnum.Sheriff, "警长", Colors.Sheriff);
                if (CustomGameOptions.EngineerOn > 0) AddToMap(RoleEnum.Engineer, "工程师", Colors.Engineer);
                if (CustomGameOptions.SwapperOn > 0) AddToMap(RoleEnum.Swapper, "换票师", Colors.Swapper);
                if (CustomGameOptions.InvestigatorOn > 0) AddToMap(RoleEnum.Investigator, "调查员", Colors.Investigator);
                if (CustomGameOptions.MedicOn > 0) AddToMap(RoleEnum.Medic, "法医", Colors.Medic);
                if (CustomGameOptions.SeerOn > 0) AddToMap(RoleEnum.Seer, "预言家", Colors.Seer);
                if (CustomGameOptions.SpyOn > 0) AddToMap(RoleEnum.Spy, "特工", Colors.Spy);
                if (CustomGameOptions.SnitchOn > 0) AddToMap(RoleEnum.Snitch, "告密者", Colors.Snitch);
                if (CustomGameOptions.AltruistOn > 0) AddToMap(RoleEnum.Altruist, "殉道者", Colors.Altruist);
                if (CustomGameOptions.VigilanteOn > 0 || (CustomGameOptions.VampireHunterOn > 0 && CustomGameOptions.GameMode == GameMode.Classic && CustomGameOptions.VampireOn > 0 && CustomGameOptions.BecomeOnVampDeaths == BecomeEnum.Vigilante)) AddToMap(RoleEnum.Vigilante, "侠客", Colors.Vigilante);
                if (CustomGameOptions.VeteranOn > 0 || (CustomGameOptions.VampireHunterOn > 0 && CustomGameOptions.GameMode == GameMode.Classic && CustomGameOptions.VampireOn > 0 && CustomGameOptions.BecomeOnVampDeaths == BecomeEnum.Veteran)) AddToMap(RoleEnum.Veteran, "老兵", Colors.Veteran);
                if (CustomGameOptions.TrackerOn > 0) AddToMap(RoleEnum.Tracker, "追踪者", Colors.Tracker);
                if (CustomGameOptions.TrapperOn > 0) AddToMap(RoleEnum.Trapper, "陷阱师", Colors.Trapper);
                if (CustomGameOptions.TransporterOn > 0) AddToMap(RoleEnum.Transporter, "传送师", Colors.Transporter);
                if (CustomGameOptions.MediumOn > 0) AddToMap(RoleEnum.Medium, "招魂师", Colors.Medium);
                if (CustomGameOptions.MysticOn > 0) AddToMap(RoleEnum.Mystic, "招魂", Colors.Mystic);
                if (CustomGameOptions.DetectiveOn > 0) AddToMap(RoleEnum.Detective, "侧写师", Colors.Detective);
                if (CustomGameOptions.ImitatorOn > 0) AddToMap(RoleEnum.Imitator, "效颦者", Colors.Imitator);
                if (CustomGameOptions.VampireHunterOn > 0 && CustomGameOptions.GameMode == GameMode.Classic && CustomGameOptions.VampireOn > 0) AddToMap(RoleEnum.VampireHunter, "吸血鬼杀手", Colors.VampireHunter);
                if (CustomGameOptions.ProsecutorOn > 0) AddToMap(RoleEnum.Prosecutor, "检察官", Colors.Prosecutor);
                if (CustomGameOptions.OracleOn > 0) AddToMap(RoleEnum.Oracle, "神之使徒", Colors.Oracle);
                if (CustomGameOptions.AurialOn > 0) AddToMap(RoleEnum.Aurial, "修仙者", Colors.Aurial);

                if (CustomGameOptions.DoomsayerGuessImpostors && !PlayerControl.LocalPlayer.Is(Faction.Impostors))
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
                }

                if (CustomGameOptions.DoomsayerGuessNeutralBenign)
                {

                    if (CustomGameOptions.AmnesiacOn > 0 || (CustomGameOptions.ExecutionerOn > 0 && CustomGameOptions.OnTargetDead == OnTargetDead.Amnesiac) || (CustomGameOptions.GuardianAngelOn > 0 && CustomGameOptions.GaOnTargetDeath == BecomeOptions.Amnesiac)) AddToMap(RoleEnum.Amnesiac, "失忆者", Colors.Amnesiac);
                    if (CustomGameOptions.GuardianAngelOn > 0) AddToMap(RoleEnum.GuardianAngel, "守护天使", Colors.GuardianAngel);
                    if (CustomGameOptions.SurvivorOn > 0 || (CustomGameOptions.ExecutionerOn > 0 && CustomGameOptions.OnTargetDead == OnTargetDead.Survivor) || (CustomGameOptions.GuardianAngelOn > 0 && CustomGameOptions.GaOnTargetDeath == BecomeOptions.Survivor)) AddToMap(RoleEnum.Survivor, "幸存者", Colors.Survivor);
                }

                if (CustomGameOptions.DoomsayerGuessNeutralEvil)
                {
                    if (CustomGameOptions.DoomsayerOn > 0) AddToMap(RoleEnum.Doomsayer, "末日预示者", Colors.Doomsayer);
                    if (CustomGameOptions.ExecutionerOn > 0) AddToMap(RoleEnum.Executioner, "处刑人", Colors.Executioner);
                    if (CustomGameOptions.JesterOn > 0 || (CustomGameOptions.ExecutionerOn > 0 && CustomGameOptions.OnTargetDead == OnTargetDead.Jester) || (CustomGameOptions.GuardianAngelOn > 0 && CustomGameOptions.GaOnTargetDeath == BecomeOptions.Jester)) AddToMap(RoleEnum.Jester, "小丑", Colors.Jester);
                }

                if (CustomGameOptions.DoomsayerGuessNeutralKilling)
                {
                    if (CustomGameOptions.ArsonistOn > 0) AddToMap(RoleEnum.Arsonist, "纵火犯", Colors.Arsonist);
                    if (CustomGameOptions.GlitchOn > 0) AddToMap(RoleEnum.Glitch, "混沌", Colors.Glitch);
                    if (CustomGameOptions.PlaguebearerOn > 0) AddToMap(RoleEnum.Plaguebearer, "瘟疫之源", Colors.Plaguebearer);
                    if (CustomGameOptions.GameMode == GameMode.Classic && CustomGameOptions.VampireOn > 0) AddToMap(RoleEnum.Vampire, "吸血鬼", Colors.Vampire);
                    if (CustomGameOptions.WerewolfOn > 0) AddToMap(RoleEnum.Werewolf, "月下狼人", Colors.Werewolf);
                    if (CustomGameOptions.HiddenRoles) AddToMap(RoleEnum.Juggernaut, "剑圣", Colors.Juggernaut);
                }
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

        public float ObserveTimer()
        {
            var utcNow = DateTime.UtcNow;
            var timeSpan = utcNow - LastObserved;
            var num = CustomGameOptions.ObserveCooldown * 1000f;
            var flag2 = num - (float)timeSpan.TotalMilliseconds < 0f;
            if (flag2) return 0;
            return (num - (float)timeSpan.TotalMilliseconds) / 1000f;
        }

        public int GuessedCorrectly = 0;
        public bool WonByGuessing = false;

        public List<string> PossibleGuesses => SortedColorMapping.Keys.ToList();

        protected override void IntroPrefix(IntroCutscene._ShowTeam_d__36 __instance)
        {
            var doomTeam = new Il2CppSystem.Collections.Generic.List<PlayerControl>();
            doomTeam.Add(PlayerControl.LocalPlayer);
            __instance.teamToShow = doomTeam;
        }

        internal override bool NeutralWin(LogicGameFlowNormal __instance)
        {
            if (Player.Data.IsDead) return true;
            if (!WonByGuessing) return true;
            Utils.EndGame();
            return false;
        }
    }
}
