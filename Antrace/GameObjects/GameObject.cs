using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using AntRace.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AntRace.GameObjects
{
    public class GameObject : IDrawableObject, IGameObject
    {
        #region Interface Variables
        public Vector2 Position
        {
            get; set;
        }

        public int DrawOrder
        {
            get;
        }
        public bool Visible
        {
            get;
        }

        public Color Color
        {
            get; set;
        }

        public Texture2D Texture
        {
            get; set;
        }

        public Rectangle Bounds
        {
            get
            {
                if (Texture == null)
                    return Rectangle.Empty;

                return new Rectangle((int)Position.X, (int)Position.Y, Texture.Width, Texture.Height);
            }
        }
        #endregion

        public event EventHandler DrawOrderChanged;
        public event EventHandler VisibleChanged;

        public GameObject()
        {
            Color = Color.White;
        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            if (Texture != null)
            {
                spriteBatch.Draw(Texture, Position, Color);
            }
        }

        public virtual void Update(GameTime gameTime)
        {

        }

        

    }
}
