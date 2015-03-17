using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using MyFirstGame.Sprites;

namespace MyFirstGame
{
	public class Camera
	{

		/// <summary>
		/// Position of camera. Denotes center of viewport.
		/// </summary>
		private Vector2 position;


		/// <summary>
		/// Rotation in radians.
		/// </summary>
		private float rotation;

		/// <summary>
		/// Camera zoom. Greater than 1 zooms in, and less than 1 zooms out
		/// </summary>
		private float scale;

		/// <summary>
		/// The sprite the camera will follow around. If set to null, camera remains still.
		/// </summary>
		public BaseSprite spriteToFollow;

		/// <summary>
		/// The linear transformation represented by position, rotation, and scale of the camera. Passed into SpriteBatch.Begin()
		/// </summary>
		public Matrix matrix;

		/// <summary>
		/// Viewing window of the camera. 
		/// </summary>
		public Vector2 viewport;           


		/// <summary>
		/// How fast the camera locks on to the character. 
		/// </summary>
		public float acceleration = 4f;
	
		/// <summary>
		/// How fast the camera changes scale
		/// </summary>
		public float zoomSpeed = 4;

		/// <summary>
		/// Setting this will change the zoom of the camera.
		/// </summary>
		public float targetScale;


		/// <summary>
		/// Determine whether the camera is currently shaking.
		/// </summary>
		private bool shaking;

		/// <summary>
		/// Power of the shake.
		/// </summary>
		private float shakeMagnitude;

		/// <summary>
		/// How long the current shake lasts.
		/// </summary>
		private float shakeDuration;

		/// <summary>
		/// Duration of currentShake. Shake ends when the timer surpasse the duration.
		/// </summary>
		private float shakeTimer;

		/// <summary>
		/// The vector determining the current amount the shake moves the camera.
		/// </summary>
		private Vector2 shakeOffset;


		/// <summary>
		/// Property for changing position.
		/// </summary>
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
			
		/// <summary>
		/// Property for chaning the rotation.
		/// </summary>
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



		/// <summary>
		/// Property for changing the scale
		/// </summary>
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

		/// <summary>
		/// Initializes the camera
		/// </summary>
		/// <param name="width">Width of the viewport</param>
		/// <param name="height">Height of the viewport</param>
		public Camera(float width, float height)
		{
			position = Vector2.Zero;
			rotation = 0;
			scale = 1f;
			targetScale = 1.5f;
			viewport = new Vector2(width, height);
			updateMatrix();
		}

		/// <summary>
		/// Updates the camera's matrix. Called after any change to position, rotation, or scale.
		/// </summary>
		private void updateMatrix()
		{
			position = new Vector2((float)Math.Round(position.X, 3), (float)Math.Round(position.Y, 3));
			matrix = Matrix.CreateTranslation(-position.X, -position.Y, 0.0f) *
				Matrix.CreateRotationZ(rotation) *
				Matrix.CreateScale(scale) *
				Matrix.CreateTranslation(viewport.X / 2, viewport.Y / 2, 0.0f);
		}

		/// <summary>
		/// Updates the viewport with a new width and height.
		/// </summary>
		/// <param name="width">Width of the viewport</param>
		/// <param name="height">Height of the viewport</param>
		public void updateViewport(float width, float height)
		{
			viewport.X = width;
			viewport.Y = height;
			updateMatrix();
		}

		/// <summary>
		/// Begins a camera shake with a magnitude and duration.
		/// </summary>
		/// <param name="duration">How long the shake will be</param>
		/// <param name="magnitude">The furthest length of a single shake/param>
		public void shake(float duration, float magnitude)
		{
			shaking = true;

			shakeMagnitude = magnitude;
			shakeDuration = duration;

			shakeTimer = 0f;
		}

		/// <summary>
		/// Returns a point applied to the camera's transformation matrix.
		/// </summary>
		/// <param name="position">Point to transform</param>
		public Vector2 getPointRelativeToCamera(Vector2 position)
		{
			return Vector2.Transform(position, matrix);
		}


		/// <summary>
		/// Transforms a camera point to a point in the world.
		/// </summary>
		/// <param name="position">Point to transform</param>
		public Vector2 CameraToWorldPoint(Vector2 position)
		{
			return Vector2.Transform(position, Matrix.Invert(matrix));
		}



		/// <summary>
		/// [DEPRECATED] Locks the camera to a sprite.
		/// </summary>
		/// <param name="sprite">Sprite to lock to</param>
		public void LockToTarget(BaseSprite sprite)
		{

			position.X =
				sprite.position.X +
				(sprite.texture.Width / 2);
			position.Y =
				sprite.position.Y +
				(sprite.texture.Height / 2);

		}

	
		/// <summary>
		/// Sets the follow target for the camera.
		/// </summary>
		/// <param name="sprite">Sprite to followo</param>
		public void SetTarget(BaseSprite sprite)
		{
			this.spriteToFollow = sprite;
		}


		/// <summary>
		/// Updates the shaking and handles the target following.
		/// </summary>
		/// <param name="gameTime">GameTime from main game</param>
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

			scale += (targetScale - scale) * zoomSpeed * delta;
			if (spriteToFollow != null) {
				Vector2 spriteCenter = spriteToFollow.Center;
				position.X += ((spriteCenter.X - position.X) * acceleration * delta);
				position.Y += ((spriteCenter.Y - position.Y) * acceleration * delta);

			}

			updateMatrix ();

		}


		/// <summary>
		/// Prevents the camera from moving out of the specificed rectangle.
		/// </summary>
		/// <param name="z">X value of rect</param>
		/// <param name="y">Y value of rect</param>
		/// <param name="width">Width of rect</param>
		/// <param name="height">Height of rect</param>
		public void ClampToArea(int x, int y, int width, int height)
		{
			// Calculate bounds of rectangle
			int left = x, right = x + width, top = y, bottom = y + height;


			// Calculate bounds of camera viewport
			float cameraLeft = position.X - (viewport.X / 2) / scale, cameraRight = position.X + (viewport.X / 2) / scale,
			cameraTop = position.Y - (viewport.Y / 2) / scale, cameraBottom = position.Y + (viewport.Y / 2) / scale;


			// use bounds to calcualate new camera bounds

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

			// Average camera bounds to determine new caemra position
			Position = new Vector2 ((cameraLeft + cameraRight) / 2, (cameraTop + cameraBottom) / 2);

		}
	}
}
