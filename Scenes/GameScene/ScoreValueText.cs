using Godot;

/// <summary>
/// Score Value Text
/// </summary>
public partial class ScoreValueText : RichTextLabel
{
    // Editor State
    [Export] double changeDelay = 0.01f;

    // Private state
    private double changeTime = 0;
    private int scoreDisplayed = 0;
    private int scoreCurrent = 0;

    /// <summary>
    /// Called when the node enters the scene tree for the first time.
    /// </summary>
    public override void _Ready()
    {
        // Respond to the score changed event
        this.GetCustomSignals().ScoreChanged += (newScore) => scoreCurrent = newScore;
    }

    /// <summary>
    /// Called every frame. 'delta' is the elapsed time since the previous frame.
    /// </summary>
    public override void _Process(double delta)
    {
        // If the score displayed is behind the actual score - increment by one, but slowly
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
}
