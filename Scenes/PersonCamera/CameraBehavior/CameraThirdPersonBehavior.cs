using Godot;

namespace WWIIGame.Scenes.CameraBehavior;

public abstract class CameraThirdPersonBehavior
{
	protected float CurrentDistance => _hinge.Translation.z;
	
	private readonly Spatial _hinge;
	private readonly Tween _tween;
	private readonly float _tweenDuration;

	private static readonly NodePath TranslationPath = new("translation:z");
	
	public CameraThirdPersonBehavior(Spatial hinge, Tween tween, float tweenDuration)
	{
		_hinge = hinge;
		_tween = tween;
		_tweenDuration = tweenDuration;
		GlobalInputManager.Instance.UnhandledInput += OnUnhandledInput;
	}

	protected abstract void OnUnhandledInput(InputEvent inputEvent);

	protected void SetDistance(float distance)
	{
		if (distance != 0 && CurrentDistance != 0)
		{
			_tween.InterpolateProperty(_hinge, TranslationPath, _hinge.Translation.z, distance, _tweenDuration,
				Tween.TransitionType.Quad);
			_tween.Start();
		}
		else _hinge.Translation = new Vector3(0, 0, distance);
	}
}
