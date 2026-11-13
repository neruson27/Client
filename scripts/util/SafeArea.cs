using Godot;

/// <summary>
/// Keeps scene UI out of display cutouts (front camera holes) on mobile.
/// Backdrops (ColorRect, TextureRect, SubViewportContainer) keep covering the whole screen,
/// only the controls inside them get inset.
/// </summary>
public static class SafeArea
{
    private const string applied_meta = "safe_area_applied";

    public static void Apply(Node parent)
    {
        if (!OS.HasFeature("mobile") || parent == null)
        {
            return;
        }

        Window root = parent.GetTree().Root;
        Rect2I safeArea = DisplayServer.GetDisplaySafeArea();
        Vector2I windowSize = DisplayServer.WindowGetSize();
        // Screen pixels per canvas unit, whatever stretch mode is active
        float scale = windowSize.Y / root.GetVisibleRect().Size.Y;

        // left, top, right, bottom in canvas units
        var margins = new Vector4(safeArea.Position.X, safeArea.Position.Y, windowSize.X - safeArea.End.X, windowSize.Y - safeArea.End.Y);
        margins = margins.Max(Vector4.Zero) / scale;

        foreach (Node child in parent.GetChildren())
        {
            if (child is Control control)
            {
                inset(control, margins);
            }
        }
    }

    private static void inset(Control control, Vector4 margins)
    {
        if (control is ColorRect or TextureRect or SubViewportContainer)
        {
            foreach (Node child in control.GetChildren())
            {
                if (child is Control childControl)
                {
                    inset(childControl, margins);
                }
            }

            return;
        }

        Vector4 previous = control.HasMeta(applied_meta) ? control.GetMeta(applied_meta).AsVector4() : Vector4.Zero;

        // Remap anchors from [0, size] to [margin start, size - margin end], undoing the previous inset first
        control.OffsetLeft += edgeShift(margins.X, margins.Z, control.AnchorLeft) - edgeShift(previous.X, previous.Z, control.AnchorLeft);
        control.OffsetRight += edgeShift(margins.X, margins.Z, control.AnchorRight) - edgeShift(previous.X, previous.Z, control.AnchorRight);
        control.OffsetTop += edgeShift(margins.Y, margins.W, control.AnchorTop) - edgeShift(previous.Y, previous.W, control.AnchorTop);
        control.OffsetBottom += edgeShift(margins.Y, margins.W, control.AnchorBottom) - edgeShift(previous.Y, previous.W, control.AnchorBottom);

        control.SetMeta(applied_meta, margins);
    }

    private static float edgeShift(float start, float end, float anchor) => start - anchor * (start + end);
}
