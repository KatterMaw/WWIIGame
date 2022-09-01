using Godot;

namespace WWIIGame.Misc;

public static class Settings
{
	private const string Physics3DDefaultGravity = "physics/3d/default_gravity";

	public static float GravityMagnitude
	{
		get => (float) ProjectSettings.GetSetting(Physics3DDefaultGravity);
		set => ProjectSettings.SetSetting(Physics3DDefaultGravity, value);
	}
}
