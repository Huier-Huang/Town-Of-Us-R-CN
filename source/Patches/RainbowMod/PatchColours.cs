using HarmonyLib;
using Il2CppInterop.Runtime.InteropTypes.Arrays;

namespace TownOfUs.RainbowMod
{
    [HarmonyPatch(typeof(TranslationController), nameof(TranslationController.GetString),
        new[] { typeof(StringNames), typeof(Il2CppReferenceArray<Il2CppSystem.Object>) })]
    public class PatchColours
    {
        public static bool Prefix(ref string __result, [HarmonyArgument(0)] StringNames name)
        {
            var newResult = (int)name switch
            {
                999983 => "西瓜色",
                999984 => "巧克力色",
                999985 => "天蓝色",
                999986 => "米色",
                999987 => "品红色",
                999988 => "绿松石色",
                999989 => "淡紫色",
                999990 => "橄榄色",
                999991 => "蔚蓝色",
                999992 => "李子色",
                999993 => "丛林色",
                999994 => "薄荷色",
                999995 => "黄绿色",
                999996 => "神秘的绿色",
                999997 => "茶色",
                999998 => "金色",
                999999 => "彩虹",
                _ => null
            };
            if (newResult != null)
            {
                __result = newResult;
                return false;
            }

            return true;
        }
    }
}
