using Godot;

/// <summary>
/// Lives value text
/// </summary>
public partial class LivesValueText : RichTextLabel
{
    /// <summary>
    /// Called when the node enters the scene tree for the first time.
    /// </summary>
    public override void _Ready()
    {
        // Received a LivesChanged event
        this.GetCustomSignals().LivesChanged += (int lives) =>
            {
                this.Text = lives.ToString();
            };
    }
}
