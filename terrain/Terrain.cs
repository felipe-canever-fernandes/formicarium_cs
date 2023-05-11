using Godot;
using System;
using System.Collections.Generic;

namespace Formicarium.Terrain;

/// <summary>
/// The procedurally-generated voxel-based world terrain.
/// </summary>
public partial class Terrain : Node3D
{
	public void Generate(Voxels.Voxels voxels)
	{
		var vertices = new List<Vector3>();
		var normals = new List<Vector3>();
		var indices = new List<int>();

		voxels.ForEachVoxel((Voxels.Voxel voxel, Vector3I position) =>
		{
			if (voxel.Type != Voxels.Type.Dirt)
			{
				return;
			}

			var currentVertexCount = vertices.Count;

			foreach (var side in CubeSides.Sides)
			{
				for (var v = 0; v < Cube.UniqueVerticesCountPerSide; ++v)
				{
					var vertex = Cube.SidesUniqueVertices[side][v];

					vertices.Add(vertex + position);
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
