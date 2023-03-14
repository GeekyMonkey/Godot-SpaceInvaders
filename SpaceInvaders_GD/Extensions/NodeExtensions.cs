using Godot;
using System.Threading.Tasks;

/// <summary>
/// Extensions to all node derived types
/// </summary>
public static class NodeExtensions
{
    /// <summary>
    /// Await until the next frame draw
    /// </summary>
    public static async Task NextIdle(this Node node)
    {
        await node.GetTree().NextIdle();
    }

    /// <summary>
    /// Get the custom signals
    /// </summary>
    public static CustomSignals GetCustomSignals(this Node node)
    {
        return node.GetNode<CustomSignals>("/root/CS");
    }

    /// <summary>
    /// Get the game manager
    /// </summary>
    public static GameManager GetGameManager(this Node node)
    {
        return node.GetNode<GameManager>("/root/GameManager");
    }

    /// <summary>
    /// Wait for the given number of milliseconds
    /// </summary>
    public static async Task DelayMs(this Node node, int milliseconds)
    {
        await System.Threading.Tasks.Task.Delay(milliseconds);
    }

    /// <summary>
    /// Wait for the given number of seconds
    /// </summary>
    public static async Task DelaySec(this Node node, double seconds)
    {
        await System.Threading.Tasks.Task.Delay((int)(seconds * 1000));
    }

    /// <summary>
    /// Get the root node of the current scene
    /// </summary>
    /// <returns>Scene root node</returns>
    public static Node Root(this Node node)
    {
        return node.GetTree().CurrentScene;
    }

    /// <summary>
    /// Used to initialize your new prefab object before it is added to the scene
    /// </summary>
    /// <typeparam name="T">Prefab's main script class</typeparam>
    /// <param name="newObject">The newly created prefab instance</param>
    public delegate void PrefabInitCallback<T>(T newObject);

    /// <summary>
    /// Spawn a prefab instance as the child of the scene root
    /// </summary>
    /// <typeparam name="T">Type of the prefab's main script class</typeparam>
    /// <param name="init">A callback to initialize your new prefab instance before it is plonked into the scene</param>
    /// <param name="variantName">If the prefab folder has different prefab variants, specify which one here. This must be the folder name, then an underscore, then a variant name.</param>
    /// <returns>New prefab instance</returns>
    public static T SpawnPrefabAtRoot<T>(this Node node, PrefabInitCallback<T> init = null, string variantName = null) where T : Node
    {
        return node.Root().SpawnPrefabChild(init, variantName);
    }

    /// <summary>
    /// Spawn a prefab instance as the child of this node
    /// </summary>
    /// <typeparam name="T">Type of the prefab's main script class</typeparam>
    /// <param name="init">A callback to initialize your new prefab instance before it is plonked into the scene</param>
    /// <param name="variantName">If the prefab folder has different prefab variants, specify which one here. This must be the folder name, then an underscore, then a variant name.</param>
    /// <returns>New prefab instance</returns>
    public static T SpawnPrefabChild<T>(this Node node, PrefabInitCallback<T> init = null, string variantName = null) where T : Node
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

        // Init callback and add now
        if (init != null)
        {
            init(obj);

            // Add the child now since we know it's initialized
            node.AddChild(obj);
        }
        else
        {
            // Add a child on the next frame. Gives time to initialize before adding
            node.AddChildDeferred(obj);
        }

        // Add the child
        return obj;
    }

    /// <summary>
    /// Add a child on the next frame. Gives time to initialize before adding
    /// </summary>
    public static void AddChildDeferred(this Node node, Node child)
    {
        node.CallDeferred("add_child", child);
    }

}
