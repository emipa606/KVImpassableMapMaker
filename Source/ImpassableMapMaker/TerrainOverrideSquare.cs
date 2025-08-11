using Verse;

namespace ImpassableMapMaker;

internal class TerrainOverrideSquare(int lowX, int lowZ, int highX, int highZ)
    : ITerrainOverride
{
    private readonly IntVec3 High = new(highX, 0, highZ);
    private readonly IntVec3 Low = new(lowX, 0, lowZ);

    public bool IsInside(IntVec3 i, int rand = 0)
    {
        return i.x >= Low.x + rand && i.x <= High.x + rand &&
               i.z >= Low.z + rand && i.z <= High.z + rand;
    }

    public int LowX => Low.x;
    public int HighX => High.x;
    public int LowZ => Low.z;
    public int HighZ => High.z;
    public IntVec3 Center => new((int)(HighZ - (LowX * 0.5f)) + LowX, 0, (int)(HighZ - (LowZ * 0.5f)) + LowZ);
}