using System.Text;
using HarmonyLib;
using RimWorld.Planet;
using Verse;

namespace ImpassableMapMaker;

[HarmonyPatch(typeof(WorldPathGrid), nameof(WorldPathGrid.CalculatedMovementDifficultyAt))]
internal static class WorldPathGrid_CalculatedMovementDifficultyAt
{
    private static bool Prefix(ref float __result, PlanetTile tile, int? ticksAbs = null,
        StringBuilder explanation = null)
    {
        var tile2 = Find.WorldGrid[tile];
        if (!tile2.PrimaryBiome.impassable && tile2.hilliness != Hilliness.Impassable)
        {
            return true;
        }

        if (explanation is { Length: > 0 })
        {
            explanation.AppendLine();
        }

        var num = tile2.PrimaryBiome.movementDifficulty;
        explanation?.Append(tile2.PrimaryBiome.LabelCap + ": " +
                            tile2.PrimaryBiome.movementDifficulty.ToStringWithSign("0.#"));

        var num2 = Settings.MovementDifficulty;
        num += num2;
        if (explanation != null)
        {
            explanation.AppendLine();
            explanation.Append(tile2.hilliness.GetLabelCap() + ": " + num2.ToStringWithSign("0.#"));
        }

        num += WorldPathGrid.GetCurrentWinterMovementDifficultyOffset(tile, ticksAbs ?? GenTicks.TicksAbs, explanation);
        __result = num;
        return false;
    }
}