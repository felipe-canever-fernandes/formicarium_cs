using Godot;
using System;
using System.Collections.Generic;

/// <summary>
/// The procedurally-generated voxel-based world terrain.
/// </summary>
public partial class Terrain : Node3D
{

	/// <summary>
	/// The size of each side of a cube in world units.
	/// </summary>
	/// 
	/// <value>
	/// A floating-point number greater than 0.
	/// </value>
	[Export]
	public float CubeSize { get; set; } = 1;
	/// <summary>
	/// The position of the cube in world coordinates.
	/// </summary>
	[Export]
	public Vector3 CubePosition { get; set; } = Vector3.Zero;

	public override void _Ready()
	{
		GenerateCube();

		void GenerateCube()
		{
			if (CubeSize <= 0)
			{
				throw new ArgumentException
					("the terrain's cube size must be greater than 0");
			}

			var uniqueVertices = new List<Vector3>();
			var uniqueVerticesNormals = new List<Vector3>();
			var verticesIndices = new List<int>();

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

					uniqueVertices.Add
					(
						sideUniqueVertex * CubeSize + CubePosition
					);
					
					uniqueVerticesNormals.Add(Cube.SidesNormals[side]);
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

					verticesIndices.Add
					(
						Cube.SideVerticesIndices[sideVertexIndexIndex]
						+ vertexIndexOffset
					);
				}
			}

			var arrays = new Godot.Collections.Array();
			arrays.Resize((int)Mesh.ArrayType.Max);

			arrays[(int)Mesh.ArrayType.Vertex] = uniqueVertices.ToArray();

			arrays[(int)Mesh.ArrayType.Normal] =
				uniqueVerticesNormals.ToArray();

			arrays[(int)Mesh.ArrayType.Index] = verticesIndices.ToArray();

			var mesh = new ArrayMesh();
			mesh.AddSurfaceFromArrays(Mesh.PrimitiveType.Triangles, arrays);

			var meshInstance = GetNode<MeshInstance3D>("MeshInstance");
			meshInstance.Mesh = mesh;
		}
	}
}
