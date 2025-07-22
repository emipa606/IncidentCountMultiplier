# GitHub Copilot Instructions for RimWorld Mod Development

## Mod Overview and Purpose

The mod, referred to as "Incident Count Multiplier," aims to enhance the gameplay experience in RimWorld by allowing players to customize the frequency of incidents. This flexibility provides players with a tailored challenge level and can adapt the game dynamics based on personal preferences.

## Key Features and Systems

### 1. Incident Customization
- **SimpleCurveEditor:** A method in the `IncidentCountMultiplier` class that facilitates editing curves to define how the frequency of incidents adjusts as the game progresses.
  
### 2. Mod Settings
- **Settings Storage:** The `IncidentCountMultiplierSettings` class extends `ModSettings` and stores user-defined settings in a persistent manner.

### 3. Mod Functionality
- **Harmony Patching:** The mod employs Harmony to inject or override existing game logic, ensuring seamless integration without altering the original game files.

## Coding Patterns and Conventions

- **Class Naming:** Classes begin with uppercase letters and are named using PascalCase (e.g., `IncidentCountMultiplier`, `IncidentCountMultiplierSettings`).
- **Method Naming:** Methods also use PascalCase and should describe the action they perform (e.g., `SimpleCurveEditor`).
- **Access Modifiers:** Methods and classes are appropriately marked with access modifiers to highlight their visibility and intended usage, for example, `public` for accessible classes and methods.

## XML Integration

The mod likely integrates XML to define game objects or settings which were not explicitly detailed in the summary provided. Typically, XML files in RimWorld mods define items, incidents, and map configurations. Ensure XML files follow:

- **Schema Conformity:** Adhere to RimWorld's XML schemas for compatibility.
- **Tag Naming:** Use lowercase with hyphens where necessary (e.g., `<incident>`).
- **Patch Operations:** Utilize xml for patch operations like `PatchOperationAdd` or `PatchOperationReplace` for smooth data integration.

## Harmony Patching

### Key Concepts:
- **Patch Class:** `IncidentCountMultiplier_Harmony` is the dedicated class for setting up Harmony patches. Patching aims to modify behavior at runtime.
- **Prefix and Postfix Methods:** Use these to insert logic before or after a game method is called, allowing for flexible modification.
- **Debugging:** Use Harmony's debugging tools to trace patches and resolve conflicts or unintended behaviors.

## Suggestions for Copilot

1. **Autocompletion of Harmony Patches:** When adding a new Harmony patch, Copilot can suggest boilerplate code, including the creation of `[HarmonyPatch]`, prefix, and postfix methods.
   
2. **Pattern Recognition:** Use Copilot to detect common coding patterns, like repetitive settings management, and automate the generation of similar patterns across different classes or methods.

3. **XML File Assistance:** Copilot can predict and complete common XML tags and attributes used in RimWorld mods, aiding in quicker data integration.

4. **Tooling Suggestions:** Recommend development environment settings for optimal use of Harmony and XML, perhaps suggesting plugins or extensions that facilitate XML editing or debugging.

By using these instructions, developers can leverage GitHub Copilot to enhance code efficiency and maintain higher standards while working on the RimWorld modding project.
