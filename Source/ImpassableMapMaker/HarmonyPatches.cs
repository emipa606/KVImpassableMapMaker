using System.Reflection;
using HarmonyLib;
using Verse;

namespace ImpassableMapMaker;

[StaticConstructorOnStartup]
public static class HarmonyPatches
{
    static HarmonyPatches()
    {
        new Harmony("com.impassablemapmaker.rimworld.mod").PatchAll(Assembly.GetExecutingAssembly());
    }
}