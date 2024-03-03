
namespace Haven.Ecs;

[Serializable]
[DataContract]
[Entity]
[RequireComponent(typeof(TransformComponent), typeof(SizeComponent), typeof(SpriteComponent))]
public class Entity {
	[DataMember]
	public bool Active;
	[DataMember]
	public bool Visible;

	[DataMember]
	public Scene Scene { get; private set; }
	[DataMember]
	public ComponentList Components { get; private set; }

	public Entity(Vector2 position) {
		Components = new ComponentList(this);

		Add([
			new TransformComponent(position),
			new SizeComponent(),
		]);
	}

	public Entity() : this(Vector2.Zero) { }

	public virtual void Added(Scene scene) {
		Scene = scene;
		Components.EntityAdded(scene);
	}

	public virtual void Removed(Scene scene) {
		Scene = null;
		Components.EntityRemoved(scene);
	}

	public virtual void SceneBegin(Scene scene) {
		Components.SceneBegin(scene);
	}

	public virtual void SceneEnd(Scene scene) {
		Components.SceneEnd(scene);
	}

	public virtual void Awake(Scene scene) {
		Components.EntityAwake(scene);
	}

	public virtual void BeforeUpdate() {
		Components.BeforeUpdate();
	}

	public virtual void Update() {
		Components.Update();
	}

	public virtual void AfterUpdate() {
		Components.AfterUpdate();
	}

	public virtual void BeforeDraw() {
		Components.BeforeDraw();
	}

	public virtual void Draw() {
		Components.Draw();
	}

	public virtual void AfterDraw() {
		Components.AfterDraw();
	}

	public void RemoveSelf() {
		Scene?.Remove(this);
	}

	public void Add(Component component) {
		Components.Add(component);
	}

	public void Remove(Component component) {
		Components.Remove(component);
	}

	public void Add(params Component[] components) {
		Components.Add(components);
	}

	public void Remove(params Component[] components) {
		Components.Remove(components);
	}

	public T Get<T>() where T : Component {
		return Components.Get<T>();
	}

	public TransformComponent Transform => Get<TransformComponent>();
	public SizeComponent Size => Get<SizeComponent>();
}
