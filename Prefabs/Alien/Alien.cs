using Godot;

/// <summary>
/// Alien Prefab
/// </summary>
public partial class Alien : GmRigidBody2D
{
    // Editor State
    [Export] public int BombTypes = 2;
    [Export] public int Points = 10;
    [Export] public float ReloadSecMin = 1f;
    [Export] public float ReloadSecMax = 15f;

    // Public State
    public bool Dead = false;

    // Private State
    private AnimatedSprite2D AnimatedSprite;
    private Marker2D GunPosition;
    private PointLight2D GunSprite;
    public Rect2 Extents;
    private float Width = 0f;
    private float Height = 0f;
    private bool PlayerIsDead = false;
    private float ReloadTime = 1f;
    private float SpriteScale = 1;
    private bool ViewIsClear = false;

    /// <summary>
    /// Alien added to the scene
    /// </summary>
    public override async void _Ready()
    {
        AnimatedSprite = GetNode<AnimatedSprite2D>("AnimatedSprite");
        GunPosition = GetNode<Marker2D>("GunPosition");
        GunSprite = GetNode<PointLight2D>("GunSprite");

        // Watch for swarm stomp events
        this.GetCustomSignals().Connect("Stomp", Callable.From(() => Stomp()));

        // Watch for alien died event
        this.GetCustomSignals().Connect(CustomSignals.SignalName.AlienDied, Callable.From((Alien alien) => OnAlienDied(alien)));

        // Watch for player lives changed to stop stomping when he's dead
        this.GetCustomSignals().LivesChanged += (int lives) =>
        {
            if (lives == 0)
            {
                PlayerIsDead = true;
                CheckView();
            }
        };

        // Measure the size of the alien so we can get the size of the swarm
        var collisionShape = GetNode<CollisionShape2D>("CollisionShape2D");
        SpriteScale = collisionShape.Transform.Scale.X;
        var collisionRect = collisionShape.Shape.GetRect();
        Extents = new Rect2(collisionRect.Position.X * SpriteScale, collisionRect.Position.Y * SpriteScale, collisionRect.Size.X * SpriteScale, collisionRect.Size.Y * SpriteScale);

        // Initial check if it's safe to shoot
        await DelaySec(1);
        CheckView();
    }

    /// <summary>
    /// Begin reloading
    /// </summary>
    private void Reload()
    {
        ReloadTime = new RandomNumberGenerator().RandfRange(ReloadSecMin, ReloadSecMax);
    }

    /// <summary>
    /// An alien has perished. Perhaps now we have a clear shot at the player.
    /// </summary>
    private async void OnAlienDied(Alien alien)
    {
        if (alien != this)
        {
            await DelaySec(0.5);
            CheckView();
        }
    }

    /// <summary>
    /// Called every frame. 'delta' is the elapsed time since the previous frame.
    /// </summary>
    public override void _Process(double delta)
    {
        if (ViewIsClear && ReloadTime > 0)
        {
            ReloadTime -= (float)delta;
            if (ReloadTime < 0 && !PlayerIsDead)
            {
                Shoot();
                Reload();
            }
        }
    }

    /// <summary>
    /// Drop a bomb
    /// </summary>
    private void Shoot()
    {
        int bombTypeIndex = new RandomNumberGenerator().RandiRange(1, BombTypes);
        var bomb = SpawnPrefabAtRoot<Bomb>((b) =>
        {
            b.Position = GunPosition.GlobalPosition;
        }, $"Bomb_{bombTypeIndex}");
    }

    /// <summary>
    /// Alien has collided with something
    /// </summary>
    private void OnBodyEntered(Node otherObject)
    {
        // GD.Print(Name + " hit by " + otherObject.Name + " in group " + otherObject.GetGroups());
        if (otherObject is Bullet)
        {
            HitByBullet(otherObject);
        }
    }

    /// <summary>
    /// A bullet has hit this alien
    /// </summary>
    private void HitByBullet(Node missile)
    {
        if (!Dead)
        {
            Dead = true;

            // Spread the word
            this.GetCustomSignals().EmitAlienDied(this);

            // Show an explosion
            SpawnPrefabSibling<Alien_Explosion>((e) =>
            {
                e.Position = Position;
            });

            // Remove alien from the scene
            QueueFree();
        }
    }

    /// <summary>
    /// We're all stompin'. Do a little dance.
    /// </summary>
    public void Stomp()
    {
        AnimatedSprite.Frame = AnimatedSprite.Frame == 0 ? 1 : 0;
    }

    /// <summary>
    /// Make sure there are no aliens under this alien's gun
    /// </summary>
    public void CheckView()
    {
        if (IsInstanceValid(this))
        {
            var spaceState = GetWorld2D().DirectSpaceState;
            var query = PhysicsRayQueryParameters2D.Create(GlobalPosition, GlobalPosition + new Vector2(0, 1000), 2);
            var result = spaceState.IntersectRay(query);
            bool newViewIsClear = result.Count == 0;

            // The view has just become clear - Begin reloading
            if (newViewIsClear == true && ViewIsClear == false)
            {
                Reload();
            }

            ViewIsClear = newViewIsClear;
            GunSprite.Enabled = ViewIsClear && !PlayerIsDead;
        }
    }
}
