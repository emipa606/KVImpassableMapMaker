using System;
using System.Collections.Generic;
using HarmonyLib;
using RimWorld;
using RimWorld.Planet;
using Verse;

namespace ImpassableMapMaker;

[HarmonyPatch(typeof(GenStep_ElevationFertility), nameof(GenStep_ElevationFertility.Generate))]
internal static class GenStep_ElevationFertility_Generate
{
    public static IntVec2? MiddleAreaCenter;
    private static HashSet<string> middleAreaCells;
    public static ITerrainOverride QuestArea;

    private static void Postfix(Map map)
    {
        //Log.Error($"Roof Edge: {Settings.RoofEdgeDepth}");
        MiddleAreaCenter = null;
        if (map.TileInfo.hilliness != Hilliness.Impassable)
        {
            return;
        }

        var radius = (int)(((float)map.Size.x + map.Size.z) * 0.25f) + Settings.OuterRadius;

        var middleWallSmoothness = Settings.MiddleWallSmoothness;
        var r = Settings.TrueRandom
            ? new Random(Guid.NewGuid().GetHashCode())
            : new Random((Find.World.info.name + map.Tile).GetHashCode());

        ITerrainOverride middleArea = null;
        if (Settings.HasMiddleArea)
        {
            middleArea = generateMiddleArea(r, map);
        }

        QuestArea = null;
        if (MapGenerator_GenerateMap.IsQuestMap)
        {
            Log.Message("[Impassable Map Maker] map is for a quest. An open area will be created to support it.");
            QuestArea = generateAreaForQuests(r, map);
        }

        ITerrainOverride quaryArea = null;
        if (Settings.IncludeQuarrySpot)
        {
            quaryArea = determineQuarry(r, map, middleArea);
        }

        var elevation = MapGenerator.Elevation;
        var roofXMinMax = new IntVec2(Settings.RoofEdgeDepth, map.Size.x - Settings.RoofEdgeDepth);
        var roofZMinMax = new IntVec2(Settings.RoofEdgeDepth, map.Size.z - Settings.RoofEdgeDepth);
        foreach (var current in map.AllCells)
        {
            float elev;
            if (isMountain(current, map, radius))
            {
                elev = 3.40282347E+38f;
                map.roofGrid.SetRoof(current, RoofDefOf.RoofRockThick);
            }
            else if (Settings.ScatteredRocks && isScatteredRock(current, r, map, radius))
            {
                elev = 0.75f;
            }
            else
            {
                elev = 0.57f;
                map.roofGrid.SetRoof(current, null);
            }

            if (QuestArea?.IsInside(current) == true)
            {
                elev = 0;
                map.roofGrid.SetRoof(current, null);
            }
            else if (quaryArea?.IsInside(current) == true)
            {
                // Gravel
                elev = 0.57f;
                map.roofGrid.SetRoof(current, null);
            }
            else if (middleArea != null)
            {
                var i = middleWallSmoothness == 0 ? 0 : r.Next(middleWallSmoothness);
                if (middleArea.IsInside(current, i))
                {
                    addMiddleAreaCell(current);
                    elev = 0;
                    map.roofGrid.SetRoof(current, null);
                }
            }

            elevation[current] = elev;

            if (Settings.OuterShape != ImpassableShape.Fill || roofXMinMax.x <= 0)
            {
                continue;
            }

            if (current.x > roofXMinMax.x &&
                current.x < roofXMinMax.z &&
                current.z > roofZMinMax.x &&
                current.z < roofZMinMax.z)
            {
                continue;
            }

            elevation[current] = 0.75f;
            map.roofGrid.SetRoof(current, null);
        }
    }

    private static void addMiddleAreaCell(IntVec3 c)
    {
        middleAreaCells ??= [];

        middleAreaCells.Add($"{c.x},{c.z}");
    }

    public static void ClearMiddleAreaCells()
    {
        middleAreaCells?.Clear();
    }

    public static bool IsInMiddleArea(IntVec3 c)
    {
        return middleAreaCells?.Contains($"{c.x},{c.z}") == true;
    }

    private static ITerrainOverride generateMiddleArea(Random r, Map map)
    {
        var basePatchX = randomBasePatch(r, map.Size.x);
        var basePatchZ = randomBasePatch(r, map.Size.z);
        MiddleAreaCenter = new IntVec2(basePatchX, basePatchZ);

        if (Settings.OpenAreaShape != ImpassableShape.Square)
        {
            return new TerrainOverrideRound(new IntVec3(basePatchX, 0, basePatchZ), Settings.OpenAreaSizeX);
        }

        var halfXSize = Settings.OpenAreaSizeX / 2;
        var halfZSize = Settings.OpenAreaSizeZ / 2;
        return new TerrainOverrideSquare(basePatchX - halfXSize, basePatchZ - halfZSize, basePatchX + halfXSize,
            basePatchZ + halfZSize);
    }

    private static ITerrainOverride generateAreaForQuests(Random r, Map map)
    {
        var x = determineRandomPlacement(30, map.Size.x - 30, 0, r);
        var z = determineRandomPlacement(30, map.Size.z - 30, 0, r);

        return new TerrainOverrideRound(new IntVec3(x, 0, z), 20);
    }

    private static bool isMountain(IntVec3 i, Map map, int radius)
    {
        // Fill
        if (Settings.OuterShape == ImpassableShape.Fill)
        {
            return Settings.RoofEdgeDepth <= 0 || !IsWithinCornerOfMap(i, map.Size.x, map.Size.z);
        }

        if (IsWithinCornerOfMap(i, map.Size.x, map.Size.z))
        {
            return false;
        }

        var buffer = Settings.PerimeterBuffer;
        if (Settings.OuterShape == ImpassableShape.Round)
        {
            // Round
            if (buffer != 0)
            {
                if (i.x < buffer || i.x > map.Size.x - buffer - 1 ||
                    i.z < buffer || i.z > map.Size.z - buffer - 1)
                {
                    return false;
                }
            }

            var x = i.x - (int)(map.Size.x * 0.5f);
            var z = i.z - (int)(map.Size.z * 0.5f);
            return Math.Sqrt(Math.Pow(x, 2) + Math.Pow(z, 2)) < radius;
        }

        if (buffer == 0)
        {
            return true;
        }

        return
            i.x > buffer &&
            i.x < map.Size.x - (buffer + 1) &&
            i.z > buffer &&
            i.z < map.Size.z - (buffer + 1);
    }

    public static bool IsWithinCornerOfMap(IntVec3 i, int xMax, int zMax)
    {
        const int min = 8;
        return i is { x: < min, z: < min } ||
               i.x < min && i.z > zMax - min ||
               i.x > xMax - min && i.z < min ||
               i.x > xMax - min && i.z > zMax - min;
    }

    private static bool isScatteredRock(IntVec3 i, Random r, Map map, int radius)
    {
        if (r.Next(42) >= 5)
        {
            return false;
        }

        if (Settings.OuterShape != ImpassableShape.Round)
        {
            return true;
        }

        // Round
        var x = i.x - (int)(map.Size.x * 0.5f);
        var z = i.z - (int)(map.Size.z * 0.5f);
        return Math.Sqrt(Math.Pow(x, 2) + Math.Pow(z, 2)) < radius + 8;
    }

    private static int randomBasePatch(Random r, int size)
    {
        var half = size / 2;
        var delta = r.Next((int)(0.01 * Settings.PercentOffset * half));
        var sign = r.Next(2);
        if (sign == 0)
        {
            delta *= -1;
        }

        return half + delta;
    }

    private static ITerrainOverride determineQuarry(Random r, Map map, ITerrainOverride middleArea)
    {
        var quarterMapX = map.Size.x / 4;
        var quarterMapZ = map.Size.z / 4;
        int lowX, highX, lowZ, highZ;
        var quarrySize = Settings.QuarrySize;

        middleArea ??= new TerrainOverrideSquare(quarterMapX * 2, quarterMapZ * 2, quarterMapX * 2, quarterMapZ * 2);

        if (r.Next(2) == 0)
        {
            highX = middleArea.LowX - 2;
            lowX = highX - quarterMapX;
            var x = determineRandomPlacement(lowX, highX, quarrySize, r);
            lowX = x - quarrySize;
            highX = x;
        }
        else
        {
            lowX = middleArea.HighX + 2;
            highX = lowX + quarterMapX;
            var x = determineRandomPlacement(lowX, highX, quarrySize, r);
            lowX = x;
            highX = x + quarrySize;
        }

        if (r.Next(2) == 0)
        {
            highZ = middleArea.LowZ - 2;
            lowZ = highZ - quarterMapZ;
            var z = determineRandomPlacement(lowZ, highZ, quarrySize, r);
            lowZ = z - quarrySize;
            highZ = z;
        }
        else
        {
            lowZ = middleArea.HighZ + 2;
            highZ = lowZ + quarterMapZ;
            var z = determineRandomPlacement(lowZ, highZ, quarrySize, r);
            lowZ = z;
            highZ = z + quarrySize;
        }

        return new TerrainOverrideSquare(lowX, lowZ, highX, highZ);
    }

    private static int determineRandomPlacement(int low, int high, int size, Random r)
    {
        low += size;
        high -= size;
        return r.Next(high - low) + low;
    }
}