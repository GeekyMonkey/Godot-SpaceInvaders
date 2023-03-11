using Godot;
using System.Threading.Tasks;

/// <summary>
/// Scene Tree Extensions
/// </summary>
public static class SceneTreeExtensions
{
    /// <summary>
    /// Wait for the next frame
    /// </summary>
    public static async Task NextIdle(this SceneTree sceneTree)
    {
        await sceneTree.ToSignal(sceneTree, SceneTree.SignalName.ProcessFrame);
    }

}
