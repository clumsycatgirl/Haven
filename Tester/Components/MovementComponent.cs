
using System;
using System.Collections.Generic;
using System.Linq;

using Haven;
using Haven.Ecs;
using Haven.Ecs.Attributes;
using Haven.Ecs.Components;
using Haven.Inputs;
using Haven.Utils;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Tester.Components {
	[Serializable]
	[Component]
	[RequireComponentAttribute(typeof(TransformComponent), typeof(SizeComponent), typeof(DebugComponent))]
	internal class MovementComponent : Component {
		public const float MaxMovementSpeed = 95.0f;
		public float MovementSpeed = MaxMovementSpeed;
		public const float JumpForce = 47.5f;

		public const float Acceleration = 4.5f;
		public const float Deceleration = 1.85f;

		public const float RunMovementModifier = 1.96f;
		public const float AirMovementModifier = 0.96f;

		public const float Gravity = 9.81f * 16;
		public bool Grounded = false;
		public bool Jumping = false;

		private Vector2 velocity;

		public override void Update() {
			MoveAndJump();

			// Gravity
			velocity.Y += Gravity * Engine.DeltaTime;
			if (Grounded) {
				velocity.Y = MathF.Min(velocity.Y, 0);
			}

			CheckCollision();

			// Update Position
			Entity.Transform.Position += velocity * Engine.DeltaTime;
		}

		public void MoveAndJump() {
			// MovementSpeed Changes (eg. run, air)
			if (Input.Keyboard.Pressed(Keys.LeftShift)) MovementSpeed = MaxMovementSpeed * RunMovementModifier;
			else if (Input.Keyboard.Released(Keys.LeftShift)) MovementSpeed = MaxMovementSpeed * RunMovementModifier;
			MovementSpeed = !Grounded ? MaxMovementSpeed * AirMovementModifier : MaxMovementSpeed / AirMovementModifier;

			// Move
			bool pressing = false;
			if (Input.Keyboard.Check(Keys.A)) {
				velocity.X -= MovementSpeed * Acceleration * Engine.DeltaTime;
				pressing = true;
			}
			if (Input.Keyboard.Check(Keys.D)) {
				velocity.X += MovementSpeed * Acceleration * Engine.DeltaTime;
				pressing = true;
			}

			if (!pressing) {
				if (velocity.X < 0) {
					velocity.X += Deceleration * MovementSpeed * Engine.DeltaTime;
					velocity.X = MathF.Min(velocity.X, 0);
				} else if (velocity.X > 0) {
					velocity.X -= Deceleration * MovementSpeed * Engine.DeltaTime;
					velocity.X = MathF.Max(velocity.X, 0);
				}
			}

			velocity.X = MathF.Max(MathF.Min(velocity.X, MovementSpeed), -MovementSpeed);

			// Jump
			if (Input.Keyboard.Check(Keys.Space) && Grounded) {
				velocity.Y -= JumpForce;
				Grounded = false;
			}
		}

		public void CheckCollision() {
			// Floor
			IEnumerable<Entity> flooors = Scene.GetEntitiesOfTag<Entity>("floor");
			foreach (Entity floor in flooors) {
				if (Entity.Transform.Position.Y + Entity.Size.Height >= floor.Transform.Position.Y) {
					Entity.Transform.Position.Y = floor.Transform.Position.Y - Entity.Size.Height;
					Grounded = true;
				} else {
					Grounded = false;
				}
			}
		}

		public override void Draw() {
			base.Draw();
			// Log.WriteLine(Entity.Get<AnimationComponent>().ToString());
		}
	}
}
