﻿using FontStashSharp.Interfaces;
using System;
using System.Reflection;

#if MONOGAME || FNA
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
#elif STRIDE
using Stride.Core.Mathematics;
using Stride.Graphics;
using Texture2D = Stride.Graphics.Texture;
#endif

namespace FontStashSharp
{
	internal class SpriteBatchRenderer : IFontStashRenderer
	{
		public static readonly SpriteBatchRenderer Instance = new SpriteBatchRenderer();

		private GraphicsDevice _graphicsDevice;
		private SpriteBatch _batch;

		public GraphicsDevice GraphicsDevice
		{
			get
			{
				if (_graphicsDevice != null)
				{
					return _graphicsDevice;
				}

				var bindingFlags = BindingFlags.Instance | BindingFlags.NonPublic;

				// get the field info
				var fieldInfo = typeof(SpriteBatch).GetField("graphicsDevice", bindingFlags);
				_graphicsDevice = (GraphicsDevice)fieldInfo.GetValue(_batch);

				return _graphicsDevice;
			}
		}

		public SpriteBatch Batch
		{
			get
			{
				return _batch;
			}

			set
			{
				if (value == null)
				{
					throw new ArgumentNullException(nameof(value));
				}

				if (value == _batch)
				{
					return;
				}

				_batch = value;
				_graphicsDevice = null;
			}
		}

		private SpriteBatchRenderer()
		{
		}

		public void Draw(Texture2D texture, Vector2 position, Rectangle? sourceRectangle, Color color, 
			float rotation, Vector2 origin, Vector2 scale, float depth)
		{
#if MONOGAME || FNA
			_batch.Draw(texture,
				position,
				sourceRectangle,
				color,
				rotation,
				origin,
				scale,
				SpriteEffects.None,
				depth);
#elif STRIDE
			_batch.Draw(texture,
				position,
				sourceRectangle,
				color,
				rotation,
				origin,
				scale,
				SpriteEffects.None,
				ImageOrientation.AsIs,
				depth);
#endif
		}
	}
}
