<p align="center">
  <img src="https://github.com/Rhythia/Client/blob/master/textures/RhythiaSquircle.png?raw=true" alt="Rhythia" width="400"/>
</p>

<p align="center">
  <b>English</b> · <a href="README.es.md">Español</a>
</p>

<h3 align="center">Rhythia for Android: the 3×3 grid rhythm game, now with touch controls.</h3>

<p align="center">
  <a href="https://github.com/neruson27/Client/releases"><img src="https://img.shields.io/github/v/release/neruson27/Client?include_prereleases&label=APK&logo=android&logoColor=white&color=3DDC84" alt="Latest release"></a>
  <a href="https://godotengine.org/download"><img src="https://img.shields.io/badge/Godot-4.7.2%20.NET-%23478CBF?logo=godot-engine&logoColor=white" alt="Godot 4.7.2"></a>
</p>

---

> [!IMPORTANT]
> **This is an unofficial mobile port** of [Rhythia/Client](https://github.com/Rhythia/Client). It is not maintained by the Rhythia team: report issues with this version in [this fork's issues](https://github.com/neruson27/Client/issues), not on the Rhythia Discord.

> [!CAUTION]
> Rhythia is **not** affiliated with CAPO Games, Steam Rhythia, or rhythia.com.

---

## Table of Contents

- [Installation](#installation)
- [Playing on your phone](#playing-on-your-phone)
- [Getting maps](#getting-maps)
- [Differences from the PC version](#differences-from-the-pc-version)
- [Known issues](#known-issues)
- [Building the APK](#building-the-apk)
- [Keeping the fork up to date](#keeping-the-fork-up-to-date)
- [License](#license)

---

## Installation

1. Download **[Rhythia.apk](https://github.com/neruson27/Client/releases/download/v0.2.0-android/Rhythia.apk)** on your phone (also on the [releases](https://github.com/neruson27/Client/releases) page).
2. Open it and, if Android asks, allow your browser or file manager to **install unknown apps**.
3. Open **Rhythia** from your app drawer.

**Requirements:** Android with a 64-bit ARM CPU (`arm64-v8a`, practically any phone from the last few years) and Vulkan support.

> [!NOTE]
> The APK is currently signed with a debug key. If a future version ships with a different signature, you will have to uninstall the old one first, which deletes your maps and settings.

---

## Playing on your phone

- The game plays in **landscape** (it rotates with how you hold the phone).
- The cursor uses **absolute input**: it follows your finger and **jumps to wherever you tap**, no need to drag from the center.
- On mobile the **hitbox grows with the cursor size** to make up for a finger being less precise than a mouse.

Useful settings:

| Setting | Where | What for |
|---|---|---|
| **Cursor Scale** | Settings → Visual | Size of the cursor **and** the hitbox. 1.5 by default; raise it if you keep barely missing notes. |
| **Absolute Sensitivity** | Settings → Gameplay | How far the cursor moves relative to your finger. If the cursor lags behind your finger, try ~1.25. |
| **Absolute Input** | Settings → Gameplay | On by default on mobile; leave it on to play with your finger. |

---

## Getting maps

The game ships without maps. You can find them on the [Rhythia Discord](https://discord.gg/rhythia).

1. Download the map (`.sspm`, `.phxm`, ...) on your phone.
2. In the game go to **Play → Import** and pick it with the Android file picker.

Game data (maps, settings, replays) lives in the app's private storage, so it is deleted if you uninstall the app.

---

## Differences from the PC version

Everything mobile-specific only kicks in on Android (`OS.HasFeature("mobile")`); the same code keeps working unchanged on PC.

- **UI:** adaptive scaling (`canvas_items`, 864 base height) so text is readable without panels getting cut off, and it stays clear of the front camera cutout (`scripts/util/SafeArea.cs`).
- **Touch input:** the cursor jumps to the tapped point and latency is reduced (no input accumulation, `agile_event_flushing` and 2 swapchain images).
- **Hitbox:** `0.07 × Cursor Scale` on mobile, so **mobile scores are not comparable with PC scores**.
- **Map import:** `content://` URIs from the Android picker are copied to the cache before importing.
- **No** Discord Rich Presence or auto-updater.
- **.NET 9 on Android:** the Godot 4.7.2 export template only supports `net9.0`; PC stays on .NET 10.
- **SQLite:** upgraded to `sqlite-net-pcl` 1.11.285, which ships the native library for Android.

---

## Known issues

- C# on Android is still **experimental** in Godot.
- The **Restart** button in settings only closes the game; you have to reopen it yourself.
- Buttons and text designed for a mouse can still be a bit small on very small screens.

---

## Building the APK

### Requirements

| Tool | Version |
|---|---|
| [Godot (.NET)](https://godotengine.org/download) | **4.7.2** + its export templates |
| [.NET SDK](https://dotnet.microsoft.com/download) | **10.0** |
| JDK | 17 or newer |
| Android SDK | platform-tools, build-tools and a recent platform |
| [Git LFS](https://git-lfs.github.com/) | For textures and audio |

### Steps

```bash
git clone https://github.com/neruson27/Client.git
cd Client
git lfs pull
dotnet tool restore
```

In Godot:

1. **Editor → Manage Export Templates → Download and Install**.
2. **Editor Settings → Export → Android**: paths to the Android SDK and the JDK (plus the debug keystore, which Android Studio creates at `~/.android/debug.keystore`).
3. **Project → Export → Android → Export Project**, or the Android button in the top right to install straight to a phone with USB or wireless debugging.

Or from the terminal:

```bash
godot --headless --path . --export-debug "Android" Rhythia.apk
adb install -r Rhythia.apk
```

> [!TIP]
> Exporting modifies `packages.lock.json` (the Android configuration uses a different runtime). Restore it with `dotnet restore --force-evaluate` before committing.

Before pushing changes, format the code like the upstream project does:

```bash
dotnet format Rhythia.csproj
dotnet csharpier format .
```

---

## Keeping the fork up to date

This fork's `master` branch is a copy of the upstream project; `main` holds the Android port. To pull in Rhythia updates:

```bash
git remote add upstream https://github.com/Rhythia/Client.git
git fetch upstream
git merge upstream/master
```

---

## License

Rhythia is licensed under the [GNU Affero General Public License v3.0](LICENSE). This fork keeps the same license; the source code for this modified version is available in this repository.

Game credits go to the [Rhythia](https://github.com/Rhythia/Client) team and contributors.
