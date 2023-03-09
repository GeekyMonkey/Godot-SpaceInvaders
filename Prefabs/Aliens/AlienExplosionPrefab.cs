using Godot;
using System;

public partial class AlienExplosionPrefab : Node2D
{
    private double Age = 0f;

    [Export]
    public double FadeSeconds = 1.1f;

    private Sprite2D Sprite;
    private AudioStreamPlayer2D ExplosionSound;


    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        Sprite = GetNode<Sprite2D>("./AlienDeathSprite");
        ExplosionSound = GetNode<AudioStreamPlayer2D>("./ExplosionSound");
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
    {
        Age += delta;
        float opacity = Math.Clamp(1f - (float)(Age / FadeSeconds), 0, 1f);
        Sprite.Modulate = new Color(1f, 1f, 1f, opacity);
        if (opacity <= 0 && !ExplosionSound.Playing)
        {
            QueueFree();
        }
    }
}
