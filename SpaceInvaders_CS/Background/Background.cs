using Godot;

/// <summary>
/// Background Image
/// </summary>
public partial class Background : GmSprite2D
{
    /// <summary>
    /// Called when the node enters the scene tree for the first time.
    /// </summary>
    public override void _Ready()
    {
        // Size the sprite texture
        var vpSize = GetViewportRect().Size;
        var textureSize = Texture.GetSize();
        var scale = vpSize.X / textureSize.X;
        ApplyScale(new Vector2(scale, scale));
        GD.Print("Background scale = " + scale);
    }
}
