using HarmonyLib;
using RimWorld.Planet;

namespace ImpassableMapMaker;

[HarmonyPatch(typeof(WorldPathGrid), "HillinessMovementDifficultyOffset")]
internal static class WorldPathGrid_HillinessMovementDifficultyOffset
{
    public static void Postfix(ref float __result, Hilliness hilliness)
    {
        if (hilliness == Hilliness.Impassable)
        {
            __result = Settings.MovementDifficulty;
        }
    }
}