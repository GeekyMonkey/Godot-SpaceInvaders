using Godot;
using System;
using System.Threading.Tasks;

public partial class Alien1 : RigidBody2D
{
    private CustomSignals cs;
    private bool Dead = false;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        cs = this.GetCustomSignals();
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
    {
    }

    private void OnBodyEntered(Node otherObject)
    {
        // GD.Print(this.Name + " hit by " + otherObject.Name + " in group " + otherObject.GetGroups());
        if (otherObject.IsInGroup("Missiles"))
        {
            HitByMissile(otherObject);
        }
    }

    private async void HitByMissile(Node missile)
    {
        if (!Dead)
        {
            Dead = true;
            // GetTree().CallGroup("Players", nameof(PlayerMove.MethodName.OnAlienDied), this);
            cs.EmitAlienDied(this);

            await this.NextIdle();
            this.QueueFree();
        }
    }
}
