public static class CubeSides
{
	public enum Side
	{
		Front,
		Right,
		Back,
		Left,
		Bottom,
		Top,
	}

	/// <summary>
	/// A list of all the possible sides of a cube.
	/// </summary>
	/// 
	/// <value>
	/// All the enumerators of <see cref="CubeSide"/>.
	/// </value>
	public static Side[] Sides { get; } = new[]
	{
		Side.Front,
		Side.Right,
		Side.Back,
		Side.Left,
		Side.Bottom,
		Side.Top,
	};
}

