
namespace Haven.Ecs.Components;

[Serializable]
[DataContract]
[Component]
public class Component(bool active, bool visible) {
	[DataMember]
	public bool Active = active;
	[DataMember]
	public bool Visible = visible;
	[DataMember]
	public Entity Entity { get; private set; }
	public Scene Scene => Entity.Scene;

	public Component() : this(true, true) { }

	public virtual void Added(Entity entity) {
		Entity = entity;
	}

	public virtual void Removed(Entity entity) {
		Entity = null;
	}

	public virtual void EntityAdded(Scene scene) { }
	public virtual void EntityRemoved(Scene scene) { }

	public virtual void SceneBegin(Scene scene) { }
	public virtual void SceneEnd(Scene scene) { }

	public virtual void EntityAwake(Scene scene) { }

	public virtual void BeforeUpdate() { }
	public virtual void Update() { }
	public virtual void AfterUpdate() { }

	public virtual void BeforeDraw() { }
	public virtual void Draw() { }
	public virtual void AfterDraw() { }

	public void RemoveSelf() {
		Entity?.Remove(this);
	}

	public T SceneAs<T>() where T : Scene => Scene as T;
	public T EntityAs<T>() where T : Entity => Entity as T;
}
