using Godot;

/// <summary>
/// Bullet sprite FX
/// </summary>
public partial class BulletSprite : Sprite2D
{
    // Editor State
    [Export] public Color Color = Color.FromHsv(0.5f, 0.8f, 0, 8f);
    [Export] public float Speed = .1f;

    // Private state
    private float Hue;
    private float Sat;
    private float Val;

    /// <summary>
    /// Called when the node enters the scene tree for the first time.
    /// </summary>
    public override void _Ready()
    {
        Color.ToHsv(out Hue, out Sat, out Val);
    }

    /// <summary>
    /// Rotate the color
    /// </summary>
    public override void _Process(double delta)
    {
        Hue += (float)delta * Speed;
        this.Modulate = Color.FromHsv(Hue, Sat, Val);
    }
}
