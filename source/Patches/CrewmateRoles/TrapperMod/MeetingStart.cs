using HarmonyLib;
using System;
using System.Linq;
using TownOfUs.Roles;

namespace TownOfUs.CrewmateRoles.TrapperMod
{
    [HarmonyPatch(typeof(MeetingHud), nameof(MeetingHud.Start))]
    public class MeetingStart
    {
        public static void Postfix(MeetingHud __instance)
        {
            if (PlayerControl.LocalPlayer.Data.IsDead) return;
            if (!PlayerControl.LocalPlayer.Is(RoleEnum.Trapper)) return;
            var trapperRole = Role.GetRole<Trapper>(PlayerControl.LocalPlayer);
            if (trapperRole.trappedPlayers.Count == 0)
            {
                DestroyableSingleton<HudManager>.Instance.Chat.AddChat(PlayerControl.LocalPlayer, "没有玩家进入你的陷阱");
            }
            else if (trapperRole.trappedPlayers.Count < CustomGameOptions.MinAmountOfPlayersInTrap)
            {
                DestroyableSingleton<HudManager>.Instance.Chat.AddChat(PlayerControl.LocalPlayer, "没有足够的玩家触发你的陷阱");
            }
            else
            {
                string message = "落入陷阱的职业:\n";
                foreach (RoleEnum role in trapperRole.trappedPlayers.OrderBy(x => Guid.NewGuid()))
                {
                    message += $" {role},";
                }
                message.Remove(message.Length - 1, 1);
                if (DestroyableSingleton<HudManager>.Instance)
                    DestroyableSingleton<HudManager>.Instance.Chat.AddChat(PlayerControl.LocalPlayer, message);
            }
        }
    }
}
