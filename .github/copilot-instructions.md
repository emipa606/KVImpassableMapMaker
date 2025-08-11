# GitHub Copilot Instructions for [KV] Impassable Map Maker (Continued)

## Overview and Purpose
The **[KV] Impassable Map Maker (Continued)** mod allows players to settle on impassable tiles in RimWorld. You can start a new game on an impassable tile or travel to one within an existing game using a caravan or drop ship. The impassable maps feature unique gameplay elements, such as hidden open areas, ruins, and ancient dangers. World pathing naturally avoids these tiles, but you can override this by directing a caravan to travel there. The journey to an impassable tile on foot is intentionally long to maintain balance.

## Key Features and Systems
- **World Map Movement Difficulty**: Adjust the time it takes to traverse an impassable tile on the world map.
- **Mountain Shape**: Choose between square or round shapes for the impassable mountain.
  - **Outer Radius**: Set the radius for round mountains.
- **Scattered Rocks**: Toggle whether rocks appear around the mountain's perimeter.
- **Middle Area**: Decide if there's an open area within the mountain.
  - **Middle Area Shape**: Set the shape of the open area.
  - **Size and Offset**: Define dimensions and offset for the open area.
  - **Wall Smoothness**: Choose between smooth or rough walls.
- **Edge Buffer**: Ensure a navigable buffer around the map edges.
- **Include Quarry**: Compatibility with the Quarry mod.
- **True Random**: Each generated map is unique, irrespective of world-name and location.

## Coding Patterns and Conventions
- Use the C# class naming conventions, following PascalCase for class names and camelCase for method names.
- Implement interface methods (such as `ITerrainOverride`) clearly within their respective classes.
- Follow established patterns for static class patching using Harmony for function overriding.

## XML Integration
- Ensure XML files defining map generation parameters, mountain shapes, and other settings are structured and lodged in the appropriate directories.
- Use XML attributes strategically to customize settings for map generation.

## Harmony Patching
- Use Harmony to patch existing game methods to introduce new behaviors specific to impassable tiles.
  - **HarmonyPatches_MapGenerator.cs** defines patches related to map generation.
  - **HarmonyPatches_WorldPathGrid.cs** deals with world pathing adjustments.

### Classes and Methods Example
- **HarmonyPatches_MapGenerator.cs**
  - `public partial class HarmonyPatches`
  - `internal class TerrainOverrideRound : ITerrainOverride`
  - Multiple static classes are used for patching different game components.

- **HarmonyPatches_WorldPathGrid.cs**
  - **Patch Definitions**:
    - `static class Patch_WorldPathGrid_HillinessMovementDifficultyOffset`
    - `static class Patch_WorldPathGrid_CalculatedMovementDifficultyAt`

## Suggestions for Copilot
- When writing new features or patch methods:
  - Use concise method names that reflect their purpose, ensuring they align with the mod's theme and logic.
  - Generate comments for methods explaining what each provides.
  - Leverage Copilot to suggest optimizations and refactoring opportunities when maintaining or extending existing code.
- For XML definitions and configuration settings:
  - Prompt Copilot to assist in generating comprehensive XML structures based on specified attributes in the settings.

By adhering to these guidelines and utilizing GitHub Copilot effectively, you can maintain and evolve the [KV] Impassable Map Maker (Continued) mod with robust, clear, and high-performance code.
