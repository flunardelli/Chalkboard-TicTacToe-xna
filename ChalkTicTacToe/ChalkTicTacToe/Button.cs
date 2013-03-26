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

namespace ChalkTicTacToe
{
    class Button
    {
        public enum BState
        {
            HOVER,
            UP,
            JUST_RELEASED,
            DOWN
        }
        public int m_width;
        public int m_height;
        public Color m_color, m_colorUp, m_colorHover, m_colorDown;
        public BState m_state;
        public Texture2D m_texture;
        public Rectangle m_rectangle;

        public Button(int x, int y, Texture2D texture, Color color_up, Color color_hover, Color color_down)
        {
            m_width = texture.Width;
            m_height = texture.Height;
            m_rectangle = new Rectangle(x, y, m_width,m_height);
            m_color = color_up;
            m_colorUp = color_up;
            m_colorHover = color_hover;
            m_colorDown = color_down;
            m_texture = texture;
            m_state = BState.UP;
        }
        public void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            spriteBatch.Draw(m_texture, m_rectangle, m_color);
        }
        public bool Touched(MouseState touch)
        {
            m_state = BState.UP;
            m_color = m_colorUp;
            if (m_rectangle.Contains((int)touch.X, (int)touch.Y))
            {
                m_state = BState.HOVER;
                m_color = m_colorHover;
                if (touch.LeftButton == ButtonState.Pressed)
                {
                    m_color = m_colorDown;
                    m_state = BState.DOWN;
                }
                return true;
            }
            m_color = m_colorUp;
            return false;
        }
    }
}
