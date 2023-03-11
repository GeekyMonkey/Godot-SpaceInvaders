using Godot;
using System;

/// <summary>
/// Bomb Explosion
/// </summary>
public partial class Bomb_Explosion : Node2D
{
    // Editor State
    [Export] public double FadeSeconds = 1.1f;

    // Private state
    private double Age = 0f;
    private AudioStreamPlayer2D ExplosionSound;
    private PointLight2D ExplosionLight;
    private Sprite2D Sprite;
    private float LightEnergy;

    /// <summary>
    /// Called when the node enters the scene tree for the first time.
    /// </summary>
    public override void _Ready()
    {
        Sprite = GetNode<Sprite2D>("./BombExplosionSprite");
        ExplosionSound = GetNode<AudioStreamPlayer2D>("./ExplosionSound");
        ExplosionLight = GetNode<PointLight2D>("./ExplosionLight");
        LightEnergy = ExplosionLight.Energy;
    }

    /// <summary>
    /// Fade out the explosion
    /// </summary>
    public override void _Process(double delta)
    {
        Age += delta;

        // Fade the sprite
        float opacity = Math.Clamp(1f - (float)(Age / FadeSeconds), 0, 1f);
        Sprite.Modulate = new Color(Sprite.Modulate.R, Sprite.Modulate.G, Sprite.Modulate.B, opacity);

        // Fade the light
        ExplosionLight.Energy = LightEnergy * opacity;

        // Done fading - remove the explosion
        if (opacity <= 0 && !ExplosionSound.Playing)
        {
            QueueFree();
        }
    }
}
