using HarmonyLib;
using RimWorld.Planet;
using Verse;

namespace ImpassableMapMaker;

[HarmonyPatch(typeof(SettleInEmptyTileUtility), nameof(SettleInEmptyTileUtility.Settle))]
internal static class SettleInEmptyTileUtility_Settle
{
    private static ImpassableShape OutterShape = ImpassableShape.NotSet;

    [HarmonyPriority(Priority.First)]
    public static void Prefix()
    {
        OutterShape = Settings.OuterShape;
        if (OutterShape == ImpassableShape.Fill)
        {
            Settings.OuterShape = Rand.Bool ? ImpassableShape.Round : ImpassableShape.Square;
        }
    }

    public static void Finally()
    {
        if (OutterShape != ImpassableShape.NotSet)
        {
            Settings.OuterShape = OutterShape;
        }

        OutterShape = ImpassableShape.NotSet;
    }
}