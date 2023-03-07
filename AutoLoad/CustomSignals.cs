using Godot;
using System;

public partial class CustomSignals : Node
{
    [Signal]
    public delegate void AlienDiedEventHandler(Node alien);
    public void EmitAlienDied(Node alien)
    {
        this.EmitSignal("AlienDied", alien);
    }

    [Signal]
    public delegate void StompEventHandler();
    public void EmitStomp()
    {
        this.EmitSignal("Stomp");
    }
}
