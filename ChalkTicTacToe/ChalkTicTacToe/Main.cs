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
    class Main
    {
        
        public const int NUMBER_OF_BUTTONS = 4,
            MODE1_ID = 0,
            MODE1B_ID = 1,
            MODE2_ID = 2,
            QUIT_ID = 3,
            BUTTON_HEIGHT = 90,
            BUTTON_WIDTH = 450;

        Button[] m_menuButtons = new Button[4];

        Texture2D m_logo;

        Game1 m_game;

        public Main(Game1 game)
        {
            m_game = game;

            m_logo = m_game.Content.Load<Texture2D>(@"sprites/MenuLogo");

            int x = m_game.Window.ClientBounds.Width / 2 - BUTTON_WIDTH / 2;
            int y = m_game.Window.ClientBounds.Height / 2 - NUMBER_OF_BUTTONS / 2 * BUTTON_HEIGHT -
                (NUMBER_OF_BUTTONS % 2) * BUTTON_HEIGHT / 2;
            for (int i = 0; i < m_menuButtons.Length; i++)
            {
                Texture2D texture = null;
                switch(i){
                    case MODE1_ID:
                        texture = m_game.Content.Load<Texture2D>(@"sprites/MenuOnePlayer");
                        break;
                    case MODE1B_ID:
                        texture = m_game.Content.Load<Texture2D>(@"sprites/MenuOnePlayer2");
                        break;
                    case MODE2_ID:
                        texture = m_game.Content.Load<Texture2D>(@"sprites/MenuTwoPlayers");
                        break;
                    case QUIT_ID:
                        texture = m_game.Content.Load<Texture2D>(@"sprites/MenuQuit");
                        break;
                }
                m_menuButtons[i] = new Button(x, y+70, texture,Color.White,Color.Gainsboro,Color.Gray);
                y += BUTTON_HEIGHT;
            }
                             
        }

        public void HandleTouch(MouseState touch)
        {
            for (int i = 0; i < m_menuButtons.Length; i++)
            {
                if (m_menuButtons[i].Touched(touch) && m_menuButtons[i].m_state == Button.BState.DOWN)
                {
                    m_game.handleMenuClick(i);
                    break;
                }
            }
        }

        public void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {

            spriteBatch.Draw(m_logo, new Rectangle(0,0,m_logo.Width,m_logo.Height), Color.White);

            for (int i = 0; i < m_menuButtons.Length; i++)
            {
                m_menuButtons[i].Draw(spriteBatch, gameTime);    
            }
        }
    }
}
