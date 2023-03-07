using Godot;
using System;

public partial class BulletSprite : Sprite2D
{
    [Export]
    public Color Color = Color.FromHsv(0.5f, 0.8f, 0, 8f);

    [Export()]
    public float Speed = .1f;

    private float Hue;
    private float Sat;
    private float Val;


    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        Color.ToHsv(out Hue, out Sat, out Val);
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
    {
        Hue += (float)delta * Speed;
        this.Modulate = Color.FromHsv(Hue, Sat, Val);
    }
}
