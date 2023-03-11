using Godot;
using System;

public partial class Player_Explosion : Node2D
{
    private double Age = 0f;

    [Export]
    public double FadeSeconds = 2.1f;

    private AnimatedSprite2D Sprite;
    private AudioStreamPlayer2D ExplosionSound;
    private PointLight2D ExplosionLight;
    private float LightEnergy;

    public override void _Ready()
    {
        Sprite = GetNode<AnimatedSprite2D>("./PlayerDeathSprite");
        ExplosionSound = GetNode<AudioStreamPlayer2D>("./ExplosionSound");
        ExplosionLight = GetNode<PointLight2D>("./ExplosionLight");
        LightEnergy = ExplosionLight.Energy;
    }

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
