using Godot;
using System;

public partial class Bomb_Explosion : Node2D
{

    private double Age = 0f;

    [Export]
    public double FadeSeconds = 1.1f;

    private Sprite2D Sprite;
    private AudioStreamPlayer2D ExplosionSound;
    private PointLight2D ExplosionLight;
    private float LightEnergy;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        Sprite = GetNode<Sprite2D>("./BombExplosionSprite");
        ExplosionSound = GetNode<AudioStreamPlayer2D>("./ExplosionSound");
        ExplosionLight = GetNode<PointLight2D>("./ExplosionLight");
        LightEnergy = ExplosionLight.Energy;
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
    {
        Age += delta;
        float opacity = Math.Clamp(1f - (float)(Age / FadeSeconds), 0, 1f);
        Sprite.Modulate = new Color(Sprite.Modulate.R, Sprite.Modulate.G, Sprite.Modulate.B, opacity);
        if (opacity <= 0 && !ExplosionSound.Playing)
        {
            QueueFree();
        }
        ExplosionLight.Energy = LightEnergy * opacity;
    }
}
