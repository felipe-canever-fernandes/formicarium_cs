namespace Formicarium.Voxels;

/// <summary>
/// A terrain voxel.
/// </summary>
internal class Voxel
{
	public Voxel(Type type = Type.Air) => Type = type;
	public Type Type { get; set; }
}
