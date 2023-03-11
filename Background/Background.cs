using Godot;

/// <summary>
/// Background Image
/// </summary>
public partial class Background : Sprite2D
{
    /// <summary>
    /// Called when the node enters the scene tree for the first time.
    /// </summary>
    public override void _Ready()
    {
        // Size the sprite texture
        var vpSize = GetViewportRect().Size;
        var textureSize = this.Texture.GetSize();
        var scale = vpSize.X / textureSize.X;
        this.ApplyScale(new Vector2(scale, scale));
        GD.Print("Background scale = " + scale);
    }
}
