using System;
using Godot;

namespace WWIIGame.Scenes.CameraBehavior;

public class ScrollBehavior : CameraThirdPersonBehavior
{
	private readonly float _zoomStep;
	private readonly float _minThirdPersonDistance;
	private readonly float _maxThirdPersonDistance;

	public ScrollBehavior(Spatial hinge, Tween tween, float tweenDuration, float zoomStep, float minThirdPersonDistance, float maxThirdPersonDistance) : base(hinge, tween, tweenDuration)
	{
		_zoomStep = zoomStep;
		_minThirdPersonDistance = minThirdPersonDistance;
		_maxThirdPersonDistance = maxThirdPersonDistance;
	}

	protected override void OnUnhandledInput(InputEvent inputEvent)
	{
		if (Input.IsActionJustPressed("view_zoom_in")) ZoomIn();
		else if (Input.IsActionJustPressed("view_zoom_out")) ZoomOut();
	}
	
	private void ZoomIn() =>
		SetDistance(Mathf.IsEqualApprox(_minThirdPersonDistance, CurrentDistance)
			? 0
			: Math.Max(_minThirdPersonDistance, CurrentDistance - _zoomStep));

	private void ZoomOut() =>
		SetDistance(CurrentDistance < _minThirdPersonDistance 
			? _minThirdPersonDistance
			: Math.Min(_maxThirdPersonDistance, CurrentDistance + _zoomStep));
}
