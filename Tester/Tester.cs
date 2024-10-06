
using Haven;
using Haven.Ecs;
using Haven.Ecs.Components;
using Haven.Utils;

using Microsoft.Xna.Framework;

using Tester.Components;

namespace Tester;

public class Tester : Engine {
	public Entity Entity;
	public Entity Obstacle;

	public static new Tester Instance { get; private set; }

	public Tester() : base() {
		Instance = this;
	}

	protected override void LoadContent() {
		base.LoadContent();
	}

	protected override void Initialize() {
		base.Initialize();

		Entity = Scene.CreateAndAdd<Entity>();
		Entity.Add([
			new AnimationComponent([
				ContentLoader.GetTexture2d("water-1"),
				ContentLoader.GetTexture2d("water-2")],
				.75f),
			new SpriteComponent(ContentLoader.GetTexture2d("dirt-1")),
			new DebugComponent(true),
			new MovementComponent(),
		]);
		Entity.Size.Width = Entity.Size.Height = 16 * 4;

		Obstacle = Scene.CreateAndAdd<Entity>();
		Obstacle.Add([new SpriteComponent(ContentLoader.GetTexture2d("stone-1"))]);
		Obstacle.Transform.Position = new(10, Graphics.PreferredBackBufferHeight - 50);
		Obstacle.Size.Width = Graphics.PreferredBackBufferWidth - 20;
		Obstacle.Size.Height = 5;
		Obstacle.Tags.Add("floor");
	}

	protected override void Draw(GameTime gameTime) {
		try {
			base.Draw(gameTime);
		} catch (System.Exception e) { Log.WriteLine(e.ToString()); }
	}

	protected override void Update(GameTime gameTime) {
		base.Update(gameTime);
	}
}
