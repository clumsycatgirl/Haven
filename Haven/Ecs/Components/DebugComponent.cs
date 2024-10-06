
namespace Haven.Ecs.Components;

[Serializable]
[DataContract]
[Component]
public class DebugComponent(bool active) : Component(active, true) {

	public DebugComponent() : this(true) {
	}
}
