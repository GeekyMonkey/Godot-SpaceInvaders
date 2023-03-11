using Godot;
using System;


public partial class Swarm : Node2D
{
    // Editor State
    [ExportCategory("Sounds")]
    [Export] AudioStream[] StompSounds;

    [ExportCategory("Swarm Geometry")]
    [Export] public float SpacingX = 16f;
    [Export] public float SpacingY = 16f;
    [Export] private float XMargin = 8f;

    [ExportCategory("Swarm Movement")]
    [Export] public int StepX = 8;
    [Export] public int StepY = 8;

    // Public State
    public int SwarmType = 0;

    // Private State
    private int DirectionX = 1; // 1 or -1
    private bool IsStomping = true;
    private float ScreenSizeX;
    private int StompSoundIndex = 0;
    private AudioStreamPlayer2D StompSoundPlayer;
    private Rect2 SwarmExtents;
    private float XMin;
    private float XMax;

    private string[][] SwarmPatterns = new string[][] {
        new String[] {
       "23332",
       " 222 ",
       " 121 ",
       "  1  "
        },
        new String[] {
       "33333333333",
       "22222222222",
       "22222222222",
       "11111111111",
       "11111111111"
        }
    };


    /// <summary>
    /// Called when the node enters the scene tree for the first time.
    /// </summary>
    public override void _Ready()
    {
        StompSoundPlayer = GetNode<AudioStreamPlayer2D>("./StompSoundPlayer");

        // It's stompin' time!
        this.GetCustomSignals().Connect(CustomSignals.SignalName.Stomp, Callable.From(Stomp));

        // When an alien dies, re-check the swarm size
        this.GetCustomSignals().Connect(CustomSignals.SignalName.AlienDied, Callable.From((Alien alien) =>
            MeasureExtents()
        ));

        // The player is out of lives - stop stomping
        this.GetCustomSignals().LivesChanged += (int lives) =>
        {
            if (lives == 0)
            {
                IsStomping = false;
            }
        };

        // Measure the screen
        ScreenSizeX = GetViewportRect().Size.X / 3;
        XMin = ScreenSizeX / -2 + XMargin;
        XMax = ScreenSizeX / 2 - XMargin;

        // Create the swarm
        CreateSwarm(SwarmType);
    }

    /// <summary>
    /// Timer says it's stomping time. Alert the aliens!
    /// </summary>
    public void StompTimer()
    {
        this.GetCustomSignals().EmitStomp();
    }

    /// <summary>
    /// Move the swarm and make the sound
    /// </summary>
    public void Stomp()
    {
        if (IsStomping)
        {
            // Play a sound
            StompSoundIndex++;
            StompSoundIndex %= StompSounds.Length;
            StompSoundPlayer.Stream = StompSounds[StompSoundIndex];
            StompSoundPlayer.Play();

            // Move the swarm
            float dX = DirectionX * StepX;
            Position += new Vector2(dX, 0);
            if ((Position.X + SwarmExtents.Position.X) <= XMin || (Position.X + SwarmExtents.End.X) >= XMax)
            {
                DirectionX *= -1;

                Position += new Vector2(-dX, StepY);
            }
        }
    }

    /// <summary>
    /// Measure the size of the swarm so we can tell if a side is hit
    /// </summary>
    public async void MeasureExtents()
    {
        await this.NextIdle();
        var children = this.GetChildren(false);
        float left = float.MaxValue;
        float top = float.MaxValue;
        float bottom = float.MinValue;
        float right = float.MinValue;
        int alienCount = 0;
        foreach (var child in children)
        {
            if (child is Alien alien)
            {
                if (!alien.Dead)
                {
                    alienCount++;
                    Rect2 extents = alien.Extents;
                    Vector2 pos = alien.Position;
                    left = Math.Min(left, pos.X + extents.Position.X);
                    top = Math.Min(top, pos.Y + extents.Position.Y);
                    right = Math.Max(right, pos.X + extents.End.X);
                    bottom = Math.Max(bottom, pos.Y + extents.End.Y);
                }
            }
        }

        if (alienCount == 0)
        {
            QueueFree();
            this.GetCustomSignals().EmitSwarmDeath();
        }
        else
        {
            SwarmExtents = new Rect2(left, top, right - left, bottom - top);
            // GD.Print("Swarm of " + alienCount + " extents: " + SwarmExtents);
            // GetNode<Sprite2D>("ExtentsMarker1").Position = SwarmExtents.Position;
            // GetNode<Sprite2D>("ExtentsMarker2").Position = SwarmExtents.End;
        }
    }

    /// <summary>
    /// Create a swarm of the specified type
    /// </summary>
    /// <param name="swarmType">Number of the swarm pattern</param>
    private void CreateSwarm(int swarmType)
    {
        GD.Print("Creating swarm type " + swarmType);
        ClearSwarm();
        var swarmPattern = SwarmPatterns[swarmType % SwarmPatterns.Length];
        int rows = swarmPattern.Length;
        int columns = 0;
        foreach (string row in swarmPattern)
        {
            columns = Math.Max(columns, row.Length);
        }
        float left = (columns / 2f) * -SpacingX;
        float top = (rows / 2f) * -SpacingY;
        for (int row = 0; row < rows; row++)
        {
            for (int col = 0; col < columns; col++)
            {
                int alienTypeIndex = swarmPattern[row][col] - '0';
                if (alienTypeIndex >= 1 && alienTypeIndex <= 3)
                {
                    float x = left + col * SpacingX;
                    float y = top + row * SpacingY;
                    GD.Print($"Spawn alien {alienTypeIndex} at {x},{y}");
                    this.SpawnPrefab<Alien>((alien) =>
                    {
                        alien.Position = new Vector2(x, y);
                    }, variantName: $"Alien_{alienTypeIndex}");
                }
            }
        }
        MeasureExtents();
    }

    /// <summary>
    /// Remove any aliens from the swarm
    /// </summary>
    private void ClearSwarm()
    {
        var children = this.GetChildren(false);
        foreach (var child in children)
        {
            if (child is Alien alien)
            {
                alien.QueueFree();
            }
        }
    }
}
