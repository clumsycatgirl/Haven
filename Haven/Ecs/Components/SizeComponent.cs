
namespace Haven.Ecs.Components;

[Serializable]
[DataContract]
[Component]
public class SizeComponent(float width, float height) : Component {
	[DataMember]
	public float Width = width;
	[DataMember]
	public float Height = height;

	public SizeComponent() : this(Vector2.Zero) { }
	public SizeComponent(Vector2 size) : this(size.X, size.Y) { }

	public Vector2 ToVector2() {
		return new Vector2(Width, Height);
	}

	public static implicit operator Vector2(SizeComponent component) {
		return component.ToVector2();
	}

	public static implicit operator SizeComponent(Vector2 vector) {
		return new SizeComponent(vector.X, vector.Y);
	}
}
