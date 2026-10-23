# GitHub Copilot Instructions for RimWorld: Incident Count Multiplier (Continued)

## Mod Overview and Purpose
The "Incident Count Multiplier (Continued)" mod is designed to enhance the gameplay experience in RimWorld by allowing players to adjust the frequency of random events or incidents introduced by the game's Storytellers. This provides players with the flexibility to experience more dynamic gameplay, customizing the level of challenge and unpredictability based on their preferences. The mod was originally developed by mlph and has been continued and updated to maintain compatibility with the latest versions of RimWorld.

## Key Features and Systems
- **Adjustable Incident Frequency**: Players can set a multiplier to increase or decrease the frequency of random incidents, which can be adjusted according to elapsed in-game days.
- **User-friendly Integration**: Settings are easily adjustable within the mod configuration interface.
- **Safe to Add and Remove**: The mod can be added or removed without causing persistent issues in saved games.
- **Robust Handling**: While setting extreme values may lead to occasional errors, the mod is designed to handle such circumstances gracefully.

## Coding Patterns and Conventions
- **C# Coding**: The codebase consists of 4 C# files, following standard coding practices for mod development in RimWorld.
  - `IncidentCountMultiplier_Harmony.cs`: Manages harmony patches.
  - `IncidentCountMultiplier.cs`: Contains main logic for the mod, including settings management.
  - `IncidentCountMultiplierSettings.cs`: Handles persistent settings data.
  - `Patches.cs`: Contains specific patches that modify game behavior.
- **Code Comments**: Use of descriptive comments to explain the purpose of classes and key functions.
- **Method Naming**: Follow PascalCase for method names and camelCase for local variables.

## XML Integration
There are no XML files or definitions utilized in this mod. The mod's functionality is implemented entirely through C# code and Harmony patching.

## Harmony Patching
Harmony is employed to patch and override the Storytellers' default incident frequency logic in RimWorld. The following are key aspects of the Harmony integration:
- **Method Replacements**: The mod uses method replacement techniques to alter how frequently incidents are triggered. Relevant methods include:
  - `MethodReplacer_IncidentCountThisInterval`
  - `MethodReplacer_MTBEventOccurs`
- **Transpilers**: Advanced patches that dynamically modify the game code to implement the multiplier effect on event frequency.

## Suggestions for Copilot
When using GitHub Copilot to aid in the further development of this mod, consider the following strategies:
- **Enhancements to Settings UI**: Generate suggestions for improving the user interface for adjusting settings, ensuring that changes are intuitive and easily accessible.
- **Error Handling Routines**: Obtain snippets or blocks of code focused on improved error handling, particularly for extreme multiplier values.
- **Performance Optimization**: Seek guidance from Copilot for optimizing Harmony patches to minimize any potential performance impact during gameplay.
- **Debugging Assistance**: Use Copilot to suggest additional logging or debugging code that can assist in diagnosing issues reported by users in the game's log uploader.

By following these instructions, developers can effectively maintain and enhance the "Incident Count Multiplier (Continued)" mod for RimWorld, leveraging GitHub Copilot as a tool for productivity and innovation.

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

