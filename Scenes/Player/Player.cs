using System;
using Godot;
using WWIIGame.Misc;

namespace WWIIGame.Scenes;

public sealed class Player : KinematicBody
{
	#region Export properties

	[Export] public float WalkSpeed = 4f;
	[Export] public float Acceleration = 50;
	[Export] public float Friction = 10f;
	[Export] public float MaxSpeedToFullSpeed = 0.1f;

	#endregion

	#region Public members

	public Vector2 LocalMoveDirection;

	#endregion

	#region Public methods

	public override void _Ready()
	{
		_gravityMagnitude = Settings.GravityMagnitude;
		_walkSpeedSquared = WalkSpeed * WalkSpeed;
		_maxSpeedToFullStopSquared = MaxSpeedToFullSpeed * MaxSpeedToFullSpeed;

		if (Friction <= 0) throw new Exception("Friction must be greater than 0");
	}

	public override void _PhysicsProcess(float delta) => Move(delta);

	#endregion

	#region Private members

	private Vector3 _velocity = Vector3.Zero;
	private float _gravityMagnitude;
	private float _walkSpeedSquared;
	private float _maxSpeedToFullStopSquared;

	#endregion

	#region Private methods

	private void Move(float delta)
	{
		LocalMoveDirection = Input.GetVector("move_left", "move_right", "move_back", "move_forward");
		Vector2 horizontalVelocity = new(_velocity.x, _velocity.z);
		if (LocalMoveDirection != Vector2.Zero)
		{
			Vector2 localDirection = new(LocalMoveDirection.x, -LocalMoveDirection.y);
			Vector2 globalDirection = localDirection.Rotated(-Rotation.y);
			Vector2 acceleration = globalDirection * Acceleration;
			horizontalVelocity /= 1 + Friction * delta;
			horizontalVelocity += acceleration * delta;
			if (horizontalVelocity.LengthSquared() > _walkSpeedSquared) horizontalVelocity = horizontalVelocity.Normalized() * WalkSpeed;
		}
		else horizontalVelocity /= 1 + Friction * delta;

		_velocity = MoveAndSlide(new Vector3(horizontalVelocity.x, _velocity.y - _gravityMagnitude * delta, horizontalVelocity.y));
		if (_velocity.LengthSquared() <= _maxSpeedToFullStopSquared) _velocity = Vector3.Zero;
	}

	#endregion

}