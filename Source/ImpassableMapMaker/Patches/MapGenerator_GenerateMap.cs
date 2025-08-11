using System;
using System.Collections.Generic;
using HarmonyLib;
using RimWorld;
using RimWorld.Planet;
using Verse;

namespace ImpassableMapMaker;

[HarmonyPatch(typeof(MapGenerator), nameof(MapGenerator.GenerateMap))]
public class MapGenerator_GenerateMap
{
    public static bool IsQuestMap;

    [HarmonyPriority(Priority.First)]
    private static void Prefix(MapParent parent, MapGeneratorDef mapGenerator,
        IEnumerable<GenStepWithParams> extraGenStepDefs)
    {
        SettleInEmptyTileUtility_Settle.Prefix();

        GenStep_ElevationFertility_Generate.ClearMiddleAreaCells();
        foreach (var q in Current.Game.questManager.QuestsListForReading)
        {
            if (q.State != QuestState.Ongoing)
            {
                continue;
            }

            foreach (var qt in q.QuestLookTargets)
            {
                if (qt.Tile != parent.Tile)
                {
                    continue;
                }

                IsQuestMap = true;
                return;
            }
        }

        foreach (var d in mapGenerator.genSteps)
        {
            if (!d.genStep.def.defName.Contains("Archonexus"))
            {
                continue;
            }

            IsQuestMap = true;
            return;
        }

        if (extraGenStepDefs != null)
        {
            foreach (var d in extraGenStepDefs)
            {
                if (!d.def.defName.Contains("Archonexus"))
                {
                    continue;
                }

                IsQuestMap = true;
                return;
            }
        }

        IsQuestMap = false;
    }

    [HarmonyPriority(Priority.First)]
    private static void Postfix(ref Map __result)
    {
        if (__result.TileInfo.hilliness != Hilliness.Impassable || Settings.OuterShape != ImpassableShape.Fill)
        {
            return;
        }

        var maxX = __result.Size.x - 1;
        var maxZ = __result.Size.z - 1;
        foreach (var current in __result.AllCells)
        {
            if (Settings.CoverRoadAndRiver && __result.roofGrid.RoofAt(current) == null)
            {
                if (GenStep_ElevationFertility_Generate.IsWithinCornerOfMap(current, maxX + 1, maxZ + 1) ||
                    GenStep_ElevationFertility_Generate.IsInMiddleArea(current))
                {
                    __result.roofGrid.SetRoof(current, null);
                    continue;
                }

                __result.roofGrid.SetRoof(current, RoofDefOf.RoofRockThick);
            }

            if (Settings.RoofEdgeDepth > 0)
            {
                if (current.x == 0 ||
                    current.x == maxX ||
                    current.z == 0 ||
                    current.z == maxZ)
                {
                    __result.roofGrid.SetRoof(current, null);
                }
            }

            if (!Settings.HasMiddleArea)
            {
                for (var x = Math.Max(0, MapGenerator.PlayerStartSpot.x - 5);
                     x < Math.Min(__result.Size.x, MapGenerator.PlayerStartSpot.x + 5);
                     ++x)
                for (var z = Math.Max(0, MapGenerator.PlayerStartSpot.z - 5);
                     z < Math.Min(__result.Size.z, MapGenerator.PlayerStartSpot.z + 5);
                     ++z)
                {
                    var i = new IntVec3(x, 0, z);
                    foreach (var t in __result.thingGrid.ThingsAt(i))
                    {
                        if (t.def.passability == Traversability.Impassable)
                        {
                            t.Destroy();
                        }
                    }

                    __result.roofGrid.SetRoof(i, null);
                }
            }
            else
            {
                __result.roofGrid.SetRoof(MapGenerator.PlayerStartSpot, null);
            }
        }
    }

    [HarmonyFinalizer]
    private static void Finally()
    {
        SettleInEmptyTileUtility_Settle.Finally();
    }
}