# Copilot Instructions for [KV] Impassable Map Maker (Continued) Mod

## Mod Overview and Purpose
The [KV] Impassable Map Maker (Continued) mod enhances gameplay in RimWorld by allowing players to settle on and explore impassable map tiles. Originally developed by Kiame Vivacity, the mod has been updated to enable players to either start new games on impassable tiles or travel to them from existing games using caravans or drop ships. Impassable maps feature hidden open areas within the mountains, ruins, and ancient dangers, offering a novel and challenging experience. 

## Key Features and Systems
- **Impassable Tile Settlement**: Enables settlement on tiles traditionally impassable, providing fresh gameplay dynamics.
- **Hidden Mountain Areas**: Each impassable map contains a concealed open area, which adds a layer of exploration and mystery.
- **World Map Mechanics**: Allows caravans to travel to impassable tiles. However, world pathing generally avoids these tiles unless directed otherwise.
- **Customizable Settings**: Players can configure various aspects of the map, such as mountain shape (Square/Round), presence of rocks, and middle open area specifications.
- **Inclusivity with Quarry Mod**: Option to include a quarry in impassable maps.
- **True Random Generation**: Ensures unique map generation each time, unlike the vanilla repetitive map layout.

## Coding Patterns and Conventions
- **C# Programming**: The mod utilizes C# extensively for custom game logic. It follows common C# conventions such as PascalCase for method names and camelCase for local variables.
- **Code Organization**: Classes are grouped into relevant files that handle specific functions. For instance, `HarmonyPatches.cs` is dedicated to all Harmony-related patches.
- **Consistent Naming**: Classes and methods are named in relation to their functionality, aiding in readability and maintenance.

## XML Integration
- **About.xml**: Defines basic mod information and dependencies, such as the requirement of brrainz.harmony.
- **ModSync.xml and Version.xml**: Manage mod synchronization and version tracking.
- XML files use a hierarchical structure to organize data logically and include necessary schema or dependencies.

## Harmony Patching
- **Harmony**: The mod leverages Harmony for patching existing game functions. It uses Prefix, Postfix, and Finally where necessary to inject or modify game logic.
- **Patch Locations**: Examples include `MapGenerator_GenerateMap.cs` for map generation customization and `GenStep_FindPlayerStartSpot_Generate.cs` for determining player start spots.
- **Patch Structure**: Follows a systematic approach where each function intended for modification is clearly marked and structured for ease of future adjustments.

## Suggestions for Copilot
1. **Harmony Patches**: Generate Prefix, Postfix, and Finally patches using patterns from existing files, focusing on map initialization and pathing logic.
2. **XML Template**: Use existing XML files as templates for creating new ones with structured data schemas, considering dependencies.
3. **Modular Methods**: Propose methods that encapsulate distinct functionalities - like map settings retrieval or path adjustment algorithms.
4. **Error Handling**: Suggest error-handling mechanisms in C# to ensure stability when the mod interacts with game APIs.
5. **Custom Settings UI**: Recommend UI elements and logic to manage user settings in the mod, keeping in line with RimWorld's UI framework.
   
This comprehensive instruction aims to guide your development using GitHub Copilot, encapsulating the core components and conventions of the [KV] Impassable Map Maker mod. By adhering to these guidelines, Copilot assistance can enhance the mod's functionality, ensure code maintainability, and improve user experience.

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
- When mentioning SonarQube issues, automatically use the SonarQube MCP service to fetch and address issues instead of making inferred fixes without querying SonarQube first.
- When mentioning the rimworld log, automatically use the Rimworld MCP service to fetch the log.

