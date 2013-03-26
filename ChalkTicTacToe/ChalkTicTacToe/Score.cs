using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System.Diagnostics;

namespace ChalkTicTacToe
{
    class Score
    {
        Rectangle m_scoreRectangle;
        Texture2D m_scoreSprite;
        Texture2D[] m_scoreMessagesSprite = new Texture2D[5];

        string[] m_scoreMessages = new string[5];

        Button m_scoreRematchButton, m_scoreMenuButton, m_scoreStartButton;

        Game1 m_game;

        SpriteFont m_scoreFont;

        int m_scoreX = 0;
        int m_scoreO = 0;
        int m_scoreDraw = 0;

        public Score(Game1 game)
        {
            m_game = game;

            m_scoreFont = m_game.Content.Load<SpriteFont>("SpriteFontScore");

            m_scoreMessages = new string[] {"ScoreTurnX","ScoreTurnO","ScoreWinX","ScoreWinO","ScoreDraw"};
            for (var i = 0; i < m_scoreMessages.Length; i++)
            {
                m_scoreMessagesSprite[i] = m_game.Content.Load<Texture2D>(@"sprites/" + m_scoreMessages[i]);
            }
            m_scoreSprite = m_game.Content.Load<Texture2D>(@"sprites/MenuScore");
            m_scoreRectangle = new Rectangle(20,50,m_scoreSprite.Width,m_scoreSprite.Height);

            m_scoreStartButton = new Button(30, 320, m_game.Content.Load<Texture2D>(@"sprites/ScoreStart"),Color.White, Color.Gainsboro, Color.White);
            m_scoreMenuButton = new Button(30, 400, m_game.Content.Load<Texture2D>(@"sprites/ScoreMenu"), Color.White, Color.Gainsboro, Color.White);
            m_scoreRematchButton = new Button(150, 400, m_game.Content.Load<Texture2D>(@"sprites/ScoreRematch"), Color.White, Color.Gainsboro, Color.White);   
        }

        public void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            spriteBatch.Draw(m_scoreSprite, m_scoreRectangle, Color.White);
            spriteBatch.DrawString(m_scoreFont, m_scoreX.ToString(), new Vector2(m_scoreRectangle.X + 110, m_scoreRectangle.Y + 70), Color.Gainsboro);
            spriteBatch.DrawString(m_scoreFont, m_scoreO.ToString(), new Vector2(m_scoreRectangle.X + 112, m_scoreRectangle.Y + 125), Color.Gainsboro);
            spriteBatch.DrawString(m_scoreFont, m_scoreDraw.ToString(), new Vector2(m_scoreRectangle.X + 150, m_scoreRectangle.Y + 180), Color.Gainsboro);
        }

        public void DrawMessage(SpriteBatch spriteBatch, GameTime gameTime,int index)
        {            
            spriteBatch.Draw(m_scoreMessagesSprite[index], new Rectangle(30, 320, m_scoreMessagesSprite[index].Width, m_scoreMessagesSprite[index].Height), Color.White);
        }

        public void DrawStartButton(SpriteBatch spriteBatch, GameTime gameTime)
        {
            m_scoreStartButton.Draw(spriteBatch, gameTime);
        }

        public void DrawMenuButton(SpriteBatch spriteBatch, GameTime gameTime)
        {
            m_scoreRematchButton.Draw(spriteBatch, gameTime);
            m_scoreMenuButton.Draw(spriteBatch, gameTime);
        }

        public void HandleTouch(MouseState touch)
        {
            if (m_scoreStartButton !=null && m_scoreStartButton.Touched(touch) && m_scoreStartButton.m_state == Button.BState.DOWN) 
            {
                m_game.handleScoreStartClick();
            }
            else if (m_scoreRematchButton != null && m_scoreRematchButton.Touched(touch) && m_scoreRematchButton.m_state == Button.BState.DOWN) 
            {
                m_game.handleScoreRematchClick();
            }
            else if (m_scoreMenuButton != null && m_scoreMenuButton.Touched(touch) && m_scoreMenuButton.m_state == Button.BState.DOWN)
            {
                m_game.handleScoreMenuClick();
            }
            
        }

        public void IncreaseScore(string type)
        {
            switch (type){
                case "PlayerXWins":
                    m_scoreX += 1;
                    break;
                case "PlayerOWins":
                    m_scoreO += 1;
                    break;
                case "IsDraw":
                    m_scoreDraw += 1;
                    break;
            }
        }

        public void ResetScore()
        {
            m_scoreX = m_scoreO = m_scoreDraw = 0;
        }
    }
}
