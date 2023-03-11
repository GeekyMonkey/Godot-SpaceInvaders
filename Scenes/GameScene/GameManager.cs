using Godot;

/// <summary>
/// Game manager
/// </summary>
public partial class GameManager : Node2D
{
    // Public State
    public int Level = 1;
    public int Score = 0;
    public int PlayerLives = 3;

    // Private State
    private CustomSignals cs;

    /// <summary>
    /// Called when the node enters the scene tree for the first time.
    /// </summary>
    public override void _Ready()
    {
        cs = this.GetCustomSignals();

        // Listen for alien deaths
        cs.Connect(CustomSignals.SignalName.AlienDied, Callable.From((Alien alien) => OnAlienDied(alien)));

        // Listen for swarm deaths
        cs.Connect(CustomSignals.SignalName.SwarmDeath, Callable.From(async () =>
        {
            // Add a bunch of points
            await this.DelaySec(0.5);
            ScoreAdd(201);
            await this.DelaySec(2.5);

            // Level up
            SpawnNewSwarm();
        }));
    }

    /// <summary>
    /// An alien is pining for the fjords
    /// </summary>
    private void OnAlienDied(Alien alien)
    {
        int points = alien.Points;
        ScoreAdd(points);
    }

    /// <summary>
    /// Add some points
    /// </summary>
    public void ScoreAdd(int points)
    {
        Score += points;

        // Alert the text box
        cs.EmitScoreChanged(Score);
    }

    /// <summary>
    /// Spawn a new swarm
    /// </summary>
    private void SpawnNewSwarm()
    {
        Level++;
        this.SpawnPrefab<Swarm>((swarm) =>
        {
            swarm.SwarmType = Level - 1;
        });
    }

    /// <summary>
    /// The player has lost a life
    /// </summary>
    public async void PlayerDied()
    {
        PlayerLives--;
        cs.EmitLivesChanged(PlayerLives);

        if (PlayerLives > 0)
        {
            await this.DelaySec(2);
            SpawnPlayer();
        }
    }

    /// <summary>
    /// Spawn a player
    /// </summary>
    public void SpawnPlayer()
    {
        Marker2D spawnMarker = GetNode<Marker2D>("PlayerSpawnPoint");
        this.SpawnPrefab<Player>((player) =>
        {
            player.GlobalPosition = spawnMarker.GlobalPosition;
        });
    }
}
