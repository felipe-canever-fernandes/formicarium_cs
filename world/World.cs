using Godot;

namespace Formicarium.World;

/// <summary>
/// The game world, responsible for managing everything.
/// </summary>
public partial class World : Node3D
{
	public override void _Ready()
	{
		var voxels = GetNode<Voxels>("Voxels");
		var terrain = GetNode<Terrain>("Terrain");

		terrain.Generate(voxels);
	}
}
