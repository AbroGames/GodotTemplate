using Godot;
using KludgeBox.DI.Requests.NotNullCheck;

namespace GodotTemplate.Scenes.Screen.LoadingScreen;

public partial class LoadingAnimHandle : Node2D
{
	[Export] [NotNull] public Sprite2D ThickPart {get; private set;}
	[Export] [NotNull] public Sprite2D ThinPart {get; private set;}

	[Export] [NotNull] public Curve ThickRotationSpeedCurve {get; private set;}
	[Export] [NotNull] public Curve ThinRotationSpeedCurve {get; private set;}

	[Export] public float AnimationProgress = 0;
	[Export] public float AnimationSpeed = 0.1f;
	
	[Export] public float RotationSpeed = Mathf.Pi;

	public override void _Ready()
	{
		Di.Process(this);
	}

	public override void _Process(double delta)
	{
		AnimationProgress += AnimationSpeed * (float) delta;
		AnimationProgress %= 1;

		ThickPart.Rotation += RotationSpeed * ThickRotationSpeedCurve.Sample(AnimationProgress) * (float) delta;
		ThinPart.Rotation += RotationSpeed * ThinRotationSpeedCurve.Sample(AnimationProgress) * (float) delta;
	}
}
