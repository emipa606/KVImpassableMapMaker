using System;
using HarmonyLib;
using RimWorld.Planet;
using Verse;

namespace ImpassableMapMaker;

[HarmonyPatch(typeof(CaravanEnterMapUtility), nameof(CaravanEnterMapUtility.Enter), typeof(Caravan), typeof(Map),
    typeof(CaravanEnterMode), typeof(CaravanDropInventoryMode), typeof(bool), typeof(Predicate<IntVec3>))]
internal static class CaravanEnterMapUtility_Enter
{
    [HarmonyPriority(Priority.First)]
    private static void Prefix(Map map, ref CaravanEnterMode enterMode, bool draftColonists)
    {
        if (map.TileInfo.hilliness != Hilliness.Impassable)
        {
            return;
        }

        enterMode = draftColonists ? CaravanEnterMode.Center : CaravanEnterMode.Edge;
    }
}