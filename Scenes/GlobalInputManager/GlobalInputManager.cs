using Godot;

namespace WWIIGame.Scenes;

public sealed class GlobalInputManager : Node
{
	public static GlobalInputManager Instance { get; private set; }
	
	public override void _Ready() => Instance = this;

	public delegate void InputHandler(InputEvent inputEvent);

	public event InputHandler? UnhandledInput;
	
	public override void _Input(InputEvent inputEvent) => UnhandledInput?.Invoke(inputEvent);
}