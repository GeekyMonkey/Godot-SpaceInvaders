using Godot;
using System;

public partial class BulletPrefab : RigidBody2D
{
    [Export]
    public float Speed = 400f;

    private CustomSignals cs;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        cs = this.GetCustomSignals();
        LinearVelocity = Vector2.Up * Speed;
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
    {
    }

    private void OnBodyEntered(Node body)
    {
        if (body.IsInGroup("Aliens"))
        {
            MissileHitAlien(body);
        }
        // todo - hit missiles
        // todo - hit shield
        // todo - hit ufo
    }

    private async void MissileHitAlien(Node alien)
    {
        cs.EmitStomp();
        await this.NextIdle();
        this.QueueFree();
    }
}
