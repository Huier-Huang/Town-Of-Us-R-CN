using System.IO;
using System.Collections.Generic;
using System.Linq;
using HarmonyLib;
using UnityEngine;
using TMPro;

namespace TownOfUs;

[HarmonyPatch(typeof(MeetingHud))]
public class MeetingHudPlayerVoteAreaPlugin
{
    public static int page = 1;
    public static int oldPage;
    public static int maxPage;
    public static int count;

    public static bool NoEnable => File.Exists("./BepInEx/plugins/CrowdedMod.dll") || Targets.Count() <= 15;

    public static GameObject pageText;

    public static List<PlayerVoteArea> Targets => DestroyableSingleton<MeetingHud>.Instance.playerStates.OrderBy(p => p.AmDead).ToList();

    [HarmonyPatch(nameof(MeetingHud.Awake)), HarmonyPostfix]
    public static void AwakePatch(MeetingHud __instance)
    {
        if(NoEnable) return;
        count = __instance.playerStates.Count();

        if (count % 15 == 0)
            maxPage = count / 15;
        else
            maxPage = (count / 15) + 1;

        oldPage = page;
    }

    [HarmonyPatch(nameof(MeetingHud.Start)), HarmonyPostfix]
    public static void StartPatch(MeetingHud __instance)
    {
        if (NoEnable) return;
        UpdateButton(__instance);
        var te = GameObject.Find("TitleText_TMP");
        pageText = GameObject.Instantiate(te, __instance.transform);
        pageText.transform.localPosition = new Vector3(2.1597f, 3.2031f, -11);
        Object.Destroy(pageText.GetComponent<TextTranslatorTMP>());
        pageText.GetComponent<TextMeshPro>().text = $"'<':上一页 '>':下一页 当前页数:{page}";
    }

    [HarmonyPatch(nameof(MeetingHud.Update)), HarmonyPostfix]
    public static void UpdatePatch(MeetingHud __instance)
    {
        if (NoEnable) return;
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