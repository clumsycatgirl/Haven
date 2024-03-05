
namespace Haven;

[Serializable]
public class Engine : Game {
	public static Engine Instance { get; private set; }
	public static GraphicsDeviceManager Graphics { get; private set; }
	public static float RawDeltaTime { get; private set; }
	public static float DeltaTime { get; private set; }
	public static float TimeRate = 1f;
	public static float FreezeTimer;
	public static int FPS;
	private TimeSpan counterElapsed = TimeSpan.Zero;
	private int fpsCounter = 0;

	private Scene scene;

	public bool ExitOnEscape = true;

	public static Scene Scene {
		get => Instance.scene;
		set => Instance.scene = value;
	}

	public Engine() {
		Log.WriteErrorIf(Instance is not null, "can't have multiple instances of the main engine");
		Instance = this;

		Graphics = new GraphicsDeviceManager(this) {
			SynchronizeWithVerticalRetrace = true,
			PreferMultiSampling = false,
			GraphicsProfile = GraphicsProfile.HiDef,
			PreferredBackBufferFormat = SurfaceFormat.Color,
			PreferredDepthStencilFormat = DepthFormat.Depth24Stencil8,
		};
		Graphics.ApplyChanges();

		GCSettings.LatencyMode = GCLatencyMode.SustainedLowLatency;

		Content.RootDirectory = "Content";
		IsMouseVisible = true;

		Scene = new();
	}

	protected override void Initialize() {
		Input.Initialize();
		Renderer.Initialize(GraphicsDevice);

		base.Initialize();
	}

	protected override void LoadContent() {
		ContentLoader.LoadContent();
	}

	protected override void Update(GameTime gameTime) {
		RawDeltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
		DeltaTime = RawDeltaTime * TimeRate;

		Input.Update();

		if (ExitOnEscape && Input.Keyboard.Pressed(Keys.Escape)) {
			Exit();
		}

		if (FreezeTimer > 0) {
			FreezeTimer = Math.Max(FreezeTimer - RawDeltaTime, 0);
		} else {
			Scene?.BeforeUpdate();
			Scene?.Update();
			Scene?.AfterUpdate();
		}

		Scene?.End();
		Scene?.Begin();

		base.Update(gameTime);
	}

	protected override bool BeginDraw() {
		base.BeginDraw();
		Renderer.DrawBegin();
		return true;
	}

	protected override void Draw(GameTime gameTime) {
		GraphicsDevice.Clear(Color.Black);

		Scene?.BeforeDraw();
		Scene?.Draw();
		Scene?.AfterDraw();

		base.Draw(gameTime);

		fpsCounter++;
		counterElapsed += gameTime.ElapsedGameTime;
		if (counterElapsed >= TimeSpan.FromSeconds(1)) {
			Window.Title = $"{fpsCounter} fps - {GC.GetTotalMemory(false) / 1048576f:F} mb";
			FPS = fpsCounter;
			fpsCounter = 0;
			counterElapsed -= TimeSpan.FromSeconds(1);
		}
	}

	protected override void EndDraw() {
		Renderer.DrawEnd();
		base.EndDraw();
	}

	protected override void Dispose(bool disposing) {
		base.Dispose(disposing);

		Log.Dispose();
	}
}
