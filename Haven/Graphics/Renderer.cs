
namespace Haven.Graphics;

public static class Renderer {
	public static SpriteBatch SpriteBatch { get; private set; }
	public static Texture2D Pixel { get; private set; }

	public static void Initialize(GraphicsDevice graphicsDevice) {
		SpriteBatch = new(graphicsDevice);
		Pixel = new(graphicsDevice, 1, 1);
		Pixel.SetData(new Color[] { Color.White });
	}

	public static void DrawBegin() {
		SpriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.LinearClamp, DepthStencilState.None, RasterizerState.CullNone, null);
	}

	public static void DrawRectangle(Vector2 position, int width, int height, Color color, float rotation = 0.0f) {
		SpriteBatch.Draw(Pixel, position, null, color, rotation, Vector2.Zero, new Vector2(width, height), SpriteEffects.None, 0f);
	}

	public static void DrawEnd() {
		SpriteBatch.End();
	}
}
