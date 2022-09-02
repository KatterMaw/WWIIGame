using System;
using Godot;
using WWIIGame.Scenes.CameraBehavior;

namespace WWIIGame.Scenes;

public sealed class PersonCamera : Spatial
{
	#region Export properties

	[Export] public NodePath? ControlledEntity;
	[Export] public float MouseSensitivity = 0.2f;
	[Export] public float MaxVerticalRotation = 90;
	[Export] public float MinVerticalRotation = -90;
	[Export] public ViewType DefaultView = ViewType.FirstPerson;
	[Export] public ThirdPersonMode ThirdPersonMode = ThirdPersonMode.Scrollable;
	[Export] public float MinThirdPersonDistance = 1;
	[Export] public float MaxThirdPersonDistance = 10;
	[Export] public float CloseDistance = 2;
	[Export] public float MidDistance = 4;
	[Export] public float FarDistance = 6;
	[Export] public float TweenDuration = 0.2f;
	[Export] public float ZoomStep = 0.5f;

	#endregion

	#region Public methods

	public override void _Ready()
	{
		try
		{
			InitializeNodes();
			InitializeValues();
			InitializeViewMode();
		}
		catch
		{
			SetProcessInput(false);
			throw;
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
	private const string HingeNodePath = "VerticalRotate/Hinge";
	private const string TweenNodePath = "VerticalRotate/Hinge/Tween";
	private const string ControlledEntityExceptionMessage = "Controlled Entity property was set, but it's not Spatial or don't exist in tree";
	private const string OwnerException = "Camera has no owner, or owner is not spatial node, so, it can't work";

	private Spatial _verticalRotate = null!;
	private Spatial _controlledEntity = null!;
	private Spatial _hinge = null!;
	private Tween _tween = null!;

	private float _maxVerticalRotationInRadians;
	private float _minVerticalRotationInRadians;

	private CameraThirdPersonBehavior _behavior = null!;

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
		_controlledEntity = ControlledEntity == null
			? GetOwnerOrNull<Spatial>() ?? throw new Exception(OwnerException)
			: GetNodeOrNull<Spatial>(ControlledEntity) ?? throw new Exception(ControlledEntityExceptionMessage);
		_hinge = GetNode<Spatial>(HingeNodePath);
		_tween = GetNode<Tween>(TweenNodePath);
	}

	private void InitializeValues()
	{
		_maxVerticalRotationInRadians = Mathf.Deg2Rad(MaxVerticalRotation);
		_minVerticalRotationInRadians = Mathf.Deg2Rad(MinVerticalRotation);
	}

	private void InitializeViewMode()
	{
		_behavior = ThirdPersonMode switch
		{
			ThirdPersonMode.ChainToggle => new ToggleChainBehavior(_hinge, _tween, TweenDuration, CloseDistance,
				MidDistance, FarDistance),
			ThirdPersonMode.Scrollable => new ScrollBehavior(_hinge, _tween, TweenDuration, ZoomStep,
				MinThirdPersonDistance, MaxThirdPersonDistance),
			_ => throw new ArgumentOutOfRangeException()
		};
	}

	#endregion
}