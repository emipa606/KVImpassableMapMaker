using System.Collections.Generic;
using HarmonyLib;
using RimWorld.Planet;
using RimWorld.QuestGen;
using Verse;

namespace ImpassableMapMaker;

[HarmonyPatch(typeof(FastTileFinder), nameof(FastTileFinder.Query))]
internal static class FastTileFinder_Query
{
    private static void Postfix(ref List<PlanetTile> __result)
    {
        if (!Settings.NoQuestsOnImpassable || QuestGen.quest == null || __result is not { Count: > 0 })
        {
            return;
        }

        for (var i = __result.Count - 1; i >= 0; i--)
        {
            if (Find.WorldGrid[__result[i]].hilliness == Hilliness.Impassable)
            {
                __result.RemoveAt(i);
            }
        }
    }
}