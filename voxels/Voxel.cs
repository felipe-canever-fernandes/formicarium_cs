/// <summary>
/// A terrain voxel.
/// </summary>
public struct Voxel
{
	public Voxel(VoxelType type = VoxelType.Air) => Type = type;
	public VoxelType Type { get; set; }
}

/// <summary>
/// The type of a terrain voxel.
/// </summary>
public enum VoxelType
{
	Air,
	Dirt,
}
