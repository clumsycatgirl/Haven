
using Haven;
using Haven.Ecs;
using Haven.Ecs.Components;
using Haven.Utils;

using Microsoft.Xna.Framework;

using Myra;
using Myra.Graphics2D.UI;

namespace MapMaker {
	public class MapMaker : Engine {
		public static new MapMaker Instance { get; private set; }

		private readonly Desktop desktop = new();

		public MapMaker() : base() {
			Instance = this;
		}

		protected override void LoadContent() {
			base.LoadContent();

			MyraEnvironment.Game = this;

			desktop.TouchDown += (s, e) => {
				if (desktop.ContextMenu is not null) return;

				VerticalStackPanel container = new() {
					Spacing = 4,
				};

				Panel titleContainer = new() {
					Background = DefaultAssets.DefaultStylesheet.Atlas["button"],
				};

				Label titleLabel = new() {
					Text = "click",
					HorizontalAlignment = HorizontalAlignment.Center,
				};

				titleContainer.Widgets.Add(titleLabel);
				container.Widgets.Add(titleContainer);

				MenuItem menuItem1 = new() {
					Text = "create new map",
				};
				menuItem1.Selected += (s, e) => Log.WriteLine("selected menuItem1");

				MenuItem menuItem2 = new() {
					Text = "open map",
				};
				menuItem2.Selected += (s, e) => Log.WriteLine("selected menuItem2");

				MenuItem menuItem3 = new() {
					Text = "Quit"
				};

				VerticalMenu verticalMenu = new();
				verticalMenu.Items.Add(menuItem1);
				verticalMenu.Items.Add(menuItem2);
				verticalMenu.Items.Add(menuItem3);

				container.Widgets.Add(verticalMenu);

				desktop.ShowContextMenu(container, desktop.TouchPosition ?? Point.Zero);
			};
		}

		protected override void Initialize() {
			base.Initialize();
		}

		protected override void Draw(GameTime gameTime) {
			base.Draw(gameTime);
			desktop.Render();
		}

		protected override void Update(GameTime gameTime) {
			base.Update(gameTime);
		}
	}
}
