using System;
using HarmonyLib;
using RimWorld;
using RimWorld.Planet;
using Verse;

namespace ImpassableMapMaker;

[HarmonyPatch(typeof(GenStep_FindPlayerStartSpot), nameof(GenStep_FindPlayerStartSpot.Generate))]
internal static class GenStep_FindPlayerStartSpot_Generate
{
    public static ITerrainOverride QuestArea = null;

    private static void Postfix(Map map)
    {
        if (map.TileInfo.hilliness != Hilliness.Impassable ||
            GenStep_ElevationFertility_Generate.MiddleAreaCenter == null ||
            !Settings.HasMiddleArea ||
            !Settings.StartInMiddleArea)
        {
            return;
        }

        float centerX = GenStep_ElevationFertility_Generate.MiddleAreaCenter.Value.x;
        float centerZ = GenStep_ElevationFertility_Generate.MiddleAreaCenter.Value.x;
        var halfX = Settings.OpenAreaSizeX * 0.5f;
        var halfZ = Settings.OpenAreaSizeZ * 0.5f;
        var minX = centerX - halfX;
        var minZ = centerZ - halfZ;
        var maxX = centerX + halfX;
        var maxZ = centerZ + halfZ;

        if (CellFinderLoose.TryFindRandomNotEdgeCellWith(
                (int)Math.Max(0, map.Size.x - centerX - halfX + 1),
                i => !i.Roofed(map) && i.x >= minX && i.x <= maxX && i.z >= minZ && i.z <= maxZ,
                map,
                out var result))
        {
            MapGenerator.PlayerStartSpot = result;
        }
        else
        {
            Log.Error("Unable to start in the middle. Sorry!");
        }
    }
}