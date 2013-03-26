using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ChalkTicTacToe
{
    public class Move
    {
        public char m_chPlayer;
        public int m_nX;
        public int m_nY;
        public AnimatedTexture m_sprite;

        public Move(int nX = 0, int nY = 0, char chPlayer = ' ', AnimatedTexture sprite = null)
        {
            m_nX = nX;
            m_nY = nY;
            m_chPlayer = chPlayer;
            m_sprite = sprite;
        }
    }
}
