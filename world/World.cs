using Godot;

namespace Formicarium.World;

/// <summary>
/// The game world, responsible for managing everything.
/// </summary>
internal partial class World : Node3D
{
	/// <summary>
	/// The initial ant position. The Y position will be determined
	/// automatically based on the terrain.
	/// </summary>
	[Export]
	private Vector3 AntInitialPosition { get; set; }

	public override void _Ready()
	{
		var voxels = GetNode<Voxels.Voxels>("Voxels");
		var ant = GetNode<Ant>("Ant");
		var camera = GetNode<Camera3D>("Camera");

		GenerateTerrain();
		PlaceAntOnGround(out Vector3 originalCameraOffsetFromAnt);
		PlaceCamera(originalCameraOffsetFromAnt);

		void GenerateTerrain()
		{
			var terrain = GetNode<Terrain.Terrain>("Terrain");
			terrain.Generate(voxels);
		}

		void PlaceAntOnGround(out Vector3 originalCameraOffsetFromAnt)
		{
			var position = (Vector3I)AntInitialPosition;

			for (position.Y = voxels.Size.Y - 1; position.Y >= 0; --position.Y)
			{
				if (voxels[position].Type == Voxels.Type.Dirt)
				{
					break;
				}
			}

			originalCameraOffsetFromAnt = camera.Position - ant.Position;
			ant.Position = AntInitialPosition with { Y = position.Y + 1 };
		}
	
		void PlaceCamera(Vector3 cameraOffsetFromAnt)
		{
			camera.Position = ant.Position + cameraOffsetFromAnt;
		}
	}
}
