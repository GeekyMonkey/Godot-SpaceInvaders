using System;
using Godot;

public partial class CustomSignals : Node
{
    [Signal]
    public delegate void AlienDiedEventHandler(AlienPrefab alien);
    public void EmitAlienDied(AlienPrefab alien)
    {
        this.EmitSignal(SignalName.AlienDied, alien);
    }

    [Signal]
    public delegate void StompEventHandler();
    public void EmitStomp()
    {
        this.EmitSignal(SignalName.Stomp);
    }

    [Signal]
    public delegate void ScoreChangedEventHandler(int score);
    public void EmitScoreChanged(int score)
    {
        this.EmitSignal(SignalName.ScoreChanged, score);
    }

    [Signal]
    public delegate void LivesChangedEventHandler(int score);
    public void EmitLivesChanged(int lives)
    {
        this.EmitSignal(SignalName.LivesChanged, lives);
    }


    [Signal]
    public delegate void SwarmDeathEventHandler();
    public void EmitSwarmDeath()
    {
        this.EmitSignal(SignalName.SwarmDeath);
    }
}
