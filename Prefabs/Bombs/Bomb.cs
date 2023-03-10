using System;
using Godot;

public partial class Bomb : RigidBody2D
{
    [Export]
    public float Speed = 50f;

    private CustomSignals cs;

    [Export]
    private PackedScene BombExplosionPrefab;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        cs = this.GetCustomSignals();
        LinearVelocity = Vector2.Up * -Speed;
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
    {
    }

    public override void _PhysicsProcess(double delta)
    {
        // Position += new Vector2(0, Speed * (float)delta);
    }


    private void OnBodyEntered(Node other)
    {
        GD.Print("Bomb " + this.Name + " hit " + other.Name);
        var explosion = BombExplosionPrefab.Instantiate<BombExplosion>(PackedScene.GenEditState.Instance);
        explosion.GlobalPosition = GlobalPosition;
        GetTree().CurrentScene.AddChild(explosion);
    }
}
