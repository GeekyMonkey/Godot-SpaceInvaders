using System;
using Godot;

public partial class LivesValueText : RichTextLabel
{

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        this.GetCustomSignals().LivesChanged += LivesChanged;
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
    {
    }

    private void LivesChanged(int lives)
    {
        this.Text = lives.ToString();
    }
}
