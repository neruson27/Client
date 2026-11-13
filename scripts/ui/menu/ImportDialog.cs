using System;
using System.IO;
using System.Linq;
using Godot;

public partial class ImportDialog : FileDialog
{
    public override void _Ready()
    {
        FilesSelected += async (paths) =>
        {
            string[] localPaths = paths.Select(localizePath).ToArray();

            await MapParser.BulkImport(localPaths);

            foreach (string path in localPaths.Except(paths))
            {
                File.Delete(path);
            }
        };
    }

    // Android's native picker returns content:// URIs that System.IO can't open, copy those to a real file first
    private static string localizePath(string path)
    {
        if (!path.StartsWith("content://"))
        {
            return path;
        }

        string name = Uri.UnescapeDataString(path).Split('/', ':')[^1];
        string importFolder = $"{OS.GetCacheDir()}/import";
        string localPath = $"{importFolder}/{name}";

        using var source = Godot.FileAccess.Open(path, Godot.FileAccess.ModeFlags.Read);

        if (source == null)
        {
            Logger.Log($"Could not open {path} ({Godot.FileAccess.GetOpenError()})");
            return path;
        }

        Directory.CreateDirectory(importFolder);
        File.WriteAllBytes(localPath, source.GetBuffer((long)source.GetLength()));

        return localPath;
    }
}
