using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using AntRace.GameObjects.NPCS;
using AntRace.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AntRace.GameObjects
{
    public class RaceHandler
    {
        Color[] AntColors = new Color[] { Color.Black, Color.IndianRed, Color.LightPink, Color.SandyBrown, Color.White };

        public static int AntsTotal = 5;
        List<AntNPC> NPCList;
        List<Texture2D> AntTextures;

        static float FinishLineXPosition = 700;
        GameObject FinishLine;
        bool PickRandomAntToMove = true;
        bool RaceIsOngoing = false;
        SpriteFont TextFont;
        bool TriggerEndResult;
        AntNPC PickedAnt;
        bool playerAntWon;

        public RaceHandler(ContentManager Content)
        {
            TextFont = Content.Load<SpriteFont>("Font");
            AntTextures = new List<Texture2D>
            {
                Content.Load<Texture2D>("Sprites/Ant1"),
                Content.Load<Texture2D>("Sprites/Ant2")
            };

            CreateAntNPCS();

            FinishLine = new GameObject();
            FinishLine.Texture = Content.Load<Texture2D>("Sprites/finishLine");
            FinishLine.Position = new Vector2(FinishLineXPosition, 0);

            EventTrigger.AddListener("StartRace", StartRace);
            EventTrigger.AddListener("LeftMouseClicked", PlayerLectClickListener);

        }

        bool PlayerLectClickListener()
        {
            if (!RaceIsOngoing)
            {
                //Fectch the data that was sent through the event
                Vector2 mousePos = EventTrigger.GetData("LeftMouseClicked").NextVector2();

                foreach (AntNPC npcObject in NPCList)
                {
                    if (npcObject.Bounds.Contains(mousePos))
                    {
                        PickedAnt = npcObject;
                        EventTrigger.TriggerListener("StartRace");
                        EventTrigger.TriggerListener("PlayerPlacedBet");
                        break;
                    }
                }
            }
            return true;
        }

        public void CreateAntNPCS()
        {
            NPCList = new List<AntNPC>();
            for (int i = 0; i < AntsTotal; ++i)
            {
                AntNPC ant = new AntNPC(AntTextures, i, AntColors[i]);
                NPCList.Add(ant);
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();

            FinishLine.Draw(spriteBatch);

            if (NPCList != null)
            {
                foreach (AntNPC npcObject in NPCList)
                {
                    npcObject.Draw(spriteBatch);
                }
            }

            if (TriggerEndResult)
            {
                string endText = playerAntWon ? "Antastic amazeballs, your ant won!" : "As ANticipated, no win..";
                spriteBatch.DrawString(TextFont, endText, new Vector2(321, 21), Color.Black, 0, Vector2.Zero, 1f, SpriteEffects.None, 0);
                spriteBatch.DrawString(TextFont, endText, new Vector2(320, 20), Color.White, 0, Vector2.Zero, 1f, SpriteEffects.None, 0);
            }
            else if (!RaceIsOngoing)
            {
                spriteBatch.DrawString(TextFont, "Click to bet on your Amazing Ant!", new Vector2(301, 21), Color.Black, 0, Vector2.Zero, 1f, SpriteEffects.None, 0);
                spriteBatch.DrawString(TextFont, "Click to bet on your Amazing Ant!", new Vector2(300, 20), Color.White, 0, Vector2.Zero, 1f, SpriteEffects.None, 0);
            }

            spriteBatch.End();
        }

        public void Update(GameTime gameTime)
        {
            if (RaceIsOngoing)
            {
                MoveRandomAnt();

                if (NPCList != null)
                {
                    foreach (AntNPC npcObject in NPCList)
                    {
                        npcObject.Update(gameTime);
                        if (npcObject.Position.X > FinishLineXPosition)
                        {
                            RaceOver(npcObject);
                            break;
                        }
                    }
                }
            }
        }

        /// This Would make more sense in update loop, but to show the TriggerListener we call a random ant directly trough EventEvaluation.
        void MoveRandomAnt()
        {
            if (PickRandomAntToMove)
            {
                int randNPC = GameCore.random.Next(0, RaceHandler.AntsTotal + 1);
                EventTrigger.TriggerListener("AntMoveForward" + randNPC.ToString());
                PickRandomAntToMove = false;

                _Timer.AddDelegate(delegate
                {
                    PickRandomAntToMove = true;
                }, 0.1f);
            }
        }

        bool StartRace()
        {
            RaceIsOngoing = true;
            return true;
        }

        void RaceOver(AntNPC winner)
        {
            RaceIsOngoing = false;
            TriggerEndResult = true;
            playerAntWon = winner == PickedAnt;

            if(playerAntWon)
                EventTrigger.TriggerListener("PlayerWonBet");

            _Timer.AddDelegate(delegate
            {
                EventTrigger.TriggerListener("ResetGame");
                TriggerEndResult = false;
                playerAntWon = false;
            }, 5);
        }
        
        public void UnloadContent()
        {
            for (int i = 0; i < AntTextures.Count; ++i)
            {
                AntTextures[i] = null;
            }
            AntTextures.Clear();
            NPCList.Clear();
        }
    }
}
