using Mlie;
using UnityEngine;
using Verse;

namespace ImpassableMapMaker;

public class SettingsController : Mod
{
    public static string CurrentVersion;

    public SettingsController(ModContentPack content) : base(content)
    {
        GetSettings<Settings>();
        CurrentVersion = VersionFromManifest.GetVersionFromModMetaData(content.ModMetaData);
    }

    public override string SettingsCategory()
    {
        return "Impassable Map Maker";
    }

    public override void DoSettingsWindowContents(Rect inRect)
    {
        Settings.DoSettingsWindowContents(inRect);
    }
}