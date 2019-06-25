using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using AntRace.GameObjects;
using AntRace.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AntRace.Handlers
{
    public class BalanceHandler
    {
        int Balance = 1000;
        int betAmount = 100;
        int currentBet;
        SpriteFont TextFont;

        public BalanceHandler(ContentManager content)
        {
            TextFont = content.Load<SpriteFont>("Font");
            EventTrigger.AddListener("PlayerWonBet", BetWon);
            EventTrigger.AddListener("PlayerPlacedBet", PlaceBet);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();
            spriteBatch.DrawString(TextFont, "Balance: $" + Balance.ToString(), new Vector2(10, 10), Color.Gold, 0, Vector2.Zero, 1.2f, SpriteEffects.None, 0);
            spriteBatch.End();
        }

        bool PlaceBet()
        {
            Balance -= betAmount;
            currentBet = betAmount;
            return true;
        }

        bool BetWon()
        {
            Balance += currentBet * RaceHandler.AntsTotal;
            return true;
        }

        public void UnloadContent()
        {
            TextFont = null;
        }
    }
}
