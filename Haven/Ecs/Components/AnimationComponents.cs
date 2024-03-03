
namespace Haven.Ecs.Components;

[Serializable]
[DataContract]
[Component]
[DependsOn(typeof(SizeComponent))]
public class AnimationComponent(bool active) : Component(active, true) {
	[DataMember]
	public List<HTexture2D> Frames;
	[DataMember]
	public float FrameTime;
	private float elapsedTime;

	public HTexture2D CurrentFrame {
		get {
			int frameIndex = (int)(elapsedTime / FrameTime) % Frames.Count;
			return Frames[frameIndex];
		}
	}

	public AnimationComponent(List<HTexture2D> frames, float frameTime) : this(true) {
		Frames = frames;
		FrameTime = frameTime;
		elapsedTime = 0f;
	}

	public override void Update() {
		base.Update();
		elapsedTime += Engine.DeltaTime;
	}
}
