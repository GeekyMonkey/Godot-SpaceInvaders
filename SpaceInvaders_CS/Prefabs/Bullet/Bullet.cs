using Godot;

/// <summary>
/// Bullet Prefab
/// </summary>
public partial class Bullet : GmRigidBody2D
{
    // Editor State
    [Export] public float Speed = 400f;

    /// <summary>
    /// Called when the node enters the scene tree for the first time.
    /// </summary>
    public override void _Ready()
    {
        // Start it moving
        LinearVelocity = Vector2.Up * Speed;

        // Watch for collisions
        Connect(SignalName.BodyEntered, Callable.From((Node other) => OnBodyEntered(other)));
    }

    /// <summary>
    /// Something collided with the bullet. The nerve!
    /// </summary>
    private void OnBodyEntered(Node other)
    {
        if (other is Alien alien)
        {
            BulletHitAlien(alien);
        }
        else if (other is Bomb bomb)
        {
            BulletHitBomb(bomb);
        }
        else
        {
            GD.Print("Bullet hit " + other);
            // ToDo: - hit Shield
            // ToDo: - hit UFO
        }
    }

    /// <summary>
    /// We have struck an alien!
    /// </summary>
    private void BulletHitAlien(Alien alien)
    {
        // The alien knows this is bad. Just remove the bullet
        QueueFree();
    }

    /// <summary>
    /// The bullet hit a bomb. What luck!
    /// </summary>
    private void BulletHitBomb(Bomb bomb)
    {
        // GD.Print("Bullet hit bomb " + bomb.Name);
        GameManager.ScoreAdd(3);

        // Delete the bullet from the scene
        QueueFree();
    }
}
