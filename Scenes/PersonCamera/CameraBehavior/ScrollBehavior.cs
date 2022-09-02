using System;
using Godot;

namespace WWIIGame.Scenes.CameraBehavior;

public class ScrollBehavior : CameraThirdPersonBehavior
{
	private readonly float _zoomStep;
	private readonly float _minThirdPersonDistance;
	private readonly float _maxThirdPersonDistance;
	private float _currentDistance;

	public ScrollBehavior(Spatial hinge, Tween tween, float tweenDuration, float zoomStep, float minThirdPersonDistance, float maxThirdPersonDistance) : base(hinge, tween, tweenDuration)
	{
		_zoomStep = zoomStep;
		_minThirdPersonDistance = minThirdPersonDistance;
		_maxThirdPersonDistance = maxThirdPersonDistance;

		_currentDistance = hinge.Translation.z;
	}

	protected override void OnUnhandledInput(InputEvent inputEvent)
	{
		if (Input.IsActionJustPressed("view_zoom_in")) ZoomIn();
		else if (Input.IsActionJustPressed("view_zoom_out")) ZoomOut();
	}
	
	private void ZoomIn()
	{
		if (Mathf.IsEqualApprox(_minThirdPersonDistance, _currentDistance)) _currentDistance = 0;
		else _currentDistance = Math.Max(_minThirdPersonDistance, _currentDistance - _zoomStep);
		SetDistance(_currentDistance);
	}
	
	private void ZoomOut()
	{
		if (_currentDistance < _minThirdPersonDistance) _currentDistance = _minThirdPersonDistance;
		else _currentDistance = Math.Min(_maxThirdPersonDistance, _currentDistance + _zoomStep);
		SetDistance(_currentDistance);
	}
}
