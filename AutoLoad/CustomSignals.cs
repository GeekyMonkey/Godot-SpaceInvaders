using Godot;

/// <summary>
/// Custom Signals
/// </summary>
public partial class CustomSignals : Node
{
    /// <summary>
    /// Alien Died
    /// </summary>
    [Signal]
    public delegate void AlienDiedEventHandler(Alien alien);
    public void EmitAlienDied(Alien alien)
    {
        this.EmitSignal(SignalName.AlienDied, alien);
    }

    /// <summary>
    /// Stompin' Time!
    /// </summary>
    [Signal]
    public delegate void StompEventHandler();
    public void EmitStomp()
    {
        this.EmitSignal(SignalName.Stomp);
    }

    /// <summary>
    /// The score has changed
    /// </summary>
    /// <param name="score">The new score</param>
    [Signal]
    public delegate void ScoreChangedEventHandler(int score);
    public void EmitScoreChanged(int score)
    {
        this.EmitSignal(SignalName.ScoreChanged, score);
    }

    /// <summary>
    /// The player lives count has changed
    /// </summary>
    /// <param name="lives">New player lives count</param>
    [Signal]
    public delegate void LivesChangedEventHandler(int lives);
    public void EmitLivesChanged(int lives)
    {
        this.EmitSignal(SignalName.LivesChanged, lives);
    }

    /// <summary>
    /// The swarm has been vanquished!
    /// </summary>
    [Signal]
    public delegate void SwarmDeathEventHandler();
    public void EmitSwarmDeath()
    {
        this.EmitSignal(SignalName.SwarmDeath);
    }
}
