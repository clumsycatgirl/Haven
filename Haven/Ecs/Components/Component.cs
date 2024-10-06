
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
#if DEBUG
		RequireComponentAttribute dependencies = GetType().GetCustomAttribute<RequireComponentAttribute>();
		if (dependencies is not null) {
			MethodInfo getComponent = entity.Components.GetType().GetMethod("Get");
			foreach (Type dependency in dependencies.RequiredComponents) {
				MethodInfo method = getComponent.MakeGenericMethod(dependency);
				if (method.Invoke(entity.Components, null) is null) {
					throw new RequiredComponentException(GetType(), dependency);
				}
			}
		}
#endif

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

