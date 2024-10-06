
namespace Haven.Ecs.Components;

[Serializable]
[DataContract]
[Component]
[RequireComponentAttribute(typeof(TransformComponent), typeof(SizeComponent))]
public class SpriteComponent(bool active) : Component(active, true) {
	[DataMember]
	public HTexture2D Texture = null;
	[DataMember]
	public Vector2 Origin;
	[DataMember]
	public Color Color = Color.White;
	[DataMember]
	public SpriteEffects Effects;

	public SpriteComponent(string atlas, string texture) : this(true) {
		Texture = new HTexture2D(atlas, texture);
	}

	public SpriteComponent(HTexture2D texture) : this(true) {
		Texture = texture;
	}

	public override void Draw() {
		AnimationComponent anim = Entity.Get<AnimationComponent>();
		if (anim is not null && anim.Frames.Count > 0) {
			anim.CurrentFrame.Draw(
				Entity.Transform.Position,
				Entity.Size,
				Vector2.Zero,
				Color,
				Entity.Transform.Rotation,
				Entity.Transform.Scale,
				Effects
			);
			// anim.CurrentFrame.Draw(
			// 	new(Entity.Transform.Position.X + Entity.Size.Width - 4, Entity.Transform.Position.Y),
			// 	Entity.Size,
			// 	Vector2.Zero,
			// 	Color,
			// 	Entity.Transform.Rotation,
			// 	Entity.Transform.Scale,
			// 	Effects
			// );
			// anim.CurrentFrame.Draw(
			// 	new(Entity.Transform.Position.X, Entity.Transform.Position.Y - Entity.Size.Height + 4),
			// 	Entity.Size,
			// 	Vector2.Zero,
			// 	Color,
			// 	Entity.Transform.Rotation,
			// 	Entity.Transform.Scale,
			// 	Effects
			// );
			// anim.CurrentFrame.Draw(
			// 	new(Entity.Transform.Position.X + Entity.Size.Width - 4, Entity.Transform.Position.Y - Entity.Size.Height + 4),
			// 	Entity.Size,
			// 	Vector2.Zero,
			// 	Color,
			// 	Entity.Transform.Rotation,
			// 	Entity.Transform.Scale,
			// 	Effects
			// );
		} else if (Texture is not null) {
			Texture.Draw(
				Entity.Transform.Position,
				Entity.Size,
				Vector2.Zero,
				Color,
				Entity.Transform.Rotation,
				Entity.Transform.Scale,
				Effects
			);
		} else {
			Renderer.DrawRectangle(Entity.Transform.Position, (int)Entity.Size.Width, (int)Entity.Size.Height, Color.Purple, Entity.Transform.Rotation);
		}
	}
}
