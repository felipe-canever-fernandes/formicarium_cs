using Godot;

public partial class Terrain : Node3D
{
	public override void _Ready()
	{
		var meshInstance = GetNode<MeshInstance3D>("MeshInstance");
		meshInstance.Mesh = CreateCubeMesh();

		ArrayMesh CreateCubeMesh()
		{
			var arrays = new Godot.Collections.Array();
			arrays.Resize((int)Mesh.ArrayType.Max);

			arrays[(int)Mesh.ArrayType.Vertex] = new Vector3[]
			{
				// Face 0
				new(0, 0, 0),
				new(1, 0, 0),
				new(1, 1, 0),
				new(0, 1, 0),
				// Face 1
				new(1, 0, 0),
				new(1, 0, 1),
				new(1, 1, 1),
				new(1, 1, 0),
				// Face 2
				new(1, 0, 1),
				new(0, 0, 1),
				new(0, 1, 1),
				new(1, 1, 1),
				// Face 3
				new(0, 0, 1),
				new(0, 0, 0),
				new(0, 1, 0),
				new(0, 1, 1),
				// Face 4
				new(0, 0, 1),
				new(1, 0, 1),
				new(1, 0, 0),
				new(0, 0, 0),
				// Face 5
				new(0, 1, 0),
				new(1, 1, 0),
				new(1, 1, 1),
				new(0, 1, 1),
			};

			arrays[(int)Mesh.ArrayType.Index] = new[]
			{
				// Face 0
				0, 1, 3,
				1, 2, 3,
				// Face 1
				4, 5, 7,
				5, 6, 7,
				// Face 2
				8, 9, 11,
				9, 10, 11,
				// Face 3
				12, 13, 15,
				13, 14, 15,
				// Face 4
				16, 17, 19,
				17, 18, 19,
				// Face 5
				20, 21, 23,
				21, 22, 23,
			};

			arrays[(int)Mesh.ArrayType.Normal] = new Vector3[]
			{
				// Face 0
				new(0, 0, -1),
				new(0, 0, -1),
				new(0, 0, -1),
				new(0, 0, -1),
				// Face 1
				new(1, 0, 0),
				new(1, 0, 0),
				new(1, 0, 0),
				new(1, 0, 0),
				// Face 2
				new(0, 0, 1),
				new(0, 0, 1),
				new(0, 0, 1),
				new(0, 0, 1),
				// Face 3
				new(-1, 0, 0),
				new(-1, 0, 0),
				new(-1, 0, 0),
				new(-1, 0, 0),
				// Face 4
				new(0, -1, 0),
				new(0, -1, 0),
				new(0, -1, 0),
				new(0, -1, 0),
				// Face 5
				new(0, 1, 0),
				new(0, 1, 0),
				new(0, 1, 0),
				new(0, 1, 0),
			};

			var mesh = new ArrayMesh();
			mesh.AddSurfaceFromArrays(Mesh.PrimitiveType.Triangles, arrays);

			return mesh;
		}
	}
}
