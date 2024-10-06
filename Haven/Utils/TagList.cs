
namespace Haven.Utils;

[Serializable]
[DataContract]
public class TagList : IEnumerable<string>, IEnumerable {
	[DataMember]
	public TagComponent Component { get; }
	[DataMember]
	private readonly List<string> tags = new();

	public int Count => tags.Count;
	public Entity Entity => Component.Entity;

	internal TagList(TagComponent component, params string[] tags) {
		Component = component;
		this.tags.AddRange(tags);
	}

	public void Add(string tag) {
		tags.Add(tag);
	}

	public void Add(IEnumerable<string> tags) {
		foreach (var tag in tags)
			Add(tag);
	}

	public void Add(params string[] tags) {
		foreach (var tag in tags)
			Add(tag);
	}

	public void Remove(string tag) {
		tags.Remove(tag);
	}

	public void Remove(IEnumerable<string> tags) {
		foreach (var tag in tags)
			Remove(tag);
	}

	public void Remove(params string[] tags) {
		foreach (var tag in tags)
			Remove(tag);
	}

	public string this[int index] => index < 0 || index >= tags.Count ? throw new IndexOutOfRangeException() : tags[index];

	public bool Has(string tag) {
		return tags.Contains(tag);
	}

	public IEnumerator<string> GetEnumerator() {
		return tags.GetEnumerator();
	}

	IEnumerator IEnumerable.GetEnumerator() {
		return GetEnumerator();
	}

	public string[] ToArray() {
		return [.. tags];
	}
}
