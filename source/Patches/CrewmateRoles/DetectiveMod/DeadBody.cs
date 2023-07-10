using System;
using TownOfUs.Roles;

namespace TownOfUs.CrewmateRoles.DetectiveMod
{
    public class BodyReport
    {
        public PlayerControl Killer { get; set; }
        public PlayerControl Reporter { get; set; }
        public PlayerControl Body { get; set; }
        public float KillAge { get; set; }

        public static string ParseBodyReport(BodyReport br)
        {
            if (br.KillAge > CustomGameOptions.DetectiveFactionDuration * 1000)
                return
                    $"尸体报告:已腐烂了,无法获得信息. (死亡时间:{Math.Round(br.KillAge / 1000)}s 之前)";

            if (br.Killer.PlayerId == br.Body.PlayerId)
                return
                    $"尸体报告:死者似乎是自杀! (死亡时间:{Math.Round(br.KillAge / 1000)}s 之前)";

            var role = Role.GetRole(br.Killer);

            if (br.KillAge < CustomGameOptions.DetectiveRoleDuration * 1000)
                return
                    $"尸体报告:凶手似乎是一名{role.Name}! (死亡时间:{Math.Round(br.KillAge / 1000)}s 之前)";

            if (br.Killer.Is(Faction.Crewmates))
                return
                    $"尸体报告:凶手似乎是一名船员! (死亡时间:{Math.Round(br.KillAge / 1000)}s 之前)";

            else if (br.Killer.Is(Faction.NeutralKilling) || br.Killer.Is(Faction.NeutralBenign))
                return
                    $"尸体报告:凶手似乎是中立角色! (死亡时间:{Math.Round(br.KillAge / 1000)}s 之前)";

            else
                return
                    $"尸体报告:凶手似乎是一名内鬼! (死亡时间:{Math.Round(br.KillAge / 1000)}s 之前)";
        }
    }
}
