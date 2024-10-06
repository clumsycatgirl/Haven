
namespace Haven.Ecs.Components;

[Serializable]
[DataContract]
[Component]
public class IdComponent : Component {
	[DataMember]
	public Guid Id { get; init; }

	public IdComponent() : this(Guid.NewGuid()) { }

	public IdComponent(Guid id) {
		Id = id;
	}
}
