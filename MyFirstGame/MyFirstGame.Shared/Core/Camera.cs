using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using MyFirstGame.Sprites;

namespace MyFirstGame
{
	public class Camera
	{
		public bool isFollowingSprite = true;
		Vector2 position;

		//public Dictionary<BaseSprite, float> spritesToFollow = new Dictionary<BaseSprite, float>();
		public BaseSprite spriteToFollow;


		public Matrix matrix;
		public Vector2 viewport;                //width and height of the viewport
		public float MoveSpeed = 4f;
	
		public float originalScale;
		public float targetScale;

		private bool shaking;
		private float shakeMagnitude;
		private float shakeDuration;
		private float shakeTimer;
		private Vector2 shakeOffset;



		public Vector2 Position
		{
			get { 
				return position; 
			}
			set { 
				position = value;
				updateMatrix();
			}
		}

		float rotation;
		public float Rotation
		{
			get
			{
				return rotation;
			}
			set
			{
				rotation = value;
				updateMatrix();
			}
		}

		float scale;
		public float Scale
		{
			get
			{
				return scale;
			}
			set
			{
				scale = value;
				updateMatrix();
			}
		}

		public Camera(float width, float height)
		{
			position = Vector2.Zero;
			rotation = 0;
			scale = 1f;
			targetScale = 1.5f;
			viewport = new Vector2(width, height);
			updateMatrix();
		}

		public void updateMatrix()
		{
			position = new Vector2((float)Math.Round(position.X, 3), (float)Math.Round(position.Y, 3));
			matrix = Matrix.CreateTranslation(-position.X, -position.Y, 0.0f) *
				Matrix.CreateRotationZ(rotation) *
				Matrix.CreateScale(scale) *
				Matrix.CreateTranslation(viewport.X / 2, viewport.Y / 2, 0.0f);
		}

		public void updateViewport(float width, float height)
		{
			viewport.X = width;
			viewport.Y = height;
			updateMatrix();
		}

		public void shake(float duration, float magnitude)
		{
			shaking = true;

			shakeMagnitude = magnitude;
			shakeDuration = duration;

			shakeTimer = 0f;
		}

		// for particles
		public Vector2 getPointRelativeToCamera(Vector2 position)
		{
			return Vector2.Transform(position, matrix);
		}

		public Vector2 CameraToWorldPoint(Vector2 position)
		{
			return Vector2.Transform(position, Matrix.Invert(matrix));
		}




		public void LockToTarget(BaseSprite sprite)
		{

			position.X =
				sprite.position.X +
				(sprite.texture.Width / 2);
			position.Y =
				sprite.position.Y +
				(sprite.texture.Height / 2);

		}

	

		public void SetTarget(BaseSprite sprite)
		{
			this.spriteToFollow = sprite;
		}

		public void Update(GameTime gameTime)
		{

			if (shaking)
			{
				// Move our timer ahead based on the elapsed time
				shakeTimer += CurrentGame.getDelta(gameTime);

				// If we're at the max duration, we're not going to be shaking
				// anymore
				if (shakeTimer >= shakeDuration)
				{
					shaking = false;
					shakeTimer = shakeDuration;
				}

				// Compute our progress in a [0, 1] range
				float progress = shakeTimer / shakeDuration;

				// Compute our magnitude based on our maximum value and our
				// progress. This causes
				// the shake to reduce in magnitude as time moves on, giving us a
				// smooth transition
				// back to being stationary. We use progress * progress to have a
				// non-linear fall
				// off of our magnitude. We could switch that with just progress if
				// we want a linear
				// fall off.
				float magnitude = shakeMagnitude * (1f - (progress * progress));

				// Generate a new offset vector with three random values and our
				// magnitude
				shakeOffset = new Vector2((float) CurrentGame.random.NextDouble()*2 - 1,
					(float) CurrentGame.random.NextDouble()*2 - 1)*magnitude;
				Position += shakeOffset;
			}
			float delta = CurrentGame.getDelta (gameTime);
			scale += (targetScale - scale) * 0.2f * 0.2f;
			if (spriteToFollow != null) {
				Vector2 spriteCenter = spriteToFollow.Center;
				position.X += ((spriteCenter.X - position.X) * MoveSpeed * delta);
				position.Y += ((spriteCenter.Y - position.Y) * MoveSpeed * delta);

			}

			updateMatrix ();

		}

		public bool IsInView(Texture2D texture, Vector2 position, int offset)
		{
			Rectangle cameraRect = new Rectangle(-offset, -offset, (int)viewport.X+2*offset, (int)viewport.Y+2*offset);
			Rectangle rect = new Rectangle((int)position.X, (int)position.Y, texture.Width, texture.Height);
			return cameraRect.Contains(rect);
		}


		public void ClampToArea(int x, int y, int width, int height)
		{

			int left = x, right = x + width, top = y, bottom = y + height;

			float cameraLeft = position.X - (viewport.X / 2) / scale, cameraRight = position.X + (viewport.X / 2) / scale,
			cameraTop = position.Y - (viewport.Y / 2) / scale, cameraBottom = position.Y + (viewport.Y / 2) / scale;

			if (cameraLeft < left) {
				cameraRight += left - cameraLeft;
				cameraLeft = left;
			}
			if (cameraRight > right) {
				cameraLeft -= cameraRight - right;
				cameraRight = right;
			}
			if (cameraTop < top) {
				cameraBottom += top - cameraTop;
				cameraTop = top;
			}
			if (cameraBottom > bottom) {
				cameraTop -= cameraBottom - bottom;
				cameraBottom = bottom;
			}

			Position = new Vector2 ((cameraLeft + cameraRight) / 2, (cameraTop + cameraBottom) / 2);
//
//			if (position.X > width)
//			{
//				position.X = width;
//
//			}
//			if (position.Y > height)
//			{
//				position.Y = height;
//
//			}
//
//			if (position.X < 0)
//			{
//				position.X = 0;
//
//			}
//			if (position.Y < 0)
//			{
//				position.Y = 0;
//
//			}
//

		}
	}
}
