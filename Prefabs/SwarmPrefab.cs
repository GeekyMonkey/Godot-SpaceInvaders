using Godot;
using System;

public partial class SwarmPrefab : Node2D
{
    [Export]
    AudioStream[] StompSounds;

    AudioStreamPlayer2D StompSoundPlayer;

    private int StompSoundIndex = 0;
    private CustomSignals cs;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        cs = this.GetCustomSignals();
        cs.Connect("Stomp", Callable.From(() => Stomp()));
        StompSoundPlayer = GetNode<AudioStreamPlayer2D>("./StompSoundPlayer");

        MeasureExtents();
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
    {
        // AudioStreamPlayer2D p;
        // p.Stream
    }

    public void Stomp()
    {
        StompSoundIndex++;
        StompSoundIndex %= StompSounds.Length;
        StompSoundPlayer.Stream = StompSounds[StompSoundIndex];
        StompSoundPlayer.Play();

        // todo - only do when an alien dies
        MeasureExtents();
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
                Vector2 pos = alien.GlobalPosition;
                left = Math.Min(left, pos.X + extents.Position.X);
                top = Math.Min(top, pos.Y + extents.Position.Y);
                right = Math.Max(right, pos.X + extents.End.X);
                bottom = Math.Max(bottom, pos.Y + extents.End.Y);
            }
        }
        var swarmExtents = new Rect2(left, top, right - left, bottom - top);
        // var swarmExtentsLocal = new Rect2(left - GlobalPosition.X, top - GlobalPosition.Y, right - GlobalPosition.X, bottom - GlobalPosition.Y);
        GD.Print("Swarm extents: " + swarmExtents);
        GetNode<Sprite2D>("ExtentsMarker1").GlobalPosition = swarmExtents.Position;
        GetNode<Sprite2D>("ExtentsMarker2").GlobalPosition = swarmExtents.End;
    }
}
