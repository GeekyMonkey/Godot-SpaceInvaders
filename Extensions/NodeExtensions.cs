using System;
using System.ComponentModel;
using System.IO;
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

    public static Node Root(this Node node)
    {
        return node.GetTree().CurrentScene;
    }

    public readonly struct SpawnOptions<T> where T : Node
    {
        readonly string Path;
        readonly PrefabInitCallback<T> Init;
    }

    public delegate void PrefabInitCallback<T>(T newObject);


    public static T SpawnPrefab<T>(this Node node, PrefabInitCallback<T> init = null, string variantName = null) where T : Node
    {
        var typeName = typeof(T).Name;
        string prefabFolder;

        if (typeName.IndexOf("_") > -1)
        {
            prefabFolder = typeName.Split("_")[0];
        }
        else
        {
            prefabFolder = typeName;
        }

        string path = $"{prefabFolder}/{variantName ?? typeName}";
        GD.Print("GameManager Spawning " + typeName);
        var prefab = GD.Load<PackedScene>($"res://Prefabs/{path}.tscn");
        T obj = prefab.Instantiate<T>(PackedScene.GenEditState.Instance);

        // Init callback
        if (init != null)
        {
            init(obj);
        }

        // Add the child
        node.AddChild(obj);
        return obj;
    }

}
