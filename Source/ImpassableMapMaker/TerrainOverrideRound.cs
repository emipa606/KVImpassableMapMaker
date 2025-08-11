using System;
using Verse;

namespace ImpassableMapMaker;

internal class TerrainOverrideRound(IntVec3 center, int radius) : ITerrainOverride
{
    public bool IsInside(IntVec3 i, int rand = 0)
    {
        if (i.x <= center.x - radius || i.x >= center.x + radius ||
            i.z <= center.z - radius || i.z >= center.z + radius)
        {
            return false;
        }

        var x = i.x - center.x;
        var z = i.z - center.z;
        return Math.Pow(x, 2) + Math.Pow(z, 2) < Math.Pow(radius, 2);
    }

    public int LowX => center.x - radius;
    public int HighX => center.x + radius;
    public int LowZ => center.z - radius;
    public int HighZ => center.z + radius;
    IntVec3 ITerrainOverride.Center => center;
}