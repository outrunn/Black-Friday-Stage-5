# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Project Overview

This is a 2D top-down Unity game called "Stage5BlackFridayGame" built with Unity 2D packages. The game features player movement, ai enemies, collectible gathering, lives/damage system, and state management.

## Key Architecture

### GameStateManager (Singleton Pattern)
- Central state controller located in `Assets/Scripts/GameStateManager.cs`
- Manages game states: Tutorial → Playing → Victory/GameOver
- Persists across scenes using DontDestroyOnLoad
- Controls UI panel visibility based on current state
- **CRITICAL**: Must be present in every scene with proper references to UI panels
- Access via `GameStateManager.Instance` (always null-check before use)

### Player System
- `PlayerMovement.cs`: Top-down movement with WASD/Arrow keys, rotation toward movement direction
- Uses Rigidbody2D with zero gravity for 2D top-down physics
- Lives system with visual damage feedback (red flash effect)
- Depends on GameStateManager to lock movement during non-Playing states

### Collectible & Detection Systems
- `Collectible.cs`: Trigger-based collectible pickup
- `PlayerDetection.cs`: Enemy collision detection with damage/lives management
- Both systems reference GameStateManager singleton

### Scene Structure
The project uses multiple scenes for different team members:
- JamesScene.unity, Grace Scene.unity, Josh Scene.unity, Orel Scene.unity
- Each team member works in their own scene to minimize conflicts

## Unity Team Collaboration Workflow

### Scene Management Strategy
1. **One Scene Per Developer**: Each team member has their own scene to work in
2. **Shared Prefabs**: Common elements (Player, Enemies, Collectibles) are prefabs in `Assets/Prefabs/`
3. **Scene Merging**: Only merge scenes when necessary, prefer working in separate scenes
4. **Main Scene**: Designate one scene as the "main" playable scene

### Git Workflow for Unity Teams

#### Before Starting Work
```bash
git pull origin main
```

#### Daily Work Cycle
1. Work in your designated scene (e.g., JamesScene.unity)
2. Commit frequently with descriptive messages
3. Always test in Unity before committing

#### Committing Changes
```bash
git add Assets/Scenes/[YourScene].unity
git add Assets/Scripts/[YourScript].cs
git commit -m "Brief description of changes"
```

#### Pushing Changes
```bash
git pull origin main  # Pull latest changes first
# Resolve any conflicts in Unity if they occur
git push origin main
```

#### Handling Scene Conflicts
Unity scene files (.unity) are YAML but difficult to merge manually:
- **Prevention**: Avoid multiple people editing the same scene simultaneously
- **Resolution**: If conflicts occur, choose one version or recreate changes in Unity
- **Best Practice**: Communicate with team about which scenes are being edited

### Prefab-First Development
- Create new game objects as prefabs first in `Assets/Prefabs/`
- Reference prefabs in scenes rather than duplicating objects
- This minimizes scene file conflicts since prefab changes are separate files

### Critical Unity Files to Commit
- Scene files: `Assets/Scenes/*.unity`
- Scripts: `Assets/Scripts/**/*.cs`
- Prefabs: `Assets/Prefabs/**/*.prefab`
- Tile Palettes: `Assets/TilePalettes/**/*`
- RuleTiles: `Assets/Scripts/Ruletile/**/*.asset`

### Files Ignored by Git
The `.gitignore` is properly configured to exclude:
- Library/, Temp/, Obj/ (Unity-generated)
- .vs/, .vscode/ (IDE settings)
- *.csproj, *.sln (auto-generated)

## Common Development Commands

### Opening the Project
Open Unity Hub → Select this project folder → Launch with Unity 2D

### Running the Game
- In Unity Editor: Press Play button or Ctrl/Cmd+P
- Select a scene from `Assets/Scenes/` and ensure GameStateManager is set up

### Testing Scripts
Unity uses the Unity Test Framework:
- Window → General → Test Runner
- Create test assemblies in `Assets/Scripts/Tests/`

## Common Issues & Solutions

### NullReferenceException with GameStateManager
**Problem**: Scripts access `GameStateManager.Instance` before it's initialized

**Solution**: Always null-check before accessing:
```csharp
if (GameStateManager.Instance != null && GameStateManager.Instance.CurrentState != GameState.Playing)
{
    return;
}
```

### Scene Won't Load / Missing References
**Problem**: GameStateManager in scene missing UI panel references

**Solution**:
1. Check GameStateManager GameObject exists in scene
2. Verify all SerializedFields are assigned in Inspector
3. Ensure panels (tutorialPanel, gameHudPanel, etc.) are referenced

### Merge Conflict in Scene File
**Problem**: Two developers edited the same scene

**Solution**:
1. Accept one version completely: `git checkout --theirs Assets/Scenes/[Scene].unity`
2. Or accept yours: `git checkout --ours Assets/Scenes/[Scene].unity`
3. Re-apply changes manually in Unity Editor
4. Never attempt to manually merge Unity scene files

## Project Dependencies
- TextMeshPro (for UI text)
- Unity 2D Pixel Perfect
- Unity 2D Tilemap
- Unity 2D PSD Importer
- Unity 2D Animation

## Code Style Notes
- MonoBehaviour scripts use SerializeField for Inspector-editable fields
- Private fields follow camelCase convention
- Public properties use PascalCase
- Singleton pattern used for managers (GameStateManager)
