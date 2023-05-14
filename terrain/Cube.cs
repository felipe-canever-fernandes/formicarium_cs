using Godot;
using System.Collections.Generic;

namespace Formicarium.Terrain;

/// <summary>
/// Contains information for building a 3D cube mesh.
/// </summary>
internal static class Cube
{
	/// <summary>
	/// The number of vertices in a triangle.
	/// </summary>
	public const int VerticesCountPerTriangle = 3;
	/// <summary>
	/// The number of triangles that make up one side of a cube.
	/// </summary>
	public const int TriangleCountPerSide = 2;

	/// <summary>
	/// The total number of vertices in one side of a cube.
	/// </summary>
	public const int TotalVerticesCountPerSide =
		VerticesCountPerTriangle * TriangleCountPerSide;

	/// <summary>
	/// The list of unique vertex coordinates for each side of a cube.
	/// </summary>
	/// 
	/// <value>
	/// A key-value pair from the side of a cube to a list of vertices.
	/// </value>
	public static Dictionary<CubeSides.Side, Vector3[]> SidesUniqueVertices
	{ get; } = new()
	{
		{
			CubeSides.Side.Front,

			new Vector3[]
			{
				new(0, 0, 0),
				new(1, 0, 0),
				new(1, 1, 0),
				new(0, 1, 0),
			}
		},
		{
			CubeSides.Side.Right,

			new Vector3[]
			{
				new(1, 0, 0),
				new(1, 0, 1),
				new(1, 1, 1),
				new(1, 1, 0),
			}
		},
		{
			CubeSides.Side.Back,

			new Vector3[]
			{
				new(1, 0, 1),
				new(0, 0, 1),
				new(0, 1, 1),
				new(1, 1, 1),
			}
		},
		{
			CubeSides.Side.Left,

			new Vector3[]
			{
				new(0, 0, 1),
				new(0, 0, 0),
				new(0, 1, 0),
				new(0, 1, 1),
			}
		},
		{
			CubeSides.Side.Bottom,

			new Vector3[]
			{
				new(0, 0, 1),
				new(1, 0, 1),
				new(1, 0, 0),
				new(0, 0, 0),
			}
		},
		{
			CubeSides.Side.Top,

			new Vector3[]
			{
				new(0, 1, 0),
				new(1, 1, 0),
				new(1, 1, 1),
				new(0, 1, 1),
			}
		},
	};

	/// <summary>
	/// The normal vector for each side of a cube.
	/// </summary>
	/// 
	/// <value>
	/// A key-value pair from the side of a cube to a vector.
	/// </value>
	public static Dictionary<CubeSides.Side, Vector3> SidesNormals
	{ get; } = new()
	{
		{CubeSides.Side.Front,	new( 0,  0, -1)},
		{CubeSides.Side.Right,	new( 1,  0,  0)},
		{CubeSides.Side.Back,	new( 0,  0,  1)},
		{CubeSides.Side.Left,	new(-1,  0,  0)},
		{CubeSides.Side.Bottom,	new( 0, -1,  0)},
		{CubeSides.Side.Top,	new( 0,  1,  0)},
	};

	/// <summary>
	/// The list of indices corresponding to one of the unique vertices for each
	/// side of a cube in <see cref="SidesUniqueVertices"/> that make up one
	/// side of a cube.
	/// </summary>
	///
	/// <value>
	/// An integer between 0 and <see cref="UniqueVerticesCountPerSide"/>.
	/// </value>
	public static int[] SideVerticesIndices { get; } = new[]
	{
		0, 1, 3,
		1, 2, 3,
	};

	/// <summary>
	/// The number of unique vertices in the side of a cube.
	/// </summary>
	/// 
	/// <value>
	/// An integer greater than 0.
	/// </value>
	public static int UniqueVerticesCountPerSide { get; } =
		SidesUniqueVertices[CubeSides.Sides[0]].Length;

	/// <summary>
	/// The number of vertices in a cube when taking into account the number of
	/// unique vertices per side.
	/// </summary>
	///
	/// <value>
	/// An integer greater than 0.
	/// </value>
	public static int VerticesCount { get; } =
		UniqueVerticesCountPerSide * CubeSides.Sides.Length;
	
	/// <summary>
	/// The total number of vertices in a cube.
	/// </summary>
	///
	/// <value>
	/// An integer greater than 0.
	/// </value>
	public static int TotalVerticesCount { get; } =
		TotalVerticesCountPerSide * CubeSides.Sides.Length;
}
