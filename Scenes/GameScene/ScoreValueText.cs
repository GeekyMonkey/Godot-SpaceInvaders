using Godot;

public partial class ScoreValueText : RichTextLabel
{
    int scoreDisplayed = 0;
    int scoreCurrent = 0;

    double changeDelay = 0.01f;
    double changeTime = 0;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        this.GetCustomSignals().ScoreChanged += ScoreChanged;
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
    {
        if (scoreCurrent > scoreDisplayed)
        {
            changeTime += delta;
            if (changeTime > changeDelay)
            {
                scoreDisplayed++;
                this.Text = scoreDisplayed.ToString();
                changeTime = 0;
            }
        }
    }

    private void ScoreChanged(int score)
    {
        scoreCurrent = score;
    }
}
