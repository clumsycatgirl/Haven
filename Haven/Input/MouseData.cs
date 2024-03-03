
namespace Haven.Inputs;

public class MouseData {
	public MouseState PreviousState;
	public MouseState CurrentState;

	internal MouseData() {
		PreviousState = new();
		CurrentState = new();
	}

	internal void Update() {
		PreviousState = CurrentState;
		CurrentState = Microsoft.Xna.Framework.Input.Mouse.GetState();
	}

	internal void UpdateNull() {
		PreviousState = CurrentState;
		CurrentState = new();
	}

	#region Buttons

	public bool CheckLeftButton {
		get { return CurrentState.LeftButton == ButtonState.Pressed; }
	}

	public bool CheckRightButton {
		get { return CurrentState.RightButton == ButtonState.Pressed; }
	}

	public bool CheckMiddleButton {
		get { return CurrentState.MiddleButton == ButtonState.Pressed; }
	}

	public bool PressedLeftButton {
		get { return CurrentState.LeftButton == ButtonState.Pressed && PreviousState.LeftButton == ButtonState.Released; }
	}

	public bool PressedRightButton {
		get { return CurrentState.RightButton == ButtonState.Pressed && PreviousState.RightButton == ButtonState.Released; }
	}

	public bool PressedMiddleButton {
		get { return CurrentState.MiddleButton == ButtonState.Pressed && PreviousState.MiddleButton == ButtonState.Released; }
	}

	public bool ReleasedLeftButton {
		get { return CurrentState.LeftButton == ButtonState.Released && PreviousState.LeftButton == ButtonState.Pressed; }
	}

	public bool ReleasedRightButton {
		get { return CurrentState.RightButton == ButtonState.Released && PreviousState.RightButton == ButtonState.Pressed; }
	}

	public bool ReleasedMiddleButton {
		get { return CurrentState.MiddleButton == ButtonState.Released && PreviousState.MiddleButton == ButtonState.Pressed; }
	}

	#endregion Buttons

	#region Wheel

	public int Wheel {
		get { return CurrentState.ScrollWheelValue; }
	}

	public int WheelDelta {
		get { return CurrentState.ScrollWheelValue - PreviousState.ScrollWheelValue; }
	}

	#endregion Wheel

	#region Position

	public bool WasMoved {
		get {
			return CurrentState.X != PreviousState.X
				|| CurrentState.Y != PreviousState.Y;
		}
	}

	public float X {
		get { return Position.X; }
		set { Position = new Vector2(value, Position.Y); }
	}

	public float Y {
		get { return Position.Y; }
		set { Position = new Vector2(Position.X, value); }
	}

	public Vector2 Position {
		get {
			//return Vector2.Transform(new Vector2(CurrentState.X, CurrentState.Y), Matrix.Invert(Engine.ScreenMatrix));
			return new Vector2(CurrentState.X, CurrentState.Y);
		}

		set {
			//var vector = Vector2.Transform(value, Engine.ScreenMatrix);
			Vector2 vector = value;
			Microsoft.Xna.Framework.Input.Mouse.SetPosition((int)Math.Round(vector.X), (int)Math.Round(vector.Y));
		}
	}

	#endregion Position
}
