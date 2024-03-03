
namespace Haven.Inputs;

public static class Input {
	public static KeyboardData Keyboard { get; private set; }
	public static MouseData Mouse { get; private set; }
	public static GamePadData[] GamePads { get; private set; }

	public static bool Active = true;
	public static bool Disabled = false;

	internal static void Initialize() {
		Keyboard = new();
		Mouse = new();
		GamePads = new GamePadData[4];
		for (int i = 0; i < 4; i++) {
			GamePads[i] = new((PlayerIndex)i);
		}
	}

	internal static void Shutdown() {
		foreach (GamePadData gamePadData in GamePads) {
			gamePadData.StopRumble();
		}
	}

	internal static void Update() {
		if (Engine.Instance.IsActive && Active) {
			Keyboard.Update();
			Mouse.Update();

			for (int i = 0; i < 4; i++)
				GamePads[i].Update();
		} else {
			Keyboard.UpdateNull();
			Mouse.UpdateNull();
			for (int i = 0; i < 4; i++)
				GamePads[i].UpdateNull();
		}
	}

	public static void UpdateNull() {
		Keyboard.UpdateNull();
		Mouse.UpdateNull();
		for (int i = 0; i < 4; i++)
			GamePads[i].UpdateNull();
	}
}
