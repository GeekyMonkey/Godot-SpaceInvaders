using Godot;

public partial class Bullet : RigidBody2D
{
    [Export]
    public float Speed = 400f;

    private CustomSignals cs;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        cs = this.GetCustomSignals();
        LinearVelocity = Vector2.Up * Speed;

        Connect(SignalName.BodyEntered, Callable.From((Node other) => OnBodyEntered(other)));
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
    {
    }

    private void OnBodyEntered(Node other)
    {
        if (other is Alien alien)
        {
            BulletHitAlien(alien);
        }
        else
        {
            GD.Print("Bullet hit " + other);
            if (other is Bomb bomb)
            {
                BulletHitBomb(bomb);
            }
        }
        // todo - hit missiles
        // todo - hit shield
        // todo - hit ufo
    }

    private async void BulletHitAlien(Alien alien)
    {
        await this.NextIdle();
        this.QueueFree();
    }

    private async void BulletHitBomb(Bomb bomb)
    {
        GD.Print("Bullet hit bomb " + bomb.Name);
        this.GetGameManager().ScoreAdd(3);
        await this.DelayMs(10);
        bomb.QueueFree();
        QueueFree();
    }
}
