# Npcgen2CoordData

A Windows utility to import NPC/mob spawn data from `npcgen.data` into `coord_data.txt` for Perfect World game servers.

## Features

- Bulk import without opening an existing `coord_data.txt`
- Bulk import creates a new `bulk/coord_data.txt` automatically
- Bulk import also creates per-location files such as `bulk/world.txt`, `bulk/a11.txt`, `bulk/a10.txt`
- Manual import opens an existing `coord_data.txt`, then imports one or many `npcgen.data` files
- Per-location update: removes old entries for the target location before importing new data
- Supports mobs and resources from `npcgen.data`
- Preserves data from other locations (e.g. importing `world` won't affect `a78` entries)

## Usage

Use one of the import modes below.

### Bulk Import

Use **Bulk Import (Select Folder)** when your `npcgen.data` files are inside map/location folders and you want the app to create a new `coord_data.txt` automatically.

Example folder:

```
165/
├── world/npcgen.data
├── a78/npcgen.data
└── a64/npcgen.data
```

Steps:

1. Click **Bulk Import (Select Folder)**.
2. Paste or enter the root folder path, e.g. `H:\DATA PW\PWServer\Tools PW\npcgen2coord_data\165`.
3. The tool scans all subfolders for `npcgen.data`.
4. Location name is taken from the parent folder name:
   - `world/npcgen.data` → `world`
   - `a78/npcgen.data` → `a78`
   - `a64/npcgen.data` → `a64`
5. A preview confirms what will be imported.
6. A new `bulk/coord_data.txt` is created in the app folder.
7. A per-location file is also created for each map.

Bulk output example:

```
Npcgen2CoordData.exe
bulk/
├── coord_data.txt
├── world.txt
├── a78.txt
└── a64.txt
```

### Manual Import

Use **Manual Import (Select Files)** when you want to select files directly and confirm/edit each location.

1. Click **Open coord_data.txt** and select your existing `coord_data.txt` file.
2. Click **Manual Import (Select Files)**.
3. Select one or more `npcgen.data` files.
4. Confirm or edit the location for each file.
5. Old entries for each selected location are removed, then new data is imported.
6. The selected `coord_data.txt` is saved directly.
7. Per-location files are created in the app folder.

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
