using System.Collections.Generic;
using HarmonyLib;
using Verse;

namespace ImpassableMapMaker;

[HarmonyPatch(typeof(RimWorld.GenStep_ScattererBestFit), "TryFindScatterCell")]
internal static class GenStep_ScattererBestFit
{
    [HarmonyPriority(Priority.First)]
    private static bool Prefix(RimWorld.GenStep_ScattererBestFit __instance, ref bool __result, Map map,
        ref IntVec3 result)
    {
        if (!__instance.def.defName.Contains("Archonexus"))
        {
            return true;
        }

        var o = GenStep_ElevationFertility_Generate.QuestArea;
        result = new IntVec3(o.Center.x, o.Center.y, o.Center.z);
        for (var x = o.LowX; x <= o.HighX; ++x)
        {
            for (var z = o.LowZ; z <= o.HighZ; ++z)
            {
                var i = new IntVec3(x, 0, z);
                if (!o.IsInside(i))
                {
                    continue;
                }

                var s = new Stack<Thing>(map.thingGrid.ThingsAt(i));
                while (s.Count > 0)
                {
                    var t = s.Pop();
                    if (!t.def.destroyable)
                    {
                        t.DeSpawn();
                    }
                }
            }
        }

        __result = true;
        return false;
    }
}