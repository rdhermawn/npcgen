# Npcgen2CoordData

A Windows utility to import NPC/mob spawn data from `npcgen.data` into `coord_data.txt` for Perfect World game servers.

## Features

- One-click pipeline: Open → Import → Save
- Per-location update: removes old entries for the specified location before importing new data
- Supports mobs and resources from npcgen.data
- Preserves data from other locations (e.g. importing `world` won't affect `a78` entries)

## Usage

1. Click **"Import npcgen.data & Save coord_data.txt"**
2. Select your `coord_data.txt` file
3. Select your `npcgen.data` file
4. Enter the location name (e.g. `world`, `a78`, `a64`)
5. The tool will:
   - Load existing coord_data.txt
   - Load npcgen.data
   - Remove all old entries matching the specified location
   - Import new mob/resource coordinates from npcgen.data
   - Save the updated coord_data.txt

## How It Works

| Scenario | Result |
|----------|--------|
| Entry exists with same location | Old entry **removed**, replaced with new data |
| Entry exists with different location | **Untouched** |
| New ID from npcgen.data | **Added** as new entry |

## Requirements

- .NET Framework 3.5+
- Windows

## Build

Open `Npcgen2CoordData.sln` in Visual Studio and build, or use MSBuild:

```
msbuild Npcgen2CoordData.csproj /p:Configuration=Release
```

## License

See [LICENSE](LICENSE) file.
