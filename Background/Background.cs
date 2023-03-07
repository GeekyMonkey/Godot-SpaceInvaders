using Godot;
using System;

public partial class Background : Sprite2D
{
    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        var vpSize = GetViewportRect().Size;
        var textureSize = this.Texture.GetSize();


        var scale = vpSize.X / textureSize.X;

        //# Optional: Center the sprite, required only if the sprite's Offset>Centered checkbox is set
        //$Sprite2D.set_position(Vector2(viewportWidth / 2, viewportHeight / 2))

        //# Set same scale value horizontally/vertically to maintain aspect ratio
        //# If however you don't want to maintain aspect ratio, simply set different
        //# scale along x and y
        //$Sprite2D.set_scale(Vector2(scale, scale))

        this.ApplyScale(new Vector2(scale, scale));

    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
    {
    }
}
