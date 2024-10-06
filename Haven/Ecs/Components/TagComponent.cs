
namespace Haven.Ecs.Components;

using Utils;

[Serializable]
[DataContract]
[Component]
public class TagComponent : Component {
	[DataMember]
	public readonly TagList Tags;

	public TagComponent() : this([]) { }
	public TagComponent(params string[] tags) {
		Tags = new(this, tags);
	}

	public void AddTag(string tag) {
		Tags.Add(tag);
	}

	public void RemoveTag(string tag) {
		Tags.Remove(tag);
	}

	public bool HasTag(string tag) {
		return Tags.Contains(tag);
	}
}
