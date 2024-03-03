
namespace Haven.Inputs;

public class KeyboardData {
	public KeyboardState PreviousState;
	public KeyboardState CurrentState;

	internal KeyboardData() { }

	internal void Update() {
		PreviousState = CurrentState;
		CurrentState = Keyboard.GetState();
	}

	internal void UpdateNull() {
		PreviousState = CurrentState;
		CurrentState = new();
	}

	#region Basic Checks

	public bool Check(Keys key) {
		if (Input.Disabled)
			return false;

		return CurrentState.IsKeyDown(key);
	}

	public bool Pressed(Keys key) {
		if (Input.Disabled)
			return false;

		return CurrentState.IsKeyDown(key) && !PreviousState.IsKeyDown(key);
	}

	public bool Released(Keys key) {
		if (Input.Disabled)
			return false;

		return !CurrentState.IsKeyDown(key) && PreviousState.IsKeyDown(key);
	}

	#endregion Basic Checks

	#region Convenience Checks

	public bool Check(Keys keyA, Keys keyB) {
		return Check(keyA) || Check(keyB);
	}

	public bool Pressed(Keys keyA, Keys keyB) {
		return Pressed(keyA) || Pressed(keyB);
	}

	public bool Released(Keys keyA, Keys keyB) {
		return Released(keyA) || Released(keyB);
	}

	public bool Check(Keys keyA, Keys keyB, Keys keyC) {
		return Check(keyA) || Check(keyB) || Check(keyC);
	}

	public bool Pressed(Keys keyA, Keys keyB, Keys keyC) {
		return Pressed(keyA) || Pressed(keyB) || Pressed(keyC);
	}

	public bool Released(Keys keyA, Keys keyB, Keys keyC) {
		return Released(keyA) || Released(keyB) || Released(keyC);
	}

	#endregion Convenience Checks

	#region Axis

	public int AxisCheck(Keys negative, Keys positive) {
		if (Check(negative)) {
			if (Check(positive))
				return 0;
			else
				return -1;
		} else if (Check(positive))
			return 1;
		else
			return 0;
	}

	public int AxisCheck(Keys negative, Keys positive, int both) {
		if (Check(negative)) {
			if (Check(positive))
				return both;
			else
				return -1;
		} else if (Check(positive))
			return 1;
		else
			return 0;
	}

	#endregion Axis
}
