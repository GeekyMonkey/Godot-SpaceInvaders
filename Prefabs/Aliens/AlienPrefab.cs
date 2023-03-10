using System;
using System.Linq;
using System.Threading.Tasks;
using Godot;

public partial class AlienPrefab : RigidBody2D
{
    private CustomSignals cs;
    public bool Dead = false;
    public bool ViewIsClear = false;

    [Export]
    public int Points = 10;

    private AnimatedSprite2D AnimatedSprite;

    public float Width = 0f;
    public float Height = 0f;
    public float SpriteScale = 1;

    public Rect2 Extents;

    [Export]
    public PackedScene ExplosionPrefab;

    // Called when the node enters the scene tree for the first time.
    public override async void _Ready()
    {
        cs = this.GetCustomSignals();
        cs.Connect("Stomp", Callable.From(() => Stomp()));
        cs.Connect(CustomSignals.SignalName.AlienDied, Callable.From((AlienPrefab alien) => OnAlienDied(alien)));
        // this.GetCustomSignals().OnStomp(Stomp);

        AnimatedSprite = GetNode<AnimatedSprite2D>("AnimatedSprite");

        var collisionShape = GetNode<CollisionShape2D>("CollisionShape2D");
        SpriteScale = collisionShape.Transform.Scale.X;
        var collisionRect = collisionShape.Shape.GetRect();
        Extents = new Rect2(collisionRect.Position.X * SpriteScale, collisionRect.Position.Y * SpriteScale, collisionRect.Size.X * SpriteScale, collisionRect.Size.Y * SpriteScale);

        await this.DelayMs(1000);
        CheckView();
    }


    private async void OnAlienDied(AlienPrefab alien)
    {
        if (alien != this)
        {
            await this.DelayMs(500);
            CheckView();
        }
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
    {
    }

    private void OnBodyEntered(Node otherObject)
    {
        // GD.Print(this.Name + " hit by " + otherObject.Name + " in group " + otherObject.GetGroups());
        if (otherObject.IsInGroup("Missiles"))
        {
            HitByMissile(otherObject);
        }
    }

    private async void HitByMissile(Node missile)
    {
        if (!Dead)
        {
            Dead = true;
            // GetTree().CallGroup("Players", nameof(PlayerMove.MethodName.OnAlienDied), this);
            this.GetCustomSignals().EmitAlienDied(this);

            await this.NextIdle();
            this.QueueFree();

            Node2D explosion = ExplosionPrefab.Instantiate<Node2D>(PackedScene.GenEditState.Instance);
            explosion.Position = Position;
            GetParent().AddChild(explosion);
            // GetTree().CurrentScene.AddChild(explosion);
        }
    }

    public void Stomp()
    {
        AnimatedSprite.Frame = AnimatedSprite.Frame == 0 ? 1 : 0;
    }

    // Make sure there are no aliens under this alien's gun
    public void CheckView()
    {
        var spaceState = GetWorld2D().DirectSpaceState;
        var query = PhysicsRayQueryParameters2D.Create(GlobalPosition, GlobalPosition + new Vector2(0, 1000), 2);
        var result = spaceState.IntersectRay(query);
        ViewIsClear = result.Count == 0;

        this.Modulate = new Color(1, 1, ViewIsClear ? 0.5f : 1, 1);
        // this.Scale = Vector2.One * (ViewIsClear ? 1.5f : 1f);
    }
}
