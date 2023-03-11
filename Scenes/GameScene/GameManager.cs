using Godot;

public partial class GameManager : Node2D
{
    private CustomSignals cs;

    public int Score = 0;
    public int Level = 1;

    public int PlayerLives = 3;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        cs = this.GetCustomSignals();
        cs.Connect(CustomSignals.SignalName.AlienDied, Callable.From((Alien alien) => OnAlienDied(alien)));

        cs.Connect(CustomSignals.SignalName.SwarmDeath, Callable.From(() =>
        {
            SpawnNewSwarm();
        }));

    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
    {
    }

    private void OnAlienDied(Alien alien)
    {
        int points = alien.Points;
        ScoreAdd(points);
    }

    public void ScoreAdd(int points)
    {
        Score += points;
        cs.EmitScoreChanged(Score);
    }


    private async void SpawnNewSwarm()
    {
        await this.DelayMs(500);
        ScoreAdd(201);
        await this.DelayMs(2500);

        Level++;
        this.SpawnPrefab<Swarm>((swarm) =>
        {
            swarm.SwarmType = Level - 1;
        });
    }

    public async void PlayerDied()
    {
        PlayerLives--;
        cs.EmitLivesChanged(PlayerLives);

        if (PlayerLives > 0)
        {
            await this.DelayMs(2000);
            SpawnPlayer();
        }
    }

    public void SpawnPlayer()
    {
        Marker2D spawnMarker = GetNode<Marker2D>("PlayerSpawnPoint");
        this.SpawnPrefab<Player>((player) =>
        {
            player.GlobalPosition = spawnMarker.GlobalPosition;
        });


    }
}
