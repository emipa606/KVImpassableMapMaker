using Verse;

namespace ImpassableMapMaker;

internal interface ITerrainOverride
{
    int HighZ { get; }
    int LowZ { get; }
    int HighX { get; }
    int LowX { get; }
    IntVec3 Center { get; }

    bool IsInside(IntVec3 i, int rand = 0);
}