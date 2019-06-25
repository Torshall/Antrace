using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AntRace.Interfaces
{
        public interface IDrawableObject
        {
            Vector2 Position
            {
                get; set;
            }

            int DrawOrder
            {
                get;
            }
            bool Visible
            {
                get;
            }
           Texture2D Texture
            {
                get;set;
            }

            Color Color
            {
                get; set;
            }

            event EventHandler DrawOrderChanged;
            event EventHandler VisibleChanged;

            void Draw(SpriteBatch spriteBatch);
        }
}
