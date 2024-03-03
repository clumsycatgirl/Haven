
namespace Haven.Utils;

[Serializable]
[DataContract]
public class ComponentList : IEnumerable<Component>, IEnumerable {
	[Serializable]
	public enum LockModes { Open, Locked, Error };

	[DataMember]
	public Entity Entity { get; internal set; }

	[DataMember]
	private readonly List<Component> components;
	[DataMember]
	private readonly List<Component> componentsToAdd;
	[DataMember]
	private readonly List<Component> componentsToRemove;

	[DataMember]
	private readonly HashSet<Component> current;
	[DataMember]
	private readonly HashSet<Component> adding;
	[DataMember]
	private readonly HashSet<Component> removing;

	[DataMember]
	private LockModes lockMode;

	public int Count => components.Count;

	internal ComponentList(Entity entity) {
		Entity = entity;

		lockMode = LockModes.Open;

		components = [];
		componentsToAdd = [];
		componentsToRemove = [];
		current = [];
		adding = [];
		removing = [];
	}

	internal LockModes LockMode {
		get => lockMode;
		set {
			lockMode = value;

			if (componentsToAdd.Count > 0) {
				componentsToAdd.Where(c => !current.Contains(c)).ToList().ForEach(c => {
					current.Add(c);
					components.Add(c);
					c.Added(Entity);
				});

				adding.Clear();
				componentsToAdd.Clear();
			}

			if (componentsToRemove.Count > 0) {
				componentsToRemove.Where(c => !current.Contains(c)).ToList().ForEach(c => {
					current.Remove(c);
					components.Remove(c);
					c.Removed(Entity);
				});

				removing.Clear();
				componentsToRemove.Clear();
			}
		}
	}

	public void EntityAdded(Scene scene) {
		foreach (Component c in components)
			c.EntityAdded(scene);
	}

	public void EntityRemoved(Scene scene) {
		foreach (Component c in components)
			c.EntityRemoved(scene);
	}

	public void SceneBegin(Scene scene) {
		foreach (Component c in components)
			c.SceneBegin(scene);
	}

	public void SceneEnd(Scene scene) {
		foreach (Component c in components)
			c.SceneEnd(scene);
	}

	public void EntityAwake(Scene scene) {
		foreach (Component c in components)
			c.EntityAwake(scene);
	}

	public void Add(Component component) {
		switch (LockMode) {
			case LockModes.Open:
				// if (!current.Contains(component)) {
				if (current.Add(component)) {
					components.Add(component);
					component.Added(Entity);
				}
				break;

			case LockModes.Locked:
				if (!current.Contains(component) && !adding.Contains(component)) {
					adding.Add(component);
					componentsToAdd.Add(component);
				}
				break;

			case LockModes.Error:
				throw new Exception("Cannot add Entities");
		}
	}

	public void Remove(Component component) {
		switch (LockMode) {
			case LockModes.Open:
				// if (current.Contains(component)) {
				if (current.Remove(component)) {
					components.Remove(component);
					component.Removed(Entity);
				}
				break;

			case LockModes.Locked:
				if (current.Contains(component) && !removing.Contains(component)) {
					removing.Add(component);
					componentsToRemove.Add(component);
				}
				break;

			case LockModes.Error:
				throw new Exception("Cannot remove Entities");
		}
	}

	public void Add(IEnumerable<Component> components) {
		foreach (var component in components)
			Add(component);
	}

	public void Remove(IEnumerable<Component> components) {
		foreach (var component in components)
			Remove(component);
	}

	public void RemoveAll<T>() where T : Component {
		Remove(GetAll<T>());
	}

	public void Add(params Component[] components) {
		foreach (var component in components)
			Add(component);
	}

	public void Remove(params Component[] components) {
		foreach (var component in components)
			Remove(component);
	}

	public Component this[int index] => index < 0 || index >= components.Count ? throw new IndexOutOfRangeException() : components[index];

	public IEnumerator<Component> GetEnumerator() {
		return components.GetEnumerator();
	}

	IEnumerator IEnumerable.GetEnumerator() {
		return GetEnumerator();
	}

	public Component[] ToArray() {
		return [.. components];
	}

	public List<Component> ToList() {
		return components;
	}

	internal void BeforeUpdate() {
		LockMode = LockModes.Locked;
		foreach (Component component in components) {
			if (component.Active) {
				component.BeforeUpdate();
			}
		}
		LockMode = LockModes.Open;
	}

	internal void Update() {
		LockMode = LockModes.Locked;
		foreach (Component component in components) {
			if (component.Active) {
				component.Update();
			}
		}
		LockMode = LockModes.Open;
	}

	internal void AfterUpdate() {
		LockMode = LockModes.Locked;
		foreach (Component component in components) {
			if (component.Active) {
				component.AfterUpdate();
			}
		}
		LockMode = LockModes.Open;
	}

	internal void BeforeDraw() {
		LockMode = LockModes.Locked;
		foreach (Component component in components) {
			if (component.Visible) {
				component.BeforeDraw();
			}
		}
		LockMode = LockModes.Open;
	}

	internal void Draw() {
		LockMode = LockModes.Locked;
		foreach (Component component in components) {
			if (component.Visible) {
				component.Draw();
			}
		}
		LockMode = LockModes.Open;
	}

	internal void AfterDraw() {
		LockMode = LockModes.Locked;
		foreach (Component component in components) {
			if (component.Visible) {
				component.AfterDraw();
			}
		}
		LockMode = LockModes.Open;
	}

	public T Get<T>() where T : Component {
		return components.OfType<T>().FirstOrDefault();
	}

	public IEnumerable<T> GetAll<T>() where T : Component {
		foreach (Component component in components) {
			if (component is T) {
				yield return component as T;
			}
		}
	}

	public void ForEach(Action<Component> action) {
		components.ForEach(action);
	}
}
