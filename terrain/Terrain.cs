using Godot;
using System;

public partial class Terrain : Node3D
{

	[Export]
	public float CubeSize { get; set; } = 1;
	[Export]
	public Vector3 CubePosition { get; set; } = Vector3.Zero;

	public override void _Ready()
	{
		var meshInstance = GetNode<MeshInstance3D>("MeshInstance");
		meshInstance.Mesh = CreateCubeMesh();

		ArrayMesh CreateCubeMesh()
		{
			if (CubeSize <= 0)
			{
				throw new ArgumentException
					("the terrain's cube size must be greater than 0");
			}

			var uniqueVertices = new Vector3[Cube.VerticesCount];
			var uniqueVerticesNormals = new Vector3[Cube.VerticesCount];
			var verticesIndices = new int[Cube.TotalVerticesCount];

			var uniqueVertexIndex = 0;
			var vertexIndexIndex = 0;

			for (var sideIndex = 0; sideIndex < Cube.Sides.Length; ++sideIndex)
			{
				var side = Cube.Sides[sideIndex];

				for
				(
					var sideUniqueVertexIndex = 0;
					sideUniqueVertexIndex < Cube.UniqueVerticesCountPerSide;
					++sideUniqueVertexIndex
				)
				{
					var sideUniqueVertex =
						Cube.SidesUniqueVertices[side][sideUniqueVertexIndex];

					uniqueVertices[uniqueVertexIndex] =
						sideUniqueVertex * CubeSize + CubePosition;
					
					uniqueVerticesNormals[uniqueVertexIndex] =
						Cube.SidesNormals[side];

					++uniqueVertexIndex;
				}

				for
				(
					var sideVertexIndexIndex = 0;
					sideVertexIndexIndex < Cube.TotalVerticesCountPerSide;
					++sideVertexIndexIndex
				)
			{
					var vertexIndexOffset =
						sideIndex * Cube.UniqueVerticesCountPerSide;

					verticesIndices[vertexIndexIndex] =
						Cube.SideVerticesIndices[sideVertexIndexIndex]
						+ vertexIndexOffset;
					
					++vertexIndexIndex;
				}
			}

			var arrays = new Godot.Collections.Array();
			arrays.Resize((int)Mesh.ArrayType.Max);

			arrays[(int)Mesh.ArrayType.Vertex] = uniqueVertices;
			arrays[(int)Mesh.ArrayType.Normal] = uniqueVerticesNormals;
			arrays[(int)Mesh.ArrayType.Index] = verticesIndices;

			var mesh = new ArrayMesh();
			mesh.AddSurfaceFromArrays(Mesh.PrimitiveType.Triangles, arrays);

			return mesh;
		}
	}
}
