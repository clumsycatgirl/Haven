
using System.Threading.Tasks;

namespace Haven.Ecs;

[Serializable]
[DataContract]
public class Scene {
	[DataMember]
	public bool Paused = false;
	public float TimeActive;
	public float RawTimeActive;

	[DataMember]
	public List<Entity> Entities { get; private set; }

	public event Action OnEndOfFrame;

	public Scene() {
		Entities = [];
	}

	public virtual void Begin() {
		Entities.ForEach(e => e.SceneBegin(this));
	}
	public virtual void End() {
		Entities.ForEach(e => e.SceneEnd(this));
	}

	public virtual void BeforeUpdate() {
		if (!Paused) TimeActive += Engine.DeltaTime;
		RawTimeActive += Engine.DeltaTime;

		Entities.ForEach(e => e.BeforeUpdate());
	}
	public virtual void Update() {
		if (Paused) return;
		Entities.ForEach(e => e.Update());
	}
	public virtual void AfterUpdate() {
		OnEndOfFrame?.Invoke();
		OnEndOfFrame = null;

		Entities.ForEach(e => e.AfterUpdate());
	}

	public virtual void BeforeDraw() {
		Entities.ForEach(e => e.BeforeDraw());
	}
	public virtual void Draw() {
		Entities.ForEach(e => e.Draw());
	}
	public virtual void AfterDraw() {
		Entities.ForEach(e => e.AfterDraw());
	}

	public T CreateAndAdd<T>() where T : Entity, new() {
		T entity = new();
		Add(entity);
		return entity;
	}

	public void Add(Entity entity) {
		Entities.Add(entity);
		entity.Added(this);
	}

	public void Remove(Entity entity) {
		Entities.Remove(entity);
		entity.Removed(this);
	}

	public void Add(IEnumerable<Entity> entities) {
		Entities.AddRange(entities);
		foreach (Entity entity in entities) {
			entity.Added(this);
		}
	}

	public void Remove(IEnumerable<Entity> entities) {
		Entities.RemoveAll(e => entities.Contains(e));
		foreach (Entity entity in entities) {
			entity.Removed(this);
		}
	}

	public void Add(params Entity[] entities) {
		Entities.AddRange(entities);
		foreach (Entity entity in entities) {
			entity.Added(this);
		}
	}

	public void Remove(params Entity[] entities) {
		Entities.RemoveAll(e => entities.Contains(e));
		foreach (Entity entity in entities) {
			entity.Removed(this);
		}
	}

	public T GetEntity<T>(Guid id) where T : Entity {
		return Entities.Find(e => e.Id == id) as T;
	}

	public T GetEntity<T>(Predicate<T> predicate) where T : Entity {
		return Entities.Find(e => e is T && predicate(e as T)) as T;
	}

	public T GetEntity<T>() where T : Entity {
		return Entities.Find(e => e is T) as T;
	}

	public IEnumerable<T> GetEntities<T>() where T : Entity {
		return Entities.Where(e => e is T).Cast<T>();
	}

	public IEnumerable<T> GetEntities<T>(Predicate<T> predicate) where T : Entity {
		return Entities.Where(e => e is T && predicate(e as T)).Cast<T>();
	}

	public IEnumerable<T> GetEntitiesOfTag<T>(string tag) where T : Entity {
		return Entities.Where(e => e.Tags.Has(tag)).Cast<T>();
	}
}
