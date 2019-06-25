using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using AntRace.Helpers;
using AntRace.Interfaces;

namespace AntRace.GameObjects.NPCS
{
    public class AntNPC : GameObject
    {
        int NPCIndex;
        Vector2 StartPosition;
        List<Texture2D> AntSprites;
        int SpriteIndex = 0;
        public AntNPC(List<Texture2D> sprites, int index, Color antColor)
        {
            NPCIndex = index;
            StartPosition = new Vector2(20, 40 + (100 * NPCIndex));
            Position = StartPosition;
            AntSprites = sprites;
            Texture = AntSprites[0];
            Color = antColor;

            EventTrigger.AddListener("AntMoveForward" + NPCIndex, OnMoveDownCallback);
            EventTrigger.AddListener("ResetGame", ResetAnt);       
        }

        bool ResetAnt()
        {
            Position = StartPosition;
            return true;
        }

        bool OnMoveDownCallback()
        {
            SpriteIndex++;
            if (SpriteIndex > AntSprites.Count - 1)
                SpriteIndex = 0;

            Texture = AntSprites[SpriteIndex];

            Position += Vector2.UnitX * 10;
            return true;
        }
    }
}
