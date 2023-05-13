using Godot;

namespace Formicarium.World;

/// <summary>
/// The game world, responsible for managing everything.
/// </summary>
public partial class World : Node3D
{
	/// <summary>
	/// The initial ant position. The Y position will be determined
	/// automatically based on the terrain.
	/// </summary>
	[Export]
	private Vector3 AntPosition { get; set; }

	public override void _Ready()
	{
		var voxels = GetNode<Voxels.Voxels>("Voxels");
		GenerateTerrain();
		PlaceAntOnGround();

		void GenerateTerrain()
		{
			var terrain = GetNode<Terrain.Terrain>("Terrain");
			terrain.Generate(voxels);
		}

		void PlaceAntOnGround()
		{
			var position = (Vector3I)AntPosition;

			for (position.Y = voxels.Size.Y - 1; position.Y >= 0; --position.Y)
			{
				if (voxels[position].Type == Voxels.Type.Dirt)
				{
					break;
				}
			}

			var ant = GetNode<Ant>("Ant");
			ant.Position = AntPosition with { Y = position.Y + 1 };
		}
	}
}
