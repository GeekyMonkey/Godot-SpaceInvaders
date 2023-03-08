using Godot;
using System;
using System.Threading.Tasks;

public partial class AlienPrefab : RigidBody2D
{
    private CustomSignals cs;
    private bool Dead = false;

    private AnimatedSprite2D AnimatedSprite;

    public float Width = 0f;
    public float Height = 0f;
    public float SpriteScale = 1;

    public Rect2 Extents;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        cs = this.GetCustomSignals();
        cs.Connect("Stomp", Callable.From(() => Stomp()));

        AnimatedSprite = GetNode<AnimatedSprite2D>("AnimatedSprite");

        var collisionShape = GetNode<CollisionShape2D>("CollisionShape2D");
        SpriteScale = collisionShape.Transform.Scale.X;
        var collisionRect = collisionShape.Shape.GetRect();
        Extents = new Rect2(collisionRect.Position.X * SpriteScale, collisionRect.Position.Y * SpriteScale, collisionRect.Size.X * SpriteScale, collisionRect.Size.Y * SpriteScale);
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
            cs.EmitAlienDied(this);

            await this.NextIdle();
            this.QueueFree();
        }
    }

    public void Stomp()
    {
        AnimatedSprite.Frame = AnimatedSprite.Frame == 0 ? 1 : 0;
    }
}
