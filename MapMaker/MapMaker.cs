
using System.Collections.Generic;

using Haven;
using Haven.Ecs;
using Haven.Ecs.Components;
using Haven.Graphics;
using Haven.Utils;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Myra;
using Myra.Graphics2D.TextureAtlases;
using Myra.Graphics2D.UI;

namespace MapMaker {
	public class MapMaker : Engine {
		public static new MapMaker Instance { get; private set; }

		private Desktop desktop;
		internal class MapData {
			internal HTexture2D[,] Textures;
			internal Grid Grid;
		}
		private readonly Dictionary<string, MapData> mapsData;
		private string MapName = "";

		public MapMaker() : base() {
			Instance = this;

			mapsData = new();
		}

		protected override void LoadContent() {
			base.LoadContent();

			MyraEnvironment.Game = this;

			desktop = new();

			Grid grid = new() {
				RowSpacing = 8,
				ColumnSpacing = 8,
				Scale = new Vector2(2.0f, 2.0f),
			};

			for (int i = 0; i < 3; i++) {
				grid.RowsProportions.Add(new Proportion(ProportionType.Pixels, 25));
				grid.ColumnsProportions.Add(new Proportion(ProportionType.Pixels, 90));
			}

			Label nameLabel = new() { Text = "Map Name" };
			TextBox nameBox = new();
			Grid.SetRow(nameLabel, 0);
			Grid.SetColumn(nameLabel, 0);
			grid.Widgets.Add(nameLabel);
			Grid.SetRow(nameBox, 0);
			Grid.SetColumn(nameBox, 1);
			grid.Widgets.Add(nameBox);

			Label widthLabel = new() { Text = "Width" };
			TextBox widthBox = new();
			Grid.SetRow(widthLabel, 1);
			Grid.SetColumn(widthLabel, 0);
			grid.Widgets.Add(widthLabel);
			Grid.SetRow(widthBox, 1);
			Grid.SetColumn(widthBox, 1);
			grid.Widgets.Add(widthBox);

			Label heightLabel = new() { Text = "Height" };
			TextBox heightBox = new();
			Grid.SetRow(heightLabel, 2);
			Grid.SetColumn(heightLabel, 0);
			grid.Widgets.Add(heightLabel);
			Grid.SetRow(heightBox, 2);
			Grid.SetColumn(heightBox, 1);
			grid.Widgets.Add(heightBox);

			Button createMapButton = new() { Content = new Label { Text = "Create Map" } };
			createMapButton.Click += (s, a) => {
				string name = nameBox.Text ?? "test";
				uint width = uint.Parse(widthBox.Text ?? "10");
				uint height = uint.Parse(heightBox.Text ?? "10");

				CreateGrid(name, width, height);
			};
			Grid.SetRow(createMapButton, 3);
			Grid.SetColumn(createMapButton, 2);
			grid.Widgets.Add(createMapButton);

			grid.VerticalAlignment = VerticalAlignment.Center;
			grid.HorizontalAlignment = HorizontalAlignment.Center;

			desktop.Root = grid;
		}

		private void CreateGrid(string name, uint width, uint height) {
			if (mapsData.ContainsKey(name)) {
				goto end;
			}

			MapData mapData = new() {
				Textures = new HTexture2D[width, height],
				Grid = new() {
					ShowGridLines = false,
				}
			};
			mapsData[name] = mapData;

			for (int i = 0; i < height; i++) {
				mapData.Grid.RowsProportions.Add(new Proportion(ProportionType.Auto));
			}

			for (int i = 0; i < width; i++) {
				mapData.Grid.ColumnsProportions.Add(new Proportion(ProportionType.Auto));
			}

			for (int y = 0; y < height; y++) {
				for (int x = 0; x < width; x++) {
					Button button = new() {
						Content = new Image {
							Renderable = new TextureRegion(
								ContentLoader.GetTexture2d("water-1").Texture,
								ContentLoader.GetTexture2d("water-1").ClipRect
							)
						}
					};
					Grid.SetRow(button, y);
					Grid.SetColumn(button, x);
					button.Click += (s, a) => {
						Log.WriteLine($"clicked: [{x},{y}]");
					};
					mapData.Grid.Widgets.Add(button);
				}
			}

		end:
			MapName = name;

			Grid outerLayoutGrid = new() {
				RowSpacing = 4,
				ColumnSpacing = 4,
				ShowGridLines = true,
			};
			outerLayoutGrid.RowsProportions.Add(new Proportion(ProportionType.Pixels, 45));
			outerLayoutGrid.RowsProportions.Add(new Proportion(ProportionType.Fill));
			outerLayoutGrid.RowsProportions.Add(new Proportion(ProportionType.Pixels, 45));
			outerLayoutGrid.ColumnsProportions.Add(new Proportion(ProportionType.Fill));

			Label titleLabel = new() {
				Text = "Map Maker",
				Scale = new Vector2(2.0f, 2.0f),
				VerticalAlignment = VerticalAlignment.Center,
				HorizontalAlignment = HorizontalAlignment.Center,
			};
			outerLayoutGrid.Widgets.Add(titleLabel);
			Label lowerLabel = new() {
				Text = "v.0.1",
				VerticalAlignment = VerticalAlignment.Center,
				Left = 10,
			};
			Grid.SetRow(lowerLabel, 2);
			Grid.SetColumn(lowerLabel, 0);
			outerLayoutGrid.Widgets.Add(lowerLabel);

			Grid innerLayoutGrid = new() {
				RowSpacing = 4,
				ColumnSpacing = 4,
				ShowGridLines = true,
			};
			innerLayoutGrid.ColumnsProportions.Add(new Proportion(ProportionType.Pixels, 250));
			innerLayoutGrid.ColumnsProportions.Add(new Proportion(ProportionType.Fill));
			innerLayoutGrid.ColumnsProportions.Add(new Proportion(ProportionType.Pixels, 45));

			CreateCommandsPanel(innerLayoutGrid);

			Grid.SetRow(innerLayoutGrid, 1);
			Grid.SetColumn(innerLayoutGrid, 0);
			outerLayoutGrid.Widgets.Add(innerLayoutGrid);

			Grid.SetRow(mapsData[name].Grid, 0);
			Grid.SetColumn(mapsData[name].Grid, 1);
			innerLayoutGrid.Widgets.Add(mapsData[name].Grid);

			desktop.Root = outerLayoutGrid;
		}

		private void CreateCommandsPanel(Grid parent) {
			VerticalStackPanel panel = new() {
				Left = 8,
				Top = 8,
				Spacing = 8,
			};

			Button button = new() {
				Content = new Label { Text = "show grid lines" },
				HorizontalAlignment = HorizontalAlignment.Left,
			};
			button.Click += (s, a) => mapsData[MapName].Grid.ShowGridLines = !mapsData[MapName].Grid.ShowGridLines;

			panel.Widgets.Add(button);

			Dictionary<string, HTexture2D> textures = ContentLoader.GetTextures2D();
			panel.Widgets.Add(new HorizontalSeparator());
			foreach (KeyValuePair<string, HTexture2D> kvp in textures) {
				Button textureButton = new() {
					Content = new Image {
						Renderable = new TextureRegion(
							kvp.Value.Texture,
							kvp.Value.ClipRect
						),
					},
					Scale = new Vector2(2.5f, 2.5f),
					VerticalAlignment = VerticalAlignment.Center,
					HorizontalAlignment = HorizontalAlignment.Center,
				};
				textureButton.Click += (s, a) => {
					foreach (Widget widget in mapsData[MapName].Grid.Widgets) {
						if (widget is Button b) {
							b.Content = new Image {
								Renderable = new TextureRegion(
									kvp.Value.Texture,
									kvp.Value.ClipRect
								)
							};
						}
					}
				};

				Grid textureGrid = new() {
					Tooltip = kvp.Key,
				};
				textureGrid.RowsProportions.Add(new Proportion(ProportionType.Pixels, 45));
				textureGrid.ColumnsProportions.Add(new Proportion(ProportionType.Pixels, 45));
				textureGrid.ColumnsProportions.Add(new Proportion(ProportionType.Fill));

				Grid.SetColumn(textureButton, 0);
				Grid.SetRow(textureButton, 0);

				Label textureLabel = new() { Text = kvp.Key, VerticalAlignment = VerticalAlignment.Center };
				Grid.SetColumn(textureLabel, 1);
				Grid.SetRow(textureLabel, 0);

				textureGrid.Widgets.Add(textureButton);
				textureGrid.Widgets.Add(textureLabel);

				panel.Widgets.Add(textureGrid);
				panel.Widgets.Add(new HorizontalSeparator());
			}

			Grid.SetRow(parent, 0);
			Grid.SetColumn(parent, 0);
			parent.Widgets.Add(panel);
		}

		protected override void Initialize() {
			base.Initialize();

			Window.AllowUserResizing = true;
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
