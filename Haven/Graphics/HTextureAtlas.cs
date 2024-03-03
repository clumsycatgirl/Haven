
namespace Haven.Graphics;

public class HTextureAtlas {
	public Texture2D Source { get; private set; }
	public Dictionary<string, Rectangle> Regions { get; private set; }

	public HTextureAtlas(Texture2D source, Dictionary<string, Rectangle> regions) {
		Source = source;
		Regions = regions;
	}
}
