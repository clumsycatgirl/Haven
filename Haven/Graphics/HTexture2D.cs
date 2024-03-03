
namespace Haven.Graphics;
public class HTexture2D {
	public Texture2D Texture { get; private set; }
	public Rectangle ClipRect { get; private set; }
	public string AtlasPath { get; private set; }

	public int Width => ClipRect.Width;
	public int Height => ClipRect.Height;

	public HTexture2D() { }

	public HTexture2D(string atlasKey, string textureKey) {
		HTextureAtlas atlas = ContentLoader.TextureAtlases[atlasKey];
		Texture = atlas.Source;
		ClipRect = atlas.Regions[textureKey];
	}

	public HTexture2D(Texture2D texture) {
		Texture = texture;
		AtlasPath = null;
		ClipRect = new(0, 0, texture.Width, texture.Height);
	}

	public HTexture2D(HTexture2D parent, int x, int y, int width, int height) {
		Texture = parent.Texture;
		AtlasPath = null;
		ClipRect = new(x, y, width, height);
	}

	public HTexture2D(HTexture2D parent, Rectangle clipRect)
		: this(parent, clipRect.X, clipRect.Y, clipRect.Width, clipRect.Height) { }

	public HTexture2D(HTextureAtlas parent, string atlasPath, Rectangle clipRect) {
		Texture = parent.Source;
		AtlasPath = atlasPath;
		ClipRect = clipRect;
	}

	public HTexture2D(int width, int height, Color color) {
		Texture = new(Engine.Instance.GraphicsDevice, width, height);
		Color[] colors = Enumerable.Repeat<Color>(color, width * height).ToArray();
		Texture.SetData(colors);

		AtlasPath = null;
		ClipRect = new Rectangle(0, 0, width, height);
	}

	public virtual void Draw(Vector2 position) {
#if DEBUG
		if (Texture.IsDisposed)
			throw new Exception("Texture2D is Disposed");
#endif
		Renderer.SpriteBatch.Draw(Texture, position, ClipRect, Color.White);
	}

	// 	public virtual void Draw(Vector2 position, Vector2 origin) {
	// #if DEBUG
	// 		if (Texture.IsDisposed)
	// 			throw new Exception("Texture2D is Disposed");
	// #endif
	// 		Renderer.SpriteBatch.Draw(Texture, position, ClipRect, Color.White, 0f, origin, 1f, SpriteEffects.None, 0f);
	// 	}

	// 	public virtual void Draw(Vector2 position, Vector2 origin, Color color) {
	// #if DEBUG
	// 		if (Texture.IsDisposed)
	// 			throw new Exception("Texture2D is Disposed");
	// #endif
	// 		Renderer.SpriteBatch.Draw(Texture, position, ClipRect, color, 0, origin, 1f, SpriteEffects.None, 0f);
	// 	}

	// 	public virtual void Draw(Vector2 position, Vector2 origin, Color color, float scale) {
	// #if DEBUG
	// 		if (Texture.IsDisposed)
	// 			throw new Exception("Texture2D is Disposed");
	// #endif
	// 		Renderer.SpriteBatch.Draw(Texture, position, ClipRect, color, 0, origin, scale, SpriteEffects.None, 0f);
	// 	}

	// 	public virtual void Draw(Vector2 position, Vector2 origin, Color color, float scale, float rotation) {
	// #if DEBUG
	// 		if (Texture.IsDisposed)
	// 			throw new Exception("Texture2D is Disposed");
	// #endif
	// 		Renderer.SpriteBatch.Draw(Texture, position, ClipRect, color, rotation, origin, scale, SpriteEffects.None, 0f);
	// 	}

	// 	public virtual void Draw(Vector2 position, Vector2 origin, Color color, float scale, float rotation, SpriteEffects flip) {
	// #if DEBUG
	// 		if (Texture.IsDisposed)
	// 			throw new Exception("Texture2D is Disposed");
	// #endif
	// 		Renderer.SpriteBatch.Draw(Texture, position, ClipRect, color, rotation, origin, scale, flip, 0f);
	// 	}

	// 	public virtual void Draw(Vector2 position, Vector2 origin, Color color, Vector2 scale) {
	// #if DEBUG
	// 		if (Texture.IsDisposed)
	// 			throw new Exception("Texture2D is Disposed");
	// #endif
	// 		Renderer.SpriteBatch.Draw(Texture, position, ClipRect, color, 0, origin, scale, SpriteEffects.None, 0f);
	// 	}

	// 	public virtual void Draw(Vector2 position, Vector2 origin, Color color, Vector2 scale, float rotation) {
	// #if DEBUG
	// 		if (Texture.IsDisposed)
	// 			throw new Exception("Texture2D is Disposed");
	// #endif
	// 		Renderer.SpriteBatch.Draw(Texture, position, ClipRect, color, rotation, origin, scale, SpriteEffects.None, 0f);
	// 	}

	// 	public virtual void Draw(Vector2 position, Vector2 origin, Color color, Vector2 scale, float rotation, SpriteEffects flip) {
	// #if DEBUG
	// 		if (Texture.IsDisposed)
	// 			throw new Exception("Texture2D is Disposed");
	// #endif
	// 		Renderer.SpriteBatch.Draw(Texture, position, ClipRect, color, rotation, origin, scale, flip, 0f);
	// 	}

	//

	public virtual void Draw(Vector2 position, Vector2 size) {
#if DEBUG
		if (Texture.IsDisposed)
			throw new Exception("Texture2D is Disposed");
#endif
		Rectangle destinationRectangle = new((int)position.X, (int)position.Y, (int)size.X, (int)size.Y);
		Renderer.SpriteBatch.Draw(Texture, destinationRectangle, ClipRect, Color.White);
	}

	public virtual void Draw(Vector2 position, Vector2 size, Vector2 origin) {
#if DEBUG
		if (Texture.IsDisposed)
			throw new Exception("Texture2D is Disposed");
#endif
		Rectangle destinationRectangle = new((int)position.X, (int)position.Y, (int)size.X, (int)size.Y);
		Renderer.SpriteBatch.Draw(Texture, destinationRectangle, ClipRect, Color.White, 0f, origin, SpriteEffects.None, 0f);
	}

	public virtual void Draw(Vector2 position, Vector2 size, Vector2 origin, Color color) {
#if DEBUG
		if (Texture.IsDisposed)
			throw new Exception("Texture2D is Disposed");
#endif
		Rectangle destinationRectangle = new((int)position.X, (int)position.Y, (int)size.X, (int)size.Y);
		Renderer.SpriteBatch.Draw(Texture, destinationRectangle, ClipRect, color, 0f, origin, SpriteEffects.None, 0f);
	}

	public virtual void Draw(Vector2 position, Vector2 size, Vector2 origin, Color color, float rotation) {
#if DEBUG
		if (Texture.IsDisposed)
			throw new Exception("Texture2D is Disposed");
#endif
		Rectangle destinationRectangle = new((int)position.X, (int)position.Y, (int)size.X, (int)size.Y);
		Renderer.SpriteBatch.Draw(Texture, destinationRectangle, ClipRect, color, rotation, origin, SpriteEffects.None, 0f);
	}

	public virtual void Draw(Vector2 position, Vector2 size, Vector2 origin, Color color, float rotation, Vector2 scale) {
#if DEBUG
		if (Texture.IsDisposed)
			throw new Exception("Texture2D is Disposed");
#endif
		Rectangle destinationRectangle = new((int)position.X, (int)position.Y, (int)MathF.Round(size.X * scale.X), (int)MathF.Round(size.Y * scale.Y));
		Renderer.SpriteBatch.Draw(Texture, destinationRectangle, ClipRect, color, rotation, origin, SpriteEffects.None, 0f);
	}

	public virtual void Draw(Vector2 position, Vector2 size, Vector2 origin, Color color, float rotation, Vector2 scale, SpriteEffects flip) {
#if DEBUG
		if (Texture.IsDisposed)
			throw new Exception("Texture2D is Disposed");
#endif
		Rectangle destinationRectangle = new((int)position.X, (int)position.Y, (int)MathF.Round(size.X), (int)MathF.Round(size.Y));
		Renderer.SpriteBatch.Draw(Texture, destinationRectangle, ClipRect, color, rotation, origin, SpriteEffects.None, 0f);
	}
}
