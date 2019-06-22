﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;


namespace Nez
{
	/// <summary>
	/// cursor with trails. Note that this should be rendered in screen space (ScreenSpaceRenderer) so it isnt transformed by the camera matrix
	/// Adapted from: http://www.catalinzima.com/xna/samples/world-of-goo-cursor/
	/// </summary>
	public class GooCursor : RenderableComponent, IUpdatable
	{
		public override float Width { get { return _cursorTexture.Width; } }
		public override float Height { get { return _cursorTexture.Height; } }

		/// <summary>
		/// Gets of Sets the stiffness of the trail A lower number means the trail will be longer
		/// </summary>
		public float TrailStiffness = 30000;

		/// <summary>
		/// Controls the damping of the velocity of trail nodes
		/// </summary>
		public float TrailDamping = 600;

		/// <summary>
		/// Mass of a trails node
		/// </summary>
		public float TrailNodeMass = 11f;

		/// <summary>
		/// The scaling applied at the tip of the cursor
		/// </summary>
		public float StartScale = 1f;

		/// <summary>
		/// The scaling applied at the end of the cursor
		/// </summary>
		public float EndScale = 0.3f;

		/// <summary>
		/// use this to control the rate of change between the StartScale and the EndScale
		/// </summary>
		public float LerpExponent = 0.5f;

		/// <summary>
		/// Color used to fill the cursor
		/// </summary>
		public Color FillColor = Color.Black;

		/// <summary>
		/// color used for the cursor border
		/// </summary>
		public Color BorderColor = Color.White;

		/// <summary>
		/// Size of the border (in pixels)
		/// </summary>
		public float BorderSize = 10;


		// this is the sprite that is drawn at the current cursor position.
		// textureCenter is used to center the sprite when drawing.
		Texture2D _cursorTexture;
		Vector2 _textureCenter;

		int _trailNodeCount;
		TrailNode[] _trailNodes;


		public GooCursor() : this( 50 )
		{}

		public GooCursor( int trailNodeCount )
		{
			_trailNodeCount = trailNodeCount;
			_trailNodes = new TrailNode[_trailNodeCount];

			// initialize all positions to the current mouse position to avoid jankiness
			for( var i = 0; i < _trailNodeCount; i++ )
				_trailNodes[i].Position = Input.ScaledMousePosition;
		}

		public override void OnAddedToEntity()
		{
			_cursorTexture = Entity.Scene.Content.Load<Texture2D>( "nez/textures/gooCursor" );
			_textureCenter = new Vector2( _cursorTexture.Width / 2, _cursorTexture.Height / 2 );
		}

		void IUpdatable.Update()
		{
			// set position of first trail node;
			_trailNodes[0].Position = Input.RawMousePosition.ToVector2();

			// update the trails
			for( var i = 1; i < _trailNodeCount; i++ )
			{
				var node = _trailNodes[i];

				// calculate spring force
				var stretch = node.Position - _trailNodes[i - 1].Position;
				var force = -TrailStiffness * stretch - TrailDamping * node.Velocity;

				// apply acceleration
				var acceleration = force / TrailNodeMass;
				node.Velocity += acceleration * Time.DeltaTime;

				// apply velocity
				node.Position += node.Velocity * Time.DeltaTime;
				_trailNodes[i] = node;
			}
		}

		public override void Render( Graphics graphics, Camera camera )
		{
			// First we draw all the trail nodes using the border color. we need to draw them slightly larger, so the border is left visible
			// when we draw the actual nodes

			// adjust the startScale and endScale to take into consideration the border
			var borderStartScale = StartScale + BorderSize / _cursorTexture.Width;
			var borderEndScale = EndScale + BorderSize / _cursorTexture.Width;

			// draw all nodes with the new scales
			for( var i = 0; i < _trailNodeCount; i++ )
			{
				var node = _trailNodes[i];
				var lerpFactor = (float)i / (float)( _trailNodeCount - 1 );
				lerpFactor = Mathf.Pow( lerpFactor, LerpExponent );
				var scale = MathHelper.Lerp( borderStartScale, borderEndScale, lerpFactor );

				// draw using the border Color
				graphics.Batcher.Draw( _cursorTexture, node.Position, null, BorderColor, 0.0f, _textureCenter, scale, SpriteEffects.None, 0.0f );
			}

			// Next, we draw all the nodes normally, using the fill Color because before we drew them larger, after we draw them at
			// their normal size, a border will remain visible.
			for( var i = 0; i < _trailNodeCount; i++ )
			{
				var node = _trailNodes[i];
				var lerpFactor = (float)i / (float)( _trailNodeCount - 1 );
				lerpFactor = Mathf.Pow( lerpFactor, LerpExponent );
				var scale = MathHelper.Lerp( StartScale, EndScale, lerpFactor );

				// draw using the fill color
				graphics.Batcher.Draw( _cursorTexture, node.Position, null, FillColor, 0.0f, _textureCenter, scale, SpriteEffects.None, 0.0f );
			}
		}


		private struct TrailNode
		{
			public Vector2 Position;
			public Vector2 Velocity;
		}

	}
}

