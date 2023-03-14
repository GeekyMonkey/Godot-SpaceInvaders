using Godot;
using System;

public partial class Player_Explosion : GmNode2D
{
    // Editor State
    [Export] public double FadeSeconds = 2.1f;

    // Private State
    private double Age = 0f;
    private PointLight2D ExplosionLight;
    private AudioStreamPlayer2D ExplosionSound;
    private float LightEnergy;
    private AnimatedSprite2D Sprite;

    /// <summary>
    /// Node started
    /// </summary>
    public override void _Ready()
    {
        Sprite = GetNode<AnimatedSprite2D>("./PlayerDeathSprite");
        ExplosionSound = GetNode<AudioStreamPlayer2D>("./ExplosionSound");
        ExplosionLight = GetNode<PointLight2D>("./ExplosionLight");
        LightEnergy = ExplosionLight.Energy;
    }

    /// <summary>
    /// Node Update
    /// </summary>
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
