using Godot;

namespace WWIIGame.Scenes.CameraBehavior;

public sealed class ToggleChainBehavior : CameraThirdPersonBehavior
{
	private readonly float[] _distances;

	private byte _currentDistanceIndex;

	public ToggleChainBehavior(Spatial hinge, Tween tween, float tweenDuration, float closeDistance, float midDistance, float farDistance) : base(hinge, tween, tweenDuration)
	{
		_distances = new[] {0, closeDistance, midDistance, farDistance};
	}

	protected override void OnUnhandledInput(InputEvent inputEvent)
	{
		if (Input.IsActionJustPressed("toggle_view")) ToggleView();
	}

	private void ToggleView()
	{
		float newDistance = GetNextDistance();
		SetDistance(newDistance);
	}

	private float GetNextDistance()
	{
		_currentDistanceIndex++;
		if (_currentDistanceIndex >= _distances.Length) _currentDistanceIndex = 0;
		return _distances[_currentDistanceIndex];
	}
}
