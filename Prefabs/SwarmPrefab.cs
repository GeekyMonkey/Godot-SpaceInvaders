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
    }
}
