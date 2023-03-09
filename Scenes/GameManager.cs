using Godot;
using System;

public partial class GameManager : Node2D
{
    private CustomSignals cs;

    public int Score = 0;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        cs = this.GetCustomSignals();
        cs.AlienDied += OnAlienDied;

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
}
