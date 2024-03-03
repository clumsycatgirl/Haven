
using System;

namespace Haven.Ecs.Components;

[Serializable]
[DataContract]
[Component]
public class TransformComponent(Vector2 position, float rotation, Vector2 scale) : Component(true, false) {
	[DataMember]
	public Vector2 Position = position;
	[DataMember]
	public float Rotation = rotation;
	[DataMember]
	public Vector2 Scale = scale;

	public TransformComponent() : this(Vector2.Zero, 0.0f, Vector2.One) { }

	public TransformComponent(Vector2 position) : this() {
		Position = position;
	}
}
