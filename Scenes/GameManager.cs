using Godot;
using System;

public partial class GameManager : Node2D
{
    [Export]
    public PackedScene SwarmPrefab;

    private CustomSignals cs;

    public int Score = 0;
    public int Level = 1;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        cs = this.GetCustomSignals();
        cs.Connect(CustomSignals.SignalName.AlienDied, Callable.From((AlienPrefab alien) => OnAlienDied(alien)));

        cs.Connect(CustomSignals.SignalName.SwarmDeath, Callable.From(() =>
        {
            SpawnNewSwarm();
        }));

    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
    {
    }

    private void OnAlienDied(AlienPrefab alien)
    {
        int points = alien.Points;
        ScoreAdd(points);
    }

    private void ScoreAdd(int points)
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
        var swarm = SwarmPrefab.Instantiate<SwarmPrefab>(PackedScene.GenEditState.Instance);
        swarm.SwarmType = Level - 1;
        GetTree().CurrentScene.AddChild(swarm);
    }
}
