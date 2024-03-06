
namespace Haven.Utils;

public static class ContentLoader {
	public class AtlasSheetMeta {
		public List<FrameData> Frames { get; set; }

		public class FrameData {
			public string Filename { get; set; }
			public Rectangle Frame { get; set; }
		}
	}

	public static Dictionary<string, HTextureAtlas> TextureAtlases { get; private set; } = new();

	public static HTexture2D GetTexture2d(string textureName) {
		foreach (KeyValuePair<string, HTextureAtlas> atlasPair in TextureAtlases) {
			if (!atlasPair.Value.Regions.ContainsKey(textureName))
				continue;

			return new HTexture2D(atlasPair.Key, textureName);
		}

		return null;
	}

	public static Dictionary<string, HTexture2D> GetTextures2D() {
		return TextureAtlases
				.SelectMany(atlasPair => atlasPair.Value.Regions.Keys,
							(atlasPair, regionName) => new { atlasPair.Key, RegionName = regionName })
				.ToDictionary(x => x.RegionName, x => new HTexture2D(x.Key, x.RegionName));
	}

	public static void LoadContent() {
		Log.WriteLine($"--- [Loading Textures] ---");

		string texturesDirectory = Path.Combine(Core.AssetsDirectory, "Textures/output");
		string[] imageFiles = Directory.GetFiles(texturesDirectory, "*.png")
			.Concat(Directory.GetFiles(texturesDirectory, "*.jpg"))
			.Select(Path.GetFileNameWithoutExtension)
			.Distinct()
			.ToArray();

		foreach (string file in imageFiles) {
			Log.WriteLine($" - {file}");
			string jsonFile = Path.Combine(texturesDirectory, file + ".json");

			Texture2D texture;
			try {
				texture = LoadTexture2DFromFile(file);
			} catch (Exception e) {
				Log.WriteLine(e.ToString());
				continue;
			}

			try {
				Log.WriteErrorIf(!File.Exists(jsonFile), "could not load 'json' data file for atlas");
			} catch {
				continue;
			}

			AtlasSheetMeta meta = LoadTextureAtlasMeta(jsonFile);

			Dictionary<string, Rectangle> regions = meta.Frames.ToDictionary(
				frame => frame.Filename,
				frame => frame.Frame);

			foreach (KeyValuePair<string, Rectangle> kvp in regions) {
				Log.WriteLine($"    - '{kvp.Key}'");
				Log.WriteLine($"        - x: {kvp.Value.X}");
				Log.WriteLine($"        - y: {kvp.Value.Y}");
				Log.WriteLine($"        - w: {kvp.Value.Width}");
				Log.WriteLine($"        - h: {kvp.Value.Height}");
			}

			TextureAtlases.Add(file, new(texture, regions));
		}
	}

	public static Texture2D LoadTexture2DFromFile(string file) {
		string texturesDirectory = Path.Combine(Core.AssetsDirectory, "Textures/output");
		string pngFile = Path.Combine(texturesDirectory, file + ".png");
		string jpgFile = Path.Combine(texturesDirectory, file + ".jpg");

		if (File.Exists(jpgFile) && File.Exists(pngFile)) {
			Log.WriteLine($"Ambiguous file names '{pngFile}' and '{jpgFile}'");
			return null;
		}

		string imageFile = File.Exists(pngFile) ? pngFile : jpgFile;

		using FileStream fs = new(imageFile, FileMode.Open);
		Texture2D texture = Texture2D.FromStream(Engine.Instance.GraphicsDevice, fs);

		return texture;
	}

	public static AtlasSheetMeta LoadTextureAtlasMeta(string file) {
		Log.WriteErrorIf(!File.Exists(file), $"Missing json data file for spritesheet '{file}'");

		string data = File.ReadAllText(file);
		AtlasSheetMeta meta = Newtonsoft.Json.JsonConvert.DeserializeObject<AtlasSheetMeta>(data);

		return meta;
	}
}
