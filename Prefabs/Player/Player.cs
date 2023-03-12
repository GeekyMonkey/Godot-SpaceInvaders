using Godot;

public partial class Player : GmNode2D
{
    // Editor State
    [Export] public float MoveSpeed = 500;
    [Export] public float ReloadSec = 0.25f;

    // Private State
    private Marker2D GunPosition;
    private float ScreenSizeX;
    private float XMin;
    private float XMax;
    private float XMargin = 16f;

    /// <summary>
    /// Player added to scene
    /// </summary>
    public override void _Ready()
    {
        GunPosition = GetNode<Marker2D>("GunPosition");
        ScreenSizeX = GetViewportRect().Size.X / 3;
        XMin = ScreenSizeX / -2 + XMargin;
        XMax = ScreenSizeX / 2 - XMargin;
    }

    /// <summary>
    /// Called every frame. 'delta' is the elapsed time since the previous frame.
    /// </summary>
    public override void _Process(double delta)
    {
        ProcessMoveInput(delta);
        ProcessShootInput(delta);
    }

    /// <summary>
    /// Left/right movement
    /// </summary>
    private void ProcessMoveInput(double delta)
    {
        float input = 0;
        if (Input.IsActionPressed("ui_right"))
        {
            input = 1;
        }
        else if (Input.IsActionPressed("ui_left"))
        {
            input = -1;
        }

        float newX = Position.X + input * MoveSpeed * (float)delta;
        newX = Mathf.Clamp(newX, XMin, XMax);
        newX = Mathf.Round(newX);
        Position = new Vector2(newX, Position.Y);
    }

    /// <summary>
    /// Shoot Input
    /// </summary>
    private void ProcessShootInput(double delta)
    {
        float input = 0;
        if (Input.IsActionJustPressed("Shoot"))
        {
            input = 1;
        }

        if (input == 1)
        {
            SpawnPrefabAtRoot<Bullet>((bullet) =>
            {
                bullet.GlobalPosition = GunPosition.GlobalPosition;
            });
        }
    }

    /// <summary>
    /// Something collided with the player
    /// </summary>
    public async void OnBodyEntered(Node2D other)
    {
        GD.Print("Player hit by " + other.Name);
        if (other is Bomb bomb)
        {
            SpawnPrefabAtRoot<Player_Explosion>((explosion) =>
            {
                explosion.Position = GlobalPosition;
            });

            await DelaySec(0.1);
            QueueFree();
            GameManager.PlayerDied();
        }
    }
}
