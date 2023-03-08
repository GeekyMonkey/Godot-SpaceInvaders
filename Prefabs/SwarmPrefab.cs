using Godot;
using System;

public partial class SwarmPrefab : Node2D
{
    [Export]
    AudioStream[] StompSounds;

    AudioStreamPlayer2D StompSoundPlayer;

    private int StompSoundIndex = 0;
    private CustomSignals cs;

    public float ScreenSizeX;
    public float XMin;
    public float XMax;

    [Export]
    private float XMargin = 8f;

    private Rect2 SwarmExtents;

    [Export]
    public int StepX = 8;

    [Export]
    public int StepY = 8;

    public bool DirectionRight = true;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        cs = this.GetCustomSignals();
        cs.Connect("Stomp", Callable.From(() => Stomp()));
        StompSoundPlayer = GetNode<AudioStreamPlayer2D>("./StompSoundPlayer");

        ScreenSizeX = GetViewportRect().Size.X / 3;
        XMin = ScreenSizeX / -2 + XMargin;
        XMax = ScreenSizeX / 2 - XMargin;

        MeasureExtents();
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
    {
        // AudioStreamPlayer2D p;
        // p.Stream
    }

    public void StompTimer()
    {
        cs.EmitStomp();
    }

    public void Stomp()
    {
        StompSoundIndex++;
        StompSoundIndex %= StompSounds.Length;
        StompSoundPlayer.Stream = StompSounds[StompSoundIndex];
        StompSoundPlayer.Play();

        // todo - only do when an alien dies
        MeasureExtents();

        float dX = (DirectionRight ? 1f : -1f) * StepX;
        Position = Position + new Vector2(dX, 0);
        GD.Print("X=" + Position.X + " Dir=" + DirectionRight);
        if ((Position.X + SwarmExtents.Position.X) <= XMin || (Position.X + SwarmExtents.End.X) >= XMax)
        {
            DirectionRight = !DirectionRight;

            Position = Position + new Vector2(-dX, StepY);
        }
    }

    public async void MeasureExtents()
    {
        // todo - remove delay when fired at the correct time
        await this.DelayMs(1);
        var children = this.GetChildren(false);
        float left = float.MaxValue;
        float top = float.MaxValue;
        float bottom = float.MinValue;
        float right = float.MinValue;
        foreach (var child in children)
        {
            //GD.Print(child.Name);
            if (child is AlienPrefab alien)
            {
                Rect2 extents = alien.Extents;
                Vector2 pos = alien.Position;
                left = Math.Min(left, pos.X + extents.Position.X);
                top = Math.Min(top, pos.Y + extents.Position.Y);
                right = Math.Max(right, pos.X + extents.End.X);
                bottom = Math.Max(bottom, pos.Y + extents.End.Y);
            }
        }
        SwarmExtents = new Rect2(left, top, right - left, bottom - top);
        // var swarmExtentsLocal = new Rect2(left - GlobalPosition.X, top - GlobalPosition.Y, right - GlobalPosition.X, bottom - GlobalPosition.Y);
        GD.Print("Swarm extents: " + SwarmExtents);
        GetNode<Sprite2D>("ExtentsMarker1").Position = SwarmExtents.Position;
        GetNode<Sprite2D>("ExtentsMarker2").Position = SwarmExtents.End;
    }
}
