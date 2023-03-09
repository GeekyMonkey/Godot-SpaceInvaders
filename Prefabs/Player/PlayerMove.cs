using Godot;
using System;

public partial class PlayerMove : Node2D
{
    [Export]
    public float MoveSpeed = 500;

    [Export]
    public float ReloadSec = 0.25f;

    [Export]
    public Marker2D GunPosition;

    [Export]
    public PackedScene BulletPrefab;

    public float ScreenSizeX;
    public float XMin;
    public float XMax;
    private float XMargin = 16f;

    // private CustomSignals cs;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        ScreenSizeX = GetViewportRect().Size.X / 3;
        XMin = ScreenSizeX / -2 + XMargin;
        XMax = ScreenSizeX / 2 - XMargin;
        GD.Print("Player xMin=" + XMin + "  xMax=" + XMax);

        // cs = this.GetCustomSignals();
        // cs.Connect("AlienDied", Callable.From((Node alien) => OnAlienDied(alien)));
    }

    // public void OnAlienDied(Node alien)
    // {
    //     GD.Print("Player knows " + alien.Name + " is dead!");
    // }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
    {
        ProcessMoveInput(delta);
        ProcessShootInput(delta);
    }

    // Left/right movement
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

    // Left/right movement
    private void ProcessShootInput(double delta)
    {
        float input = 0;
        if (Input.IsActionJustPressed("Shoot"))
        {
            input = 1;
        }

        if (input == 1)
        {
            Node2D bullet = BulletPrefab.Instantiate<Node2D>(PackedScene.GenEditState.Instance);
            bullet.Position = GunPosition.GlobalPosition;
            GetTree().CurrentScene.AddChild(bullet);
            GD.Print(GunPosition.GlobalPosition);
        }
    }
}
