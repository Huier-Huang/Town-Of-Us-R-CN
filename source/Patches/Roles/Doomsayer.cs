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

        public Dictionary<string, Color> SortedColorMapping;

        public Dictionary<byte, string> Guesses = new Dictionary<byte, string>();
        public DateTime LastObserved;
        public PlayerControl ClosestPlayer;
        public PlayerControl LastObservedPlayer;

        public Doomsayer(PlayerControl player) : base(player)
        {
            Name = "末日预示者";
            ImpostorText = () => "Guess People's Roles To Win!";
            TaskText = () => "Win by guessing player's roles\nFake Tasks:";
            Color = Patches.Colors.Doomsayer;
            RoleType = RoleEnum.Doomsayer;
            LastObserved = DateTime.UtcNow;
            AddToRoleHistory(RoleType);
            Faction = Faction.NeutralEvil;

            if (CustomGameOptions.GameMode == GameMode.Classic || CustomGameOptions.GameMode == GameMode.AllAny)
            {
                ColorMapping.Add("船员", Colors.Crewmate);
                if (CustomGameOptions.MayorOn > 0) ColorMapping.Add("市长", Colors.Mayor);
                if (CustomGameOptions.SheriffOn > 0 || (CustomGameOptions.VampireHunterOn > 0 && CustomGameOptions.GameMode == GameMode.Classic && CustomGameOptions.VampireOn > 0 && CustomGameOptions.BecomeOnVampDeaths == BecomeEnum.Sheriff)) ColorMapping.Add("警长", Colors.Sheriff);
                if (CustomGameOptions.EngineerOn > 0) ColorMapping.Add("工程师", Colors.Engineer);
                if (CustomGameOptions.SwapperOn > 0) ColorMapping.Add("Swapper", Colors.Swapper);
                if (CustomGameOptions.InvestigatorOn > 0) ColorMapping.Add("调查员", Colors.Investigator);
                if (CustomGameOptions.MedicOn > 0) ColorMapping.Add("Medic", Colors.Medic);
                if (CustomGameOptions.SeerOn > 0) ColorMapping.Add("预言家", Colors.Seer);
                if (CustomGameOptions.SpyOn > 0) ColorMapping.Add("间谍", Colors.Spy);
                if (CustomGameOptions.SnitchOn > 0) ColorMapping.Add("告密者", Colors.Snitch);
                if (CustomGameOptions.AltruistOn > 0) ColorMapping.Add("Altruist", Colors.Altruist);
                if (CustomGameOptions.VigilanteOn > 0 || (CustomGameOptions.VampireHunterOn > 0 && CustomGameOptions.GameMode == GameMode.Classic && CustomGameOptions.VampireOn > 0 && CustomGameOptions.BecomeOnVampDeaths == BecomeEnum.Vigilante)) ColorMapping.Add("侠客", Colors.Vigilante);
                if (CustomGameOptions.VeteranOn > 0 || (CustomGameOptions.VampireHunterOn > 0 && CustomGameOptions.GameMode == GameMode.Classic && CustomGameOptions.VampireOn > 0 && CustomGameOptions.BecomeOnVampDeaths == BecomeEnum.Veteran)) ColorMapping.Add("Veteran", Colors.Veteran);
                if (CustomGameOptions.TrackerOn > 0) ColorMapping.Add("Tracker", Colors.Tracker);
                if (CustomGameOptions.TrapperOn > 0) ColorMapping.Add("Trapper", Colors.Trapper);
                if (CustomGameOptions.TransporterOn > 0) ColorMapping.Add("传送师", Colors.Transporter);
                if (CustomGameOptions.MediumOn > 0) ColorMapping.Add("Medium", Colors.Medium);
                if (CustomGameOptions.MysticOn > 0) ColorMapping.Add("招魂师", Colors.Mystic);
                if (CustomGameOptions.DetectiveOn > 0) ColorMapping.Add("Detective", Colors.Detective);
                if (CustomGameOptions.ImitatorOn > 0) ColorMapping.Add("Imitator", Colors.Imitator);
                if (CustomGameOptions.VampireHunterOn > 0 && CustomGameOptions.GameMode == GameMode.Classic && CustomGameOptions.VampireOn > 0) ColorMapping.Add("Vampire Hunter", Colors.VampireHunter);
                if (CustomGameOptions.ProsecutorOn > 0) ColorMapping.Add("Prosecutor", Colors.Prosecutor);
                if (CustomGameOptions.OracleOn > 0) ColorMapping.Add("Oracle", Colors.Oracle);
                if (CustomGameOptions.AurialOn > 0) ColorMapping.Add("修仙者", Colors.Aurial);

                if (CustomGameOptions.DoomsayerGuessImpostors && !PlayerControl.LocalPlayer.Is(Faction.Impostors))
                {
                    ColorMapping.Add("Impostor", Colors.Impostor);
                    if (CustomGameOptions.JanitorOn > 0) ColorMapping.Add("清洁工", Colors.Impostor);
                    if (CustomGameOptions.MorphlingOn > 0) ColorMapping.Add("化形者", Colors.Impostor);
                    if (CustomGameOptions.MinerOn > 0) ColorMapping.Add("矿工", Colors.Impostor);
                    if (CustomGameOptions.SwooperOn > 0) ColorMapping.Add("隐身人", Colors.Impostor);
                    if (CustomGameOptions.UndertakerOn > 0) ColorMapping.Add("送葬者", Colors.Impostor);
                    if (CustomGameOptions.EscapistOn > 0) ColorMapping.Add("逃逸者", Colors.Impostor);
                    if (CustomGameOptions.GrenadierOn > 0) ColorMapping.Add("掷弹兵", Colors.Impostor);
                    if (CustomGameOptions.TraitorOn > 0) ColorMapping.Add("叛徒", Colors.Impostor);
                    if (CustomGameOptions.BlackmailerOn > 0) ColorMapping.Add("勒索者", Colors.Impostor);
                    if (CustomGameOptions.BomberOn > 0) ColorMapping.Add("爆破手", Colors.Impostor);
                    if (CustomGameOptions.WarlockOn > 0) ColorMapping.Add("术士", Colors.Impostor);
                    if (CustomGameOptions.VenererOn > 0) ColorMapping.Add("终末猎手", Colors.Impostor);
                }

                if (CustomGameOptions.DoomsayerGuessNeutralBenign)
                {
                    if (CustomGameOptions.AmnesiacOn > 0 || (CustomGameOptions.ExecutionerOn > 0 && CustomGameOptions.OnTargetDead == OnTargetDead.Amnesiac) || (CustomGameOptions.GuardianAngelOn > 0 && CustomGameOptions.GaOnTargetDeath == BecomeOptions.Amnesiac)) ColorMapping.Add("失忆者", Colors.Amnesiac);
                    if (CustomGameOptions.GuardianAngelOn > 0) ColorMapping.Add("守护天使", Colors.GuardianAngel);
                    if (CustomGameOptions.SurvivorOn > 0 || (CustomGameOptions.ExecutionerOn > 0 && CustomGameOptions.OnTargetDead == OnTargetDead.Survivor) || (CustomGameOptions.GuardianAngelOn > 0 && CustomGameOptions.GaOnTargetDeath == BecomeOptions.Survivor)) ColorMapping.Add("幸存者", Colors.Survivor);
                }
                if (CustomGameOptions.DoomsayerGuessNeutralEvil)
                {
                    if (CustomGameOptions.GameMode == GameMode.AllAny) ColorMapping.Add("末日预示者", Colors.Doomsayer);
                    if (CustomGameOptions.ExecutionerOn > 0) ColorMapping.Add("处刑人", Colors.Executioner);
                    if (CustomGameOptions.JesterOn > 0 || (CustomGameOptions.ExecutionerOn > 0 && CustomGameOptions.OnTargetDead == OnTargetDead.Jester) || (CustomGameOptions.GuardianAngelOn > 0 && CustomGameOptions.GaOnTargetDeath == BecomeOptions.Jester)) ColorMapping.Add("小丑", Colors.Jester);
                }
                if (CustomGameOptions.DoomsayerGuessNeutralKilling)
                {
                    if (CustomGameOptions.ArsonistOn > 0) ColorMapping.Add("纵火犯", Colors.Arsonist);
                    if (CustomGameOptions.GlitchOn > 0) ColorMapping.Add("混沌", Colors.Glitch);
                    if (CustomGameOptions.PlaguebearerOn > 0) ColorMapping.Add("瘟疫之源", Colors.Plaguebearer);
                    if (CustomGameOptions.GameMode == GameMode.Classic && CustomGameOptions.VampireOn > 0) ColorMapping.Add("吸血鬼", Colors.Vampire);
                    if (CustomGameOptions.WerewolfOn > 0) ColorMapping.Add("狼人", Colors.Werewolf);
                    if (CustomGameOptions.HiddenRoles) ColorMapping.Add("剑圣", Colors.Juggernaut);
                }
            }

            SortedColorMapping = ColorMapping.OrderBy(x => x.Key).ToDictionary(x => x.Key, x => x.Value);
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
