using Godot;
using System;
using System.Collections.Generic;

namespace Formicarium.Voxels;

/// <summary>
/// The cells that make up the world.
/// </summary>
internal partial class Voxels : Node
{
	/// <summary>
	/// The position offset to get to an adjacent voxel.
	/// </summary>
	private static readonly Dictionary<CubeSides.Side, Vector3I>
		_adjacentVoxelsOffsets = new()
	{
		{CubeSides.Side.Front,	new( 0,  0, -1)},
		{CubeSides.Side.Right,	new( 1,  0,  0)},
		{CubeSides.Side.Back,	new( 0,  0,  1)},
		{CubeSides.Side.Left,	new(-1,  0,  0)},
		{CubeSides.Side.Bottom,	new( 0, -1,  0)},
		{CubeSides.Side.Top,	new( 0,  1,  0)},
	};

	private Vector3I _size;
	private Voxel[,,] _voxels;

	/// <summary>
	/// The type of a voxel operation that is used by
	/// <see cref="ForEachVoxel"/>.
	/// </summary>
	/// 
	/// <param name="voxel">The current voxel.</param>
	/// <param name="position">The current voxel's position.</param>
	public delegate void VoxelOperation(Voxel voxel, Vector3I position);

	/// <summary>
	/// The 3D size of the world in voxels.
	/// </summary>
	///
	/// <value>
	/// A vector representing the width, height, and depth of the world. Each
	/// component must be greater than 0.
	/// </value>
	[Export]
	public Vector3I Size
	{
		get => _size;

		private set
		{
			if (value.X <= 0)
			{
				throw new ArgumentOutOfRangeException
					("the voxels' x size must be greater than 0");
			}

			if (value.Y <= 0)
			{
				throw new ArgumentOutOfRangeException
					("the voxels' y size must be greater than 0");
			}

			if (value.Z <= 0)
			{
				throw new ArgumentOutOfRangeException
					("the voxels' z size must be greater than 0");
			}

			_size = value;
		}
	}

	#region FunctionParameters

	/// <summary>
	/// The height of the land mass in voxel coordinates.
	/// </summary>
	[Export]
	public float LandLevel { get; private set; }

	/// <summary>
	/// The amplitude in x for the cosine.
	/// </summary>
	[ExportGroup("X Cosine Parameters", "X")]
	[Export]
	public float XAmplitude  { get; private set; } = 1;
	/// <summary>
	/// The frequency in x for the cosine.
	/// </summary>
	[Export]
	public float XFrequency { get; set; } = 1;
	/// <summary>
	/// The phase in x for the cosine.
	/// </summary>
	[Export]
	public float XPhase { get; set; } = 0;

	/// <summary>
	/// The amplitude in z for the cosine.
	/// </summary>
	[ExportGroup("Z Cosine Parameters", "Z")]
	[Export]
	public float ZAmplitude  { get; set; } = 1;
	/// <summary>
	/// The frequency in z for the cosine.
	/// </summary>
	[Export]
	public float ZFrequency { get; set; } = 1;
	/// <summary>
	/// The phase in z for the cosine.
	/// </summary>
	[Export]
	public float ZPhase { get; set; } = 0;

	#endregion

	public override void _Ready()
	{
		_voxels = new Voxel[Size.X, Size.Y, Size.Z];

		for (var x = 0; x < _voxels.GetLength(0); ++x)
		{
			for (var y = 0; y < _voxels.GetLength(1); ++y)
			{
				for (var z = 0; z < _voxels.GetLength(2); ++z)
				{
					var voxel = new Voxel();

					var xHeight =
						XAmplitude * Math.Cos(XFrequency * x + XPhase);

					var zHeight =
						ZAmplitude * Math.Cos(ZFrequency * z + ZPhase); 

					if (y <= xHeight + zHeight + LandLevel)
					{
						voxel.Type = Type.Dirt;
					}

					_voxels[x, y, z] = voxel;
				}
			}
		}
	}

	/// <summary>
	/// The voxel at the given position.
	/// </summary>
	/// 
	/// <value>
	/// A voxel if the position is valid or null otherwise.
	/// </value>
	public Voxel this[Vector3I position]
	{
		get
		{
			if (!GetIsValidPosition(position))
			{
				return null;
			}

			return _voxels[position.X, position.Y, position.Z];
		}
	}

	/// <summary>
	/// Applies an operation onto every voxel.
	/// </summary>
	/// 
	/// <param name="voxelOperation">
	/// The operation to be applied onto every voxel. It must not be null.
	/// </param>
	public void ForEachVoxel(VoxelOperation voxelOperation)
	{
		ArgumentNullException.ThrowIfNull(nameof(voxelOperation));

		for (var x = 0; x < _voxels.GetLength(0); ++x)
		{
			for (var y = 0; y < _voxels.GetLength(1); ++y)
			{
				for (var z = 0; z < _voxels.GetLength(2); ++z)
				{
					var voxel = _voxels[x, y, z];
					var position = new Vector3I(x, y, z);

					voxelOperation.Invoke(voxel, position);
				}
			}
		}
	}

	/// <summary>
	/// Gives all the visible sides for the voxel at the given position.
	/// </summary>
	/// 
	/// <param name="position">
	/// The position of the voxel.
	/// </param>
	/// 
	/// <returns>
	/// A list containing enumerators for all the visible sides.
	/// </returns>
	public List<CubeSides.Side> GetVisibleSides(Vector3I position)
	{
		if (!GetIsValidPosition(position))
		{
			return null;
		}

		var visibleSides =
			new List<CubeSides.Side>(capacity: _adjacentVoxelsOffsets.Count);

		foreach (var (side, offset) in _adjacentVoxelsOffsets)
		{
			var adjacentVoxel = this[position + offset];
			
			if (adjacentVoxel is null)
			{
				continue;
			}

			if (adjacentVoxel.Type == Type.Dirt)
			{
				continue;
			}

			visibleSides.Add(side);
		}

		return visibleSides;
	}

	/// <summary>
	/// Whether the given position corresponds to a voxel.
	/// </summary>
	///
	/// <param name="position">
	/// The voxel position.
	/// </param>
	///
	/// <returns>
	/// True if the voxel position corresponds to a valid position, false
	/// otherwise.
	/// </returns>
	private bool GetIsValidPosition(Vector3I position) =>
		position.X >= 0 && position.X < Size.X &&
		position.Y >= 0 && position.Y < Size.Y &&
		position.Z >= 0 && position.Z < Size.Z;
}
