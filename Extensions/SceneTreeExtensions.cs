using Godot;
using System.Threading.Tasks;

public static class SceneTreeExtensions
{
    public static async Task NextIdle(this SceneTree sceneTree)
    {
        await sceneTree.ToSignal(sceneTree, SceneTree.SignalName.ProcessFrame);
    }

}
