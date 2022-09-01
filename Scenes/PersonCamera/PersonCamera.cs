using System;
using Godot;

namespace WWIIGame.Scenes;

public sealed class PersonCamera : Spatial
{
	#region Export properties

	[Export] public NodePath? ControlledEntity;
	[Export] public float MouseSensitivity = 0.2f;
	[Export] public float MaxVerticalRotation = 90;
	[Export] public float MinVerticalRotation = -90;

	#endregion

	#region Public methods

	public override void _Ready()
	{
		try
		{
			InitializeNodes();
			InitializeValues();
		}
		catch
		{
			SetProcessInput(false);
		}
	}

	public override void _Input(InputEvent inputEvent)
	{
		if (inputEvent is not InputEventMouseMotion mouseMotionEvent) return;
		Vector2 delta = mouseMotionEvent.Relative;
		Vector2 vector2 = new(PrepareRawMouseInput(delta.x), PrepareRawMouseInput(delta.y));
		Rotate(vector2);
	}

	private float PrepareRawMouseInput(float rawValue) => Mathf.Deg2Rad(rawValue) * -MouseSensitivity;

	#endregion

	#region Private members
	
	private const string VerticalRotateNodePath = "VerticalRotate";
	private const string ControlledEntityExceptionMessage = "Controlled Entity property was set, but it's not Spatial or don't exist in tree";
	private const string OwnerException = "Camera has no owner, or owner is not spatial node, so, it can't work";

	private Spatial _verticalRotate = null!;
	private Spatial _controlledEntity = null!;

	private float _maxVerticalRotationInRadians;
	private float _minVerticalRotationInRadians;

	#endregion

	#region Private methods

	private void Rotate(Vector2 vector)
	{
		_controlledEntity.RotateY(vector.x);
		float rotationX = Mathf.Clamp(_verticalRotate.Rotation.x + vector.y, _minVerticalRotationInRadians, _maxVerticalRotationInRadians);
		_verticalRotate.Rotation = new Vector3(rotationX, 0, 0);
	}

	private void InitializeNodes()
	{
		_verticalRotate = GetNode<Spatial>(VerticalRotateNodePath);
		if (_verticalRotate == null) throw new Exception("Cannot found Spatial node by path \"VerticalRotate\"");

		_controlledEntity = ControlledEntity == null
			? GetOwnerOrNull<Spatial>() ?? throw new Exception(OwnerException)
			: GetNodeOrNull<Spatial>(ControlledEntity) ?? throw new Exception(ControlledEntityExceptionMessage);
	}

	private void InitializeValues()
	{
		_maxVerticalRotationInRadians = Mathf.Deg2Rad(MaxVerticalRotation);
		_minVerticalRotationInRadians = Mathf.Deg2Rad(MinVerticalRotation);
	}

	#endregion
}