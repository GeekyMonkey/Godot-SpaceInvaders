using Godot;

public partial class Bomb : RigidBody2D
{
    // Editor State
    [Export] public float Speed = 50f;

    /// <summary>
    /// Bomb added to scene
    /// </summary>
    public override void _Ready()
    {
        // Start moving down
        LinearVelocity = Vector2.Down * Speed;

        // Watch for collisions
        Connect(SignalName.BodyEntered, Callable.From((Node o) => OnBodyEntered(o)));
    }

    /// <summary>
    /// Bomb collided with something
    /// </summary>
    private void OnBodyEntered(Node other)
    {
        if (other is Bullet)
        {
            // GD.Print("Bomb " + this.Name + " hit " + other.Name);
            // Show an explosion where the bullet hit the bomb
            this.SpawnPrefabAtRoot<Bomb_Explosion>((bombExp) =>
            {
                bombExp.GlobalPosition = GlobalPosition;
            }, "Bomb_Explosion");
        }
        else
        {
            // ToDo: bomb collide with player
            // ToDo: bomb collide with shield
        }

        // Remove the bomb
        QueueFree();
    }
}
