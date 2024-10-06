
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

	public TransformComponent() : this(Vector2.Zero) { }

	public TransformComponent(Vector2 position) : this(position, 0.0f) { }

	public TransformComponent(Vector2 position, float rotation) : this(position, rotation, Vector2.One) { }
}
