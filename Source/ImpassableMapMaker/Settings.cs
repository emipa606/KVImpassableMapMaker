using System.Text;
using UnityEngine;
using Verse;

namespace ImpassableMapMaker;

public class Settings : ModSettings
{
    private const int GapSize = 24;
    private const float DefaultPercentOffset = 5f;
    private const int DefaultOpenAreaSize = 54;
    private const int DefaultWallsSmoothness = 10;
    private const int DefaultPeremeterBuffer = 6;
    private const int DefaultQuarySize = 5;
    private const float DefaultMovementDifficulty = 4.5f;
    private const int DefaultOuterRadius = 1;
    private const int DefaultRoofEdgeDepth = 5;

    private static Vector2 scrollPosition = Vector2.zero;

    private static float percentOffset = DefaultPercentOffset;
    public static ImpassableShape OpenAreaShape = ImpassableShape.Square;
    public static int OpenAreaSizeX = DefaultOpenAreaSize;
    public static int OpenAreaSizeZ = DefaultOpenAreaSize;
    public static int MiddleWallSmoothness = 10;
    public static int PerimeterBuffer = DefaultPeremeterBuffer;
    public static bool HasMiddleArea = true;
    public static bool StartInMiddleArea;
    public static ImpassableShape OuterShape = ImpassableShape.Square;
    public static bool ScatteredRocks = true;
    public static bool IncludeQuarrySpot;
    public static int OuterRadius = 1;
    public static int RoofEdgeDepth = 5;
    public static bool CoverRoadAndRiver;
    public static int QuarrySize = DefaultQuarySize;
    public static bool TrueRandom;
    public static float MovementDifficulty = DefaultMovementDifficulty;
    private static string movementDifficultyBuffer = DefaultMovementDifficulty.ToString();
    public static float PercentOffset => percentOffset;

    public override void ExposeData()
    {
        base.ExposeData();

        var s = OuterShape.ToString();
        var openAreaShape = OpenAreaShape.ToString();

        Scribe_Values.Look(ref HasMiddleArea, "ImpassableMapMaker.hasMiddleArea", true);
        Scribe_Values.Look(ref StartInMiddleArea, "ImpassableMapMaker.startInMiddleArea");
        Scribe_Values.Look(ref percentOffset, "ImpassableMapMaker.percentOffset", DefaultPercentOffset);
        Scribe_Values.Look(ref openAreaShape, "ImpassableMapMaker.OpenAreaShape", nameof(ImpassableShape.Square));
        Scribe_Values.Look(ref OpenAreaSizeX, "ImpassableMapMaker.OpenAreaSizeX", DefaultOpenAreaSize);
        Scribe_Values.Look(ref OpenAreaSizeZ, "ImpassableMapMaker.OpenAreaSizeZ", DefaultOpenAreaSize);
        Scribe_Values.Look(ref PerimeterBuffer, "ImpassableMapMaker.PeremeterBuffer", DefaultPeremeterBuffer);
        Scribe_Values.Look(ref MiddleWallSmoothness, "ImpassableMapMaker.MakeWallsSmooth", DefaultWallsSmoothness);
        Scribe_Values.Look(ref s, "ImpassableMapMaker.Shape", nameof(ImpassableShape.Square));
        Scribe_Values.Look(ref ScatteredRocks, "ImpassableMapMaker.scatteredRocks", true);
        Scribe_Values.Look(ref IncludeQuarrySpot, "ImpassableMapMaker.IncludeQuarySpot");
        Scribe_Values.Look(ref QuarrySize, "ImpassableMapMaker.QuarySize", DefaultQuarySize);
        Scribe_Values.Look(ref MovementDifficulty, "ImpassableMapMaker.MovementDifficulty",
            DefaultMovementDifficulty);
        Scribe_Values.Look(ref OuterRadius, "ImpassableMapMaker.OuterRadius", DefaultOuterRadius);
        Scribe_Values.Look(ref TrueRandom, "ImpassableMapMaker.TrueRandom");
        Scribe_Values.Look(ref RoofEdgeDepth, "ImpassableMapMaker.RoofEdgeDepth", DefaultRoofEdgeDepth);
        Scribe_Values.Look(ref CoverRoadAndRiver, "ImpassableMapMaker.CoverRoadAndRiver");
        movementDifficultyBuffer = MovementDifficulty.ToString();

        if (Scribe.mode == LoadSaveMode.Saving)
        {
            return;
        }

        switch (s)
        {
            case nameof(ImpassableShape.Round):
                OuterShape = ImpassableShape.Round;
                break;
            case nameof(ImpassableShape.Square):
                OuterShape = ImpassableShape.Square;
                break;
            default:
                OuterShape = ImpassableShape.Fill;
                break;
        }

        OpenAreaShape = nameof(ImpassableShape.Round).Equals(openAreaShape)
            ? ImpassableShape.Round
            : ImpassableShape.Square;
    }

    public static void DoSettingsWindowContents(Rect rect)
    {
        var scroll = new Rect(5f, 45f, 430, rect.height - 40);
        var view = new Rect(0, 45, 400, 1200);

        Widgets.BeginScrollView(scroll, ref scrollPosition, view);
        var ls = new Listing_Standard();
        ls.Begin(view);

        // World Map Movement Difficulty
        ls.TextFieldNumericLabeled(
            "ImpassableMapMaker.WorldMapMovementDifficulty".Translate(),
            ref MovementDifficulty, ref movementDifficultyBuffer, 1f, 100f);
        if (ls.ButtonText("ImpassableMapMaker.Default".Translate()))
        {
            MovementDifficulty = DefaultMovementDifficulty;
            movementDifficultyBuffer = DefaultMovementDifficulty.ToString();
        }

        ls.GapLine(GapSize);

        // Mountain Shape
        ls.Label("ImpassableMapMaker.MountainShape".Translate());
        if (ls.RadioButton("ImpassableMapMaker.ShapeSquare".Translate(), OuterShape == ImpassableShape.Square))
        {
            OuterShape = ImpassableShape.Square;
        }

        if (ls.RadioButton("ImpassableMapMaker.ShapeRound".Translate(), OuterShape == ImpassableShape.Round))
        {
            OuterShape = ImpassableShape.Round;
        }

        if (ls.RadioButton("ImpassableMapMaker.ShapeFill".Translate(), OuterShape == ImpassableShape.Fill))
        {
            OuterShape = ImpassableShape.Fill;
        }

        ls.Gap(GapSize);

        switch (OuterShape)
        {
            case ImpassableShape.Round:
                ls.Label("ImpassableMapMaker.OuterRadius".Translate() + ": " + OuterRadius);
                OuterRadius = (int)ls.Slider(OuterRadius, -100, 100);
                if (ls.ButtonText("ImpassableMapMaker.Default".Translate()))
                {
                    OuterRadius = DefaultOuterRadius;
                }

                ls.Gap(GapSize);
                break;
            case ImpassableShape.Fill:
                ls.CheckboxLabeled("ImpassableMapMaker.CoverRoadAndRiver".Translate(), ref CoverRoadAndRiver,
                    "ImpassableMapMaker.CoverRoadAndRiverDesc".Translate());
                ls.Label((TaggedString)("ImpassableMapMaker.RooflessEdgeBuffer".Translate() + ": " + RoofEdgeDepth), -1,
                    "ImpassableMapMaker.RooflessEdgeBufferDesc".Translate().ToString());
                RoofEdgeDepth = (int)ls.Slider(RoofEdgeDepth, 0, 20);
                if (ls.ButtonText("ImpassableMapMaker.Default".Translate()))
                {
                    RoofEdgeDepth = DefaultRoofEdgeDepth;
                }

                ls.Gap(GapSize);
                break;
        }

        ls.CheckboxLabeled("ImpassableMapMaker.ScatteredRocks".Translate(), ref ScatteredRocks);
        ls.GapLine(GapSize);

        // Middle Area
        ls.CheckboxLabeled("ImpassableMapMaker.HasMiddleArea".Translate(), ref HasMiddleArea);
        if (HasMiddleArea)
        {
            ls.CheckboxLabeled("ImpassableMapMaker.StartInMiddleArea".Translate(), ref StartInMiddleArea);
            ls.Label("ImpassableMapMaker.MiddleAreaShape".Translate());
            ls.Gap(2);
            if (ls.RadioButton("ImpassableMapMaker.ShapeSquare".Translate(), OpenAreaShape == ImpassableShape.Square))
            {
                OpenAreaShape = ImpassableShape.Square;
            }

            if (ls.RadioButton("ImpassableMapMaker.ShapeRound".Translate(), OpenAreaShape == ImpassableShape.Round))
            {
                OpenAreaShape = ImpassableShape.Round;
            }

            ls.Gap(2);

            ls.Label("ImpassableMapMaker.OpenAreaSize".Translate());
            if (OpenAreaShape == ImpassableShape.Square)
            {
                ls.Label("ImpassableMapMaker.Width".Translate() + ": " + OpenAreaSizeZ);
                OpenAreaSizeZ = (int)ls.Slider(OpenAreaSizeZ, 5, 150);
                ls.Label("ImpassableMapMaker.Height".Translate() + ": " + OpenAreaSizeX);
                OpenAreaSizeX = (int)ls.Slider(OpenAreaSizeX, 5, 150);
            }
            else
            {
                ls.Label("ImpassableMapMaker.InnerRadius".Translate() + ": " + OpenAreaSizeZ);
                OpenAreaSizeX = (int)ls.Slider(OpenAreaSizeX, 5, 100);
                OpenAreaSizeZ = OpenAreaSizeX;
            }

            if (ls.ButtonText("ImpassableMapMaker.Default".Translate()))
            {
                if (OpenAreaShape == ImpassableShape.Square)
                {
                    OpenAreaSizeX = DefaultOpenAreaSize;
                    OpenAreaSizeZ = DefaultOpenAreaSize;
                }
                else
                {
                    OpenAreaSizeX = (int)(DefaultOpenAreaSize * 0.5f);
                    OpenAreaSizeZ = (int)(DefaultOpenAreaSize * 0.5f);
                }
            }

            ls.Gap(GapSize);

            ls.Label("ImpassableMapMaker.OpenAreaMaxOffsetFromMiddle".Translate() + ": " +
                     percentOffset.ToString("N1") + "%");
            percentOffset = ls.Slider(percentOffset, 0, 25);
            if (ls.ButtonText("ImpassableMapMaker.Default".Translate()))
            {
                percentOffset = DefaultPercentOffset;
            }

            ls.Gap(GapSize);

            ls.Label("ImpassableMapMaker.MiddleOpeningWallSmoothnes".Translate());
            ls.Label("<< " + "ImpassableMapMaker.Smooth".Translate() + " -- " + "ImpassableMapMaker.Rough".Translate() +
                     " >>");
            MiddleWallSmoothness = (int)ls.Slider(MiddleWallSmoothness, 0, 20);
            if (ls.ButtonText("ImpassableMapMaker.Default".Translate()))
            {
                MiddleWallSmoothness = 10;
            }
        }

        ls.GapLine(GapSize);

        if (OuterShape != ImpassableShape.Fill)
        {
            ls.Label("ImpassableMapMaker.EdgeBuffer".Translate() + ": " + PerimeterBuffer.ToString());
            ls.Label("<< " + "ImpassableMapMaker.Smaller".Translate() + " -- " +
                     "ImpassableMapMaker.Larger".Translate() + " >>");
            PerimeterBuffer = (int)ls.Slider(PerimeterBuffer, 1, 100);
            if (ls.ButtonText("ImpassableMapMaker.Default".Translate()))
            {
                PerimeterBuffer = DefaultPeremeterBuffer;
            }

            ls.Label("ImpassableMapMaker.EdgeBufferWarning".Translate());
            ls.GapLine(GapSize);
        }

        ls.CheckboxLabeled("ImpassableMapMaker.IncludeQuarySpot".Translate(), ref IncludeQuarrySpot);
        if (IncludeQuarrySpot)
        {
            ls.Label("<< " + "ImpassableMapMaker.Smaller".Translate() + " -- " +
                     "ImpassableMapMaker.Larger".Translate() + " >>");
            QuarrySize = (int)ls.Slider(QuarrySize, 3, 20);
            var sb = new StringBuilder("(");
            sb.Append(QuarrySize.ToString());
            sb.Append(", ");
            sb.Append(QuarrySize.ToString());
            sb.Append(")");
            ls.Label(sb.ToString());
            if (ls.ButtonText("ImpassableMapMaker.Default".Translate()))
            {
                QuarrySize = DefaultQuarySize;
            }
        }

        ls.GapLine(GapSize);
        ls.CheckboxLabeled("ImpassableMapMaker.TrueRandom".Translate(), ref TrueRandom,
            "ImpassableMapMaker.TrueRandomDesc".Translate());
        if (SettingsController.CurrentVersion != null)
        {
            ls.Gap();
            GUI.contentColor = Color.gray;
            ls.Label("ImpassableMapMaker.CurrentModVersion".Translate(SettingsController.CurrentVersion));
            GUI.contentColor = Color.white;
        }

        ls.End();
        Widgets.EndScrollView();
    }
}