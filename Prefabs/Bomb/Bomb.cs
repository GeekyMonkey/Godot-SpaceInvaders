using Godot;

public partial class Bomb : RigidBody2D
{
    [Export]
    public float Speed = 50f;

    public override void _Ready()
    {
        LinearVelocity = Vector2.Up * -Speed;
        Connect(SignalName.BodyEntered, Callable.From((Node o) => OnBodyEntered(o)));
    }

    private void OnBodyEntered(Node other)
    {
        GD.Print("Bomb " + this.Name + " hit " + other.Name);
        this.Root().SpawnPrefab<Bomb_Explosion>((bombExp) =>
        {
            bombExp.GlobalPosition = GlobalPosition;
        }, "Bomb_Explosion");
    }
}
