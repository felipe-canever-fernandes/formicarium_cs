using Godot;
using System;

/// <summary>
/// The cells that make up the world.
/// </summary>
public partial class Voxels : Node
{
	private Voxel[,,] _voxels;

	/// <summary>
	/// The type of a voxel operation that is used by
	/// <see cref="ForEachVoxel"/>.
	/// </summary>
	/// 
	/// <param name="voxel">The current voxel.</param>
	/// <param name="position">The current voxel's position.</param>
	public delegate void VoxelOperation(ref Voxel voxel, Vector3I position);

	/// <summary>
	/// The 3D size of the world in voxels.
	/// </summary>
	///
	/// <value>
	/// A vector representing the width, height, and depth of the world. Each
	/// component must be greater than 0.
	/// </value>
	[Export]
	public Vector3I Size { get; set; }

	#region FunctionParameters

	/// <summary>
	/// The height of the land mass in voxel coordinates.
	/// </summary>
	[Export]
	public float LandLevel { get; set; }

	/// <summary>
	/// The amplitude in x for the cosine.
	/// </summary>
	[ExportGroup("X Cosine Parameters", "X")]
	[Export]
	public float XAmplitude  { get; set; } = 1;
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
		// Check size.

		if (Size.X <= 0)
		{
			throw new ArgumentOutOfRangeException
			(
				nameof(Size.X),
				Size.X,
				"the voxels' x size must be greater than 0"
			);
		}

		if (Size.Y <= 0)
		{
			throw new ArgumentOutOfRangeException
			(
				nameof(Size.Y),
				Size.Y,
				"the voxels' y size must be greater than 0"
			);
		}

		if (Size.Z <= 0)
		{
			throw new ArgumentOutOfRangeException
			(
				nameof(Size.Z),
				Size.Z,
				"the voxels' z size must be greater than 0"
			);
		}

		// Generate voxels.

		_voxels = new Voxel[Size.X, Size.Y, Size.Z];

		ForEachVoxel((ref Voxel voxel, Vector3I position) =>
		{
			var x = XAmplitude * Math.Cos(XFrequency * position.X + XPhase);
			var z = ZAmplitude * Math.Cos(ZFrequency * position.Z + ZPhase);

			if (position.Y <= x + z + LandLevel)
			{
				voxel.Type = VoxelType.Dirt;
			}
		});
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
		if (voxelOperation is null)
		{
			throw new ArgumentNullException
			(
				nameof(voxelOperation),
				"the voxel operation must not be null"
			);
		}

		for (var x = 0; x < _voxels.GetLength(0); ++x)
		{
			for (var y = 0; y < _voxels.GetLength(1); ++y)
			{
				for (var z = 0; z < _voxels.GetLength(2); ++z)
				{
					ref var voxel = ref _voxels[x, y, z];
					var position = new Vector3I(x, y, z);

					voxelOperation.Invoke(ref voxel, position);
				}
			}
		}
	}
}
