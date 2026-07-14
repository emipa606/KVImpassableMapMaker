# GitHub Copilot Instructions for [KV] Impassable Map Maker (Continued)

## Mod Overview and Purpose

The [KV] Impassable Map Maker (Continued) mod, originally created by Kiame Vivacity, enhances the gameplay of RimWorld by allowing players to create settlements on Impassable tiles. This update, continued by the community, provides an intriguing challenge by introducing unique elements and settings for players who settle in otherwise inaccessible areas.

## Key Features and Systems

- **Settlements on Impassable Tiles**: Players can settle on or travel to Impassable map tiles using caravans or drop ships.
  
- **Unique Map Elements**: Each Impassable map includes hidden open areas in the mountains and potential ruins or ancient dangers.

- **World Pathing**: While AI pathing avoids Impassable tiles, players can force caravans to travel there, taking more time.

- **Settings Customization**:
  - **World Map Movement Difficulty**: Adjusts the time it takes to traverse an Impassable tile.
  - **Mountain Shape**: Choose between Square or Round mountain shapes, with configurable radius for Round mountains.
  - **Middle Area Settings**: Enable or disable an open area in the mountain with customizable shape, size, and wall smoothness.
  - **Edge Buffer**: Allows room around map edges if the mountain walls extend past the map.
  - **Quarry Integration**: Optionally include a quarry.
  - **True Randomization**: Ensure unique map generation each time, even with the same world name and location.

- **Incompatibility Note**: Known issues with the Map ReRoll mod.

## Coding Patterns and Conventions

- **Structure**: The mod is organized into separate patches and utility classes, reflecting a clear separation of concerns.
- **Patching**: Uses Harmony to apply changes non-invasively to the base game logic.
- **Interfacing**: Implements interfaces such as `ITerrainOverride` to handle terrain modifications reliably.

## XML Integration

- **About.xml**: Contains metadata about the mod, listing dependencies such as `brrainz.harmony`.
- **ModSync.xml**: Used for mod synchronization details.
- **Version.xml**: Keeps versioning information for the mod.

## Harmony Patching

- **GenStep_ScattererBestFit.cs**: This patch uses a Prefix method to customize scatter generation on Impassable maps.
- **GenStep_FindPlayerStartSpot_Generate.cs**: Utilizes a Postfix method to modify the player starting spot generation for Impassable tiles.
- **SettleInEmptyTileUtility_Settle.cs**: Includes Finally and Prefix methods to ensure that new settlements on Impassable tiles are handled correctly.
- **TileFinder_IsValidTileForNewSettlement.cs**: Uses a Postfix method to adjust tile validation logic to accommodate Impassable tiles.

## Suggestions for Copilot

- **Code Completion**: Assist in writing new patches for additional map features or customization settings.
- **Refactoring**: Help refactor existing code for improved readability and performance without changing functionality.
- **XML Enhancements**: Suggest additions or improvements to XML configurations for better integration with other mods or base game updates.
- **Troubleshooting**: Provide suggestions for debugging incompatibilities, especially with other mods.

This guide should assist in maintaining and expanding the functionality of the [KV] Impassable Map Maker (Continued) mod using best practices in C# and XML within the RimWorld modding framework.

## Project Solution Guidelines
- Relevant mod XML files are included as Solution Items under the solution folder named XML, these can be read and modified from within the solution.
- Use these in-solution XML files as the primary files for reference and modification.
- The `.github/copilot-instructions.md` file is included in the solution under the `.github` solution folder, so it should be read/modified from within the solution instead of using paths outside the solution. Update this file once only, as it and the parent-path solution reference point to the same file in this workspace.
- When making functional changes in this mod, ensure the documented features stay in sync with implementation; use the in-solution `.github` copy as the primary file.
- In the solution is also a project called Assembly-CSharp, containing a read-only version of the decompiled game source, for reference and debugging purposes.
- For any new documentation, update this copilot-instructions.md file rather than creating separate documentation files.


## Hard rules (must follow)
- Do NOT run commands that modify the repo (no git commit, git apply, dotnet format) unless explicitly asked.
- Prefer minimal reads: read only the smallest code region needed (around the suspicious lines).

