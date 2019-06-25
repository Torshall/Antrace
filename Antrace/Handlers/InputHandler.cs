using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using AntRace.Containers;
using AntRace.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AntRace.Handlers
{
    public class InputHandler
    {
        MouseState oldMouseState;

        public void Update()
        {
            MouseState mouseState = Mouse.GetState();
            if (mouseState.LeftButton == ButtonState.Pressed && oldMouseState.LeftButton == ButtonState.Released)
            {
                Vector2 clickPos = new Vector2(mouseState.Position.X, mouseState.Position.Y);
                EventTrigger.TriggerListener("LeftMouseClicked", new DataContainer() { VectorValues = new Vector2[] { clickPos } });
            }

            oldMouseState = mouseState;

        }
    }
}
