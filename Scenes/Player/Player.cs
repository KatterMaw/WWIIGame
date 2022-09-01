using System;
using Godot;

namespace WWIIGame.Scenes;

public sealed class Player : KinematicBody
{
	#region Export properties

	[Export] public float WalkSpeed = 4f;
	[Export] public float Acceleration = 50;
	[Export] public float Friction = 1.2f;

	#endregion

	#region Public members

	

	#endregion

	#region Public methods

	public override void _Ready()
	{
		_gravityMagnitude = GravityMagnitudeFromSettings;
		_walkSpeedSquared = WalkSpeed * WalkSpeed;

		if (Friction <= 1) throw new Exception("Friction must be greater than 1");
	}

	public override void _PhysicsProcess(float delta)
	{
		Move(delta);
	}

	#endregion

	#region Private members

	private Vector3 _velocity = Vector3.Zero;
	private float _gravityMagnitude;
	private float _walkSpeedSquared;

	#endregion

	#region Private methods

	private void Move(float delta)
	{
		Vector2 localDirection = Input.GetVector("move_left", "move_right", "move_back", "move_forward");
		localDirection.y = -localDirection.y;
		Vector2 globalDirection = localDirection.Rotated(-Rotation.y);
		Vector2 acceleration = globalDirection * Acceleration * delta;
		Vector2 velocity = new(_velocity.x, _velocity.z);
		velocity /= Friction;
		velocity += acceleration;
		if (velocity.LengthSquared() > _walkSpeedSquared) velocity = velocity.Normalized() * WalkSpeed;
		_velocity = MoveAndSlide(new Vector3(velocity.x, _velocity.y - _gravityMagnitude * delta, velocity.y));
	}

	private static float GravityMagnitudeFromSettings => (float) ProjectSettings.GetSetting("physics/3d/default_gravity");

	#endregion
	
}