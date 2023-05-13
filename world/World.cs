using Godot;

namespace Formicarium.World;

/// <summary>
/// The game world, responsible for managing everything.
/// </summary>
public partial class World : Node3D
{
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
			var ant = GetNode<Ant>("Ant");
			var position = (Vector3I)ant.Position;

			for (position.Y = voxels.Size.Y - 1; position.Y >= 0; --position.Y)
			{
				if (voxels[position].Type == Voxels.Type.Dirt)
				{
					break;
				}
			}

			ant.Position = ant.Position with { Y = position.Y + 1 };
		}
	}
}
