namespace Voxel;

/// <summary>
/// A terrain voxel.
/// </summary>
public struct Voxel
{
	public Voxel(Type type = Type.Air) => Type = type;
	public Type Type { get; set; }
}

/// <summary>
/// The type of a terrain voxel.
/// </summary>
public enum Type
{
	Air,
	Dirt,
}
