using System.Text;
using HarmonyLib;
using RimWorld;
using RimWorld.Planet;
using Verse;

namespace ImpassableMapMaker;

[HarmonyPatch(typeof(TileFinder), nameof(TileFinder.IsValidTileForNewSettlement))]
internal static class TileFinder_IsValidTileForNewSettlement
{
    [HarmonyPriority(Priority.First)]
    private static void Postfix(ref bool __result, PlanetTile tile, StringBuilder reason)
    {
        if (__result)
        {
            return;
        }

        if (Find.WorldGrid[tile].hilliness != Hilliness.Impassable)
        {
            return;
        }

        reason?.Remove(0, reason.Length);

        __result = true;

        var settlement = Find.WorldObjects.SettlementAt(tile);
        if (settlement != null)
        {
            if (reason != null)
            {
                if (settlement.Faction == null)
                {
                    reason.Append("TileOccupied".Translate());
                }
                else if (settlement.Faction == Faction.OfPlayer)
                {
                    reason.Append("YourBaseAlreadyThere".Translate());
                }
                else
                {
                    var translated = "BaseAlreadyThere".Translate(settlement.Faction.Name);
                    reason.Append(translated);
                }
            }

            __result = false;
        }

        if (Find.WorldObjects.AnySettlementBaseAtOrAdjacent(tile))
        {
            reason?.Append("FactionBaseAdjacent".Translate());

            __result = false;
        }

        if (!Find.WorldObjects.AnyMapParentAt(tile) && Current.Game.FindMap(tile) == null)
        {
            return;
        }

        reason?.Append("TileOccupied".Translate());

        __result = false;
    }
}