using System.Collections.Generic;
using System.Linq;
using HarmonyLib;
using UnityEngine;

namespace TownOfUs;

[HarmonyPatch(typeof(MeetingHud))]
public class MeetingHudPlayerVoteAreaPlugin
{
    public static int page = 1;
    public static int oldPage;
    public static int maxPage;
    public static int count;

    public static List<PlayerVoteArea> Targets => DestroyableSingleton<MeetingHud>.Instance.playerStates.OrderBy(p => p.AmDead).ToList();

    [HarmonyPatch(nameof(MeetingHud.Awake)), HarmonyPostfix]
    public static void AwakePatch(MeetingHud __instance)
    {
        count = __instance.playerStates.Count();

        if (count % 15 == 0)
            maxPage = count / 15;
        else
            maxPage = (count / 15) + 1;

        oldPage = page;
    }

    [HarmonyPatch(nameof(MeetingHud.Start)), HarmonyPostfix]
    public static void StartPatch(MeetingHud __instance) => UpdateButton(__instance);

    [HarmonyPatch(nameof(MeetingHud.Update)), HarmonyPostfix]
    public static void UpdatePatch(MeetingHud __instance)
    {
        if(Input.GetKeyDown(KeyCode.Less) && page - 1 > 0) page--;
        if(Input.GetKeyDown(KeyCode.Greater) && page + 1 <= maxPage) page++;

        checkUpdate(__instance);
    }

    public static void checkUpdate(MeetingHud __instance)
    {
        if (oldPage == page) return;
        UpdateButton(__instance);
    }

    public static void UpdateButton(MeetingHud __instance)
    {
        var endPagePlayer = (page * 15) - 1;
        var startPagePlayer = endPagePlayer - 14;
        var xrow = 0;
        var yrow = 0;
        for (var i = 0; i < count; i++)
        {
            var Button = Targets[i];

            if (i >= startPagePlayer && i <= endPagePlayer)
            {
                Button.gameObject.SetActive(true);
                Button.transform.localPosition = __instance.VoteOrigin +
                new Vector3
                (
                    __instance.VoteButtonOffsets.x * xrow,
                    __instance.VoteButtonOffsets.y * yrow,
                    i * 0.01f
                );
            }
            else
            {
                Button.gameObject.SetActive(false);
            }

            if (xrow + 1 < 3)
                xrow++;
            else
                xrow = 0;
                yrow++;
        }

        oldPage = page;
    }
}