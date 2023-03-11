using Godot;
using System;


public partial class Swarm : Node2D
{
    [Export]
    AudioStream[] StompSounds;

    AudioStreamPlayer2D StompSoundPlayer;

    private int StompSoundIndex = 0;
    private CustomSignals cs;

    public float ScreenSizeX;
    public float XMin;
    public float XMax;

    [Export]
    private float XMargin = 8f;

    private Rect2 SwarmExtents;

    private bool IsStomping = true;

    [Export]
    public int StepX = 8;

    [Export]
    public int StepY = 8;

    public int DirectionX = 1; // 1 or -1

    [Export]
    public int SwarmType = 0;

    public string[][] SwarmPatterns = new string[][] {
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

    [Export]
    public float SpacingX = 16f;

    [Export]
    public float SpacingY = 16f;


    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        StompSoundPlayer = GetNode<AudioStreamPlayer2D>("./StompSoundPlayer");

        cs = this.GetCustomSignals();
        cs.Connect(CustomSignals.SignalName.Stomp, Callable.From(Stomp));
        cs.Connect(CustomSignals.SignalName.AlienDied, Callable.From((Alien alien) =>
            MeasureExtents()
        ));

        this.GetCustomSignals().LivesChanged += (int lives) =>
        {
            if (lives == 0)
            {
                IsStomping = false;
            }
        };

        ScreenSizeX = GetViewportRect().Size.X / 3;
        XMin = ScreenSizeX / -2 + XMargin;
        XMax = ScreenSizeX / 2 - XMargin;

        GD.Print("Creating swarm type " + SwarmType);
        CreateSwarm(SwarmType);
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
    {
        // AudioStreamPlayer2D p;
        // p.Stream
    }

    public void StompTimer()
    {
        cs.EmitStomp();
    }

    public void Stomp()
    {
        if (IsStomping)
        {
            StompSoundIndex++;
            StompSoundIndex %= StompSounds.Length;
            StompSoundPlayer.Stream = StompSounds[StompSoundIndex];
            StompSoundPlayer.Play();

            float dX = DirectionX * StepX;
            Position += new Vector2(dX, 0);
            if ((Position.X + SwarmExtents.Position.X) <= XMin || (Position.X + SwarmExtents.End.X) >= XMax)
            {
                DirectionX *= -1;

                Position += new Vector2(-dX, StepY);
            }
        }
    }

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
            // var swarmExtentsLocal = new Rect2(left - GlobalPosition.X, top - GlobalPosition.Y, right - GlobalPosition.X, bottom - GlobalPosition.Y);
            GD.Print("Swarm of " + alienCount + " extents: " + SwarmExtents);
            GetNode<Sprite2D>("ExtentsMarker1").Position = SwarmExtents.Position;
            GetNode<Sprite2D>("ExtentsMarker2").Position = SwarmExtents.End;
        }
    }

    /// <summary>
    /// Create a swarm of the specified type
    /// </summary>
    /// <param name="swarmType">Number of the swarm pattern</param>
    private void CreateSwarm(int swarmType)
    {
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

    private void ClearSwarm()
    {
        var children = this.GetChildren(false);
        foreach (var child in children)
        {
            //GD.Print(child.Name);
            if (child is Alien alien)
            {
                alien.QueueFree();
            }
        }

    }
}
