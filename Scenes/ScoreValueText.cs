using Godot;
using System;

public partial class ScoreValueText : RichTextLabel
{
    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        this.GetCustomSignals().ScoreChanged += ScoreChanged;
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
    {
    }

    private void ScoreChanged(int score)
    {
        this.Text = score.ToString();
    }
}
