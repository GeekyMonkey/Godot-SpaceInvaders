using System;
using System.Threading.Tasks;
using Godot;

public static class NodeExtensions
{
    /// <summary>
    /// Await until the next frame draw
    /// </summary>
    public static async Task NextIdle(this Node node)
    {
        await node.GetTree().NextIdle();
    }

    public static CustomSignals GetCustomSignals(this Node node)
    {
        return node.GetNode<CustomSignals>("/root/CS");
    }

    public static GameManager GetGameManager(this Node node)
    {
        return node.GetNode<GameManager>("/root/GameManager");
    }

    public static async Task DelayMs(this Node node, int milliseconds)
    {
        await System.Threading.Tasks.Task.Delay(milliseconds);
    }
}
