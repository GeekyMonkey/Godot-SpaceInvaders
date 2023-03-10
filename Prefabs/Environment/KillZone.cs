using System;
using Godot;

public partial class KillZone : Area2D
{
    private void OnAreaEntered(Node otherObject)
    {
        GD.Print("Killzone removed " + otherObject.Name + " from group " + otherObject.GetGroups());
        otherObject.QueueFree();
    }
}
