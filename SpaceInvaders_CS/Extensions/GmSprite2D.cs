using Godot;
using System.Threading.Tasks;
using static NodeExtensions;

/// <summary>
/// GeekyMonkey Sprite2D base class
/// </summary>
public partial class GmSprite2D : Sprite2D
{
    /// <summary>
    /// Wait for the given number of seconds
    /// </summary>
    public async Task DelaySec(double seconds)
    {
        await NodeExtensions.DelaySec(this, seconds);
    }

    /// <summary>
    /// Get the game manager
    /// </summary>
    public GameManager GameManager { get { return GetNode<GameManager>("/root/GameManager"); } }

    /// <summary>
    /// Await until the next frame draw
    /// </summary>
    public async Task NextIdle()
    {
        await NodeExtensions.NextIdle(this);
    }

    /// <summary>
    /// Get the root node of the current scene
    /// </summary>
    public Node Root { get { return NodeExtensions.Root(this); } }

    /// <summary>
    /// Spawn a prefab instance as the child of this node
    /// </summary>
    /// <typeparam name="T">Type of the prefab's main script class</typeparam>
    /// <param name="init">A callback to initialize your new prefab instance before it is plonked into the scene</param>
    /// <param name="variantName">If the prefab folder has different prefab variants, specify which one here. This must be the folder name, then an underscore, then a variant name.</param>
    /// <returns>New prefab instance</returns>
    public T SpawnPrefabChild<T>(PrefabInitCallback<T> init = null, string variantName = null) where T : Node
    {
        return NodeExtensions.SpawnPrefabChild<T>(this, init, variantName);
    }

    /// <summary>
    /// Spawn a prefab instance as a sibling of this node
    /// </summary>
    /// <typeparam name="T">Type of the prefab's main script class</typeparam>
    /// <param name="init">A callback to initialize your new prefab instance before it is plonked into the scene</param>
    /// <param name="variantName">If the prefab folder has different prefab variants, specify which one here. This must be the folder name, then an underscore, then a variant name.</param>
    /// <returns>New prefab instance</returns>
    public T SpawnPrefabSibling<T>(PrefabInitCallback<T> init = null, string variantName = null) where T : Node
    {
        return GetParent().SpawnPrefabChild<T>(init, variantName);
    }

    /// <summary>
    /// Spawn a prefab instance as the child of the scene root
    /// </summary>
    /// <typeparam name="T">Type of the prefab's main script class</typeparam>
    /// <param name="init">A callback to initialize your new prefab instance before it is plonked into the scene</param>
    /// <param name="variantName">If the prefab folder has different prefab variants, specify which one here. This must be the folder name, then an underscore, then a variant name.</param>
    /// <returns>New prefab instance</returns>
    public T SpawnPrefabAtRoot<T>(PrefabInitCallback<T> init = null, string variantName = null) where T : Node
    {
        return NodeExtensions.SpawnPrefabAtRoot(this, init, variantName);
    }
}
