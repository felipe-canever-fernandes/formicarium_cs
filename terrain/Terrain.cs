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

	public override void _Ready()
	{
		if (CubeSize <= 0)
		{
			throw new ArgumentOutOfRangeException
			(
				nameof(CubeSize),
				CubeSize,
				"the terrain's cube size must be greater than 0"
			);
		}
	}

	public void Generate(Formicarium.Voxels.Voxels voxels)
	{
		var vertices = new List<Vector3>();
		var normals = new List<Vector3>();
		var indices = new List<int>();

		voxels.ForEachVoxel
		((
			ref Formicarium.Voxels.Voxel voxel,
			Vector3I position
		) =>
		{
			if (voxel.Type != Formicarium.Voxels.VoxelType.Dirt)
			{
				return;
			}

			var currentVertexCount = vertices.Count;

			foreach (var side in CubeSides.Sides)
			{
				for (var v = 0; v < Cube.UniqueVerticesCountPerSide; ++v)
				{
					var vertex = Cube.SidesUniqueVertices[side][v];

					vertices.Add(vertex * CubeSize + position);
					normals.Add(Cube.SidesNormals[side]);
				}

				for (var i = 0; i < Cube.TotalVerticesCountPerSide; ++i)
				{
					var sideIndex = (int)side;

					indices.Add
					(
						Cube.SideVerticesIndices[i]
						+ sideIndex * Cube.UniqueVerticesCountPerSide
						+ currentVertexCount
					);
				}
			}
		});

		var arrays = new Godot.Collections.Array();
		arrays.Resize((int)Mesh.ArrayType.Max);

		arrays[(int)Mesh.ArrayType.Vertex] = vertices.ToArray();
		arrays[(int)Mesh.ArrayType.Normal] = normals.ToArray();
		arrays[(int)Mesh.ArrayType.Index] = indices.ToArray();

		var mesh = new ArrayMesh();
		mesh.AddSurfaceFromArrays(Mesh.PrimitiveType.Triangles, arrays);

		var meshInstance = GetNode<MeshInstance3D>("MeshInstance");
		meshInstance.Mesh = mesh;
	}
}
