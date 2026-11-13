<p align="center">
  <img src="https://github.com/Rhythia/Client/blob/master/textures/RhythiaSquircle.png?raw=true" alt="Rhythia" width="400"/>
</p>

<h3 align="center">Rhythia para Android: el juego de ritmo de la cuadrícula 3×3, ahora con controles táctiles.</h3>

<p align="center">
  <a href="https://github.com/neruson27/Client/releases/latest"><img src="https://img.shields.io/github/v/release/neruson27/Client?include_prereleases&label=APK&logo=android&logoColor=white&color=3DDC84" alt="Última versión"></a>
  <a href="https://godotengine.org/download"><img src="https://img.shields.io/badge/Godot-4.7.2%20.NET-%23478CBF?logo=godot-engine&logoColor=white" alt="Godot 4.7.2"></a>
</p>

---

> [!IMPORTANT]
> **Este es un port no oficial para móvil** de [Rhythia/Client](https://github.com/Rhythia/Client). No lo mantiene el equipo de Rhythia: los problemas de esta versión se reportan en los [issues de este fork](https://github.com/neruson27/Client/issues), no en el Discord de Rhythia.

> [!CAUTION]
> Rhythia **no** está afiliado con CAPO Games, Steam Rhythia ni rhythia.com.

---

## Contenido

- [Instalación](#instalación)
- [Cómo jugar en el teléfono](#cómo-jugar-en-el-teléfono)
- [Conseguir mapas](#conseguir-mapas)
- [Diferencias con la versión de PC](#diferencias-con-la-versión-de-pc)
- [Problemas conocidos](#problemas-conocidos)
- [Compilar el APK](#compilar-el-apk)
- [Mantener el fork al día](#mantener-el-fork-al-día)
- [Licencia](#licencia)

---

## Instalación

1. Descarga **[Rhythia.apk](https://github.com/neruson27/Client/releases/latest/download/Rhythia.apk)** desde el teléfono (también en la página de [releases](https://github.com/neruson27/Client/releases)).
2. Ábrelo y, si Android lo pide, permite **instalar apps de fuentes desconocidas** para tu navegador o gestor de archivos.
3. Abre **Rhythia** desde el cajón de aplicaciones.

**Requisitos:** Android con CPU de 64 bits ARM (`arm64-v8a`, prácticamente cualquier teléfono de los últimos años) y soporte de Vulkan.

> [!NOTE]
> Por ahora el APK está firmado con una clave de depuración. Si en el futuro se publica con otra firma, habrá que desinstalar la versión anterior antes de instalar la nueva, y eso borra mapas y ajustes.

---

## Cómo jugar en el teléfono

- El juego se juega en **horizontal** (gira solo según cómo sostengas el teléfono).
- El cursor usa **entrada absoluta**: sigue a tu dedo y **salta a donde toques**, no hace falta arrastrar desde el centro.
- En móvil el **hitbox crece con el tamaño del cursor** para compensar que un dedo es menos preciso que un ratón.

Ajustes útiles:

| Ajuste | Dónde | Para qué |
|---|---|---|
| **Cursor Scale** | Settings → Visual | Tamaño del cursor **y** del hitbox. 1.5 por defecto; súbelo si fallas notas por poco. |
| **Absolute Sensitivity** | Settings → Gameplay | Cuánto se mueve el cursor respecto al dedo. Si el cursor se queda atrás del dedo, prueba ~1.25. |
| **Absolute Input** | Settings → Gameplay | Activado por defecto en móvil; déjalo así para jugar con el dedo. |

---

## Conseguir mapas

El juego no trae mapas. Se consiguen en el [Discord de Rhythia](https://discord.gg/rhythia).

1. Descarga el mapa (`.sspm`, `.phxm`, ...) en el teléfono.
2. En el juego ve a **Play → Import** y elígelo con el selector de archivos de Android.

Los datos del juego (mapas, ajustes, repeticiones) se guardan en el almacenamiento privado de la app, así que se borran si la desinstalas.

---

## Diferencias con la versión de PC

Todo lo específico de móvil se activa solo en Android (`OS.HasFeature("mobile")`); el mismo código sigue funcionando igual en PC.

- **Interfaz:** escala adaptativa (`canvas_items`, alto base 864) para que el texto sea legible sin que se corten los paneles, y respeta el recorte de la cámara frontal (`scripts/util/SafeArea.cs`).
- **Entrada táctil:** el cursor salta al punto tocado y se reduce la latencia (sin acumulación de input, `agile_event_flushing` y 2 imágenes de swapchain).
- **Hitbox:** `0.07 × Cursor Scale` en móvil, por eso **las puntuaciones de móvil no son comparables con las de PC**.
- **Importar mapas:** las URIs `content://` del selector de Android se copian a la caché antes de importarlas.
- **Sin** Discord Rich Presence ni actualizador automático.
- **.NET 9 en Android:** la plantilla de exportación de Godot 4.7.2 solo soporta `net9.0`; PC sigue en .NET 10.
- **SQLite:** actualizado a `sqlite-net-pcl` 1.11.285, que incluye la librería nativa para Android.

---

## Problemas conocidos

- El soporte de C# en Android sigue siendo **experimental** en Godot.
- El botón **Restart** de los ajustes solo cierra el juego; hay que volver a abrirlo a mano.
- Botones y textos pensados para ratón pueden quedar algo pequeños en pantallas muy chicas.

---

## Compilar el APK

### Requisitos

| Herramienta | Versión |
|---|---|
| [Godot (.NET)](https://godotengine.org/download) | **4.7.2** + sus export templates |
| [.NET SDK](https://dotnet.microsoft.com/download) | **10.0** |
| JDK | 17 o superior |
| Android SDK | platform-tools, build-tools y una plataforma reciente |
| [Git LFS](https://git-lfs.github.com/) | Para texturas y audio |

### Pasos

```bash
git clone https://github.com/neruson27/Client.git
cd Client
git lfs pull
dotnet tool restore
```

En Godot:

1. **Editor → Manage Export Templates → Download and Install**.
2. **Editor Settings → Export → Android**: rutas del Android SDK y del JDK (y el keystore de depuración, que Android Studio crea en `~/.android/debug.keystore`).
3. **Project → Export → Android → Export Project**, o el botón de Android arriba a la derecha para instalar directo en un teléfono con depuración USB o inalámbrica.

También desde la terminal:

```bash
godot --headless --path . --export-debug "Android" Rhythia.apk
adb install -r Rhythia.apk
```

> [!TIP]
> Exportar modifica `packages.lock.json` (la configuración de Android usa otro runtime). Restáuralo con `dotnet restore --force-evaluate` antes de hacer commit.

Antes de subir cambios, formatea el código como en el proyecto original:

```bash
dotnet format Rhythia.csproj
dotnet csharpier format .
```

---

## Mantener el fork al día

La rama `master` de este fork es una copia del proyecto original; `main` tiene el port para Android. Para traer novedades de Rhythia:

```bash
git remote add upstream https://github.com/Rhythia/Client.git
git fetch upstream
git merge upstream/master
```

---

## Licencia

Rhythia está bajo la [GNU Affero General Public License v3.0](LICENSE). Este fork mantiene la misma licencia; el código de esta versión modificada está disponible en este repositorio.

Créditos del juego al equipo y colaboradores de [Rhythia](https://github.com/Rhythia/Client).
