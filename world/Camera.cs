using Godot;
using System;

public partial class Camera : Camera3D
{
	private float _movementSpeed;

	/// <summary>
	/// How fast the camera moves linearly in world units.
	/// </summary>
	[Export]
	public float MovementSpeed
	{
		get => _movementSpeed;

		set
		{
			if (value <= 0)
			{
				throw new ArgumentOutOfRangeException
					("the camera movement speed must be greater than 0");
			}

			_movementSpeed = value;
		}
	}

	public override void _Process(double delta)
	{
		var direction = Vector3.Zero;

		if (Input.IsActionPressed("move_camera_left"))
		{
			direction.X += -1;
		}

		if (Input.IsActionPressed("move_camera_right"))
		{
			direction.X += 1;
		}

		if (Input.IsActionPressed("move_camera_down"))
		{
			direction.Y += -1;
		}

		if (Input.IsActionPressed("move_camera_up"))
		{
			direction.Y += 1;
		}

		if (Input.IsActionPressed("move_camera_forward"))
		{
			direction.Z += -1;
		}

		if (Input.IsActionPressed("move_camera_backward"))
		{
			direction.Z += 1;
		}

		var velocity = direction * MovementSpeed * (float)delta;
		TranslateObjectLocal(velocity);
	}
}
