using Godot;
using System;

public partial class Camera : Camera3D
{
	private const float MinimumXRotation = -90;
	private const float MaximumXRotation = 90;

	private float _movementSpeed;
	private float _rotationSpeedInDegrees;

	private Vector2 _mouseMotionDelta;

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

	/// <summary>
	/// How fast the camera rotates in degrees.
	/// </summary>
	[Export]
	public float RotationSpeedInDegrees
	{
		get => _rotationSpeedInDegrees;

		set
		{
			if (value <= 0)
			{
				throw new ArgumentOutOfRangeException
					("the camera rotation speed must be greater than 0");
			}

			_rotationSpeedInDegrees = value;
		}
	}

	public override void _Process(double delta)
	{
		HandleMovement();
		HandleRotation();

		void HandleMovement()
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
	
		void HandleRotation()
		{
			_mouseMotionDelta *= RotationSpeedInDegrees * (float)delta;

			RotationDegrees = RotationDegrees with
			{ 
				X = Math.Clamp
				(
					RotationDegrees.X + _mouseMotionDelta.Y,
					MinimumXRotation,
					MaximumXRotation
				),

				Y = RotationDegrees.Y + _mouseMotionDelta.X
			};

			_mouseMotionDelta = Vector2.Zero;
		}
	}

	public override void _Input(InputEvent inputEvent)
	{
		if (inputEvent is not InputEventMouseMotion inputEventMouseMotion)
		{
			return;
		}

		if (!Input.IsActionPressed("rotate_camera"))
		{
			Input.MouseMode = Input.MouseModeEnum.Visible;
			return;
		}

		Input.MouseMode = Input.MouseModeEnum.Captured;
		_mouseMotionDelta -= inputEventMouseMotion.Relative;
	}
}
