using Godot;

/// <summary>
/// Kill Zone
/// </summary>
public partial class KillZone : Area2D
{
    /// <summary>
    /// Something has dared enter the dreaded kill zone
    /// </summary>
    /// <param name="otherObject">The thing meeting it's demise</param>
    private void OnAreaEntered(Node otherObject)
    {
        //GD.Print("Kill zone removed " + otherObject.Name + " from group " + otherObject.GetGroups());
        otherObject.QueueFree();
    }
}
