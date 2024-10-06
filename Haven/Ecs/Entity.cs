
namespace Haven.Ecs;

[Serializable]
[DataContract]
[Entity]
public class Entity {
	[DataMember]
	public bool Active;
	[DataMember]
	public bool Visible;

	[DataMember]
	public Scene Scene { get; private set; }
	[DataMember]
	public ComponentList Components { get; private set; }

	public Entity() : this(Vector2.Zero, Vector2.Zero) { }
	public Entity(Vector2 position) : this(position, Vector2.Zero) { }
	public Entity(Vector2 position, Vector2 size) {
		Components = new ComponentList(this);

		Add([
			new IdComponent(),
			new TagComponent(),
			new TransformComponent(position),
			new SizeComponent(size),
		]);
	}

	public Entity(float x, float y) : this(new Vector2(x, y), Vector2.Zero) { }
	public Entity(float x, float y, float width, float height) : this(new Vector2(x, y), new Vector2(width, height)) { }


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
	public Guid Id => Get<IdComponent>().Id;
	public Utils.TagList Tags => Get<TagComponent>().Tags;
}
