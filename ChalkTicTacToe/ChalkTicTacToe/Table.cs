using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ChalkTicTacToe
{
    public class Table
    {

        public Move[,] m_achCells;
        public int m_nSide = 3;

        public GameState m_eState = GameState.IsPlaying;

        public Table(int nSide)
        {

            m_nSide = nSide;

            m_achCells = new Move[m_nSide, m_nSide];
            for (int nY = 0; nY < m_nSide; nY++)
            {

                for (int nX = 0; nX < m_nSide; nX++)
                {

                    m_achCells[nX, nY] = new Move();
                }
            }
        }

        public Table(Table Original)
        {

            m_nSide = Original.m_nSide;

            m_achCells = new Move[m_nSide, m_nSide];
            for (int nY = 0; nY < m_nSide; nY++)
            {

                for (int nX = 0; nX < m_nSide; nX++)
                {

                    m_achCells[nX, nY] = Original.m_achCells[nX, nY];
                }
            }
        }
        public void ApplyMove(Move oMove)
        {

            m_achCells[oMove.m_nX, oMove.m_nY] = oMove;

            UpdateGameState();
        }

        

        public string CheckPlayerWins(char chPlayer)
        {

            { //lines?
                int nY;
                for (nY = 0; nY < m_nSide; nY++)
                {

                    bool bnComplete = true;

                    for (int nX = 0; nX < m_nSide; nX++)
                    {

                        if (m_achCells[nX, nY].m_chPlayer != chPlayer)
                        {

                            bnComplete = false;
                            break;
                        }
                    }

                    if (bnComplete)
                    {
                        return "L" + nY;
                    }

                }
            }

            { //columns?
                int nX;
                for (nX = 0; nX < m_nSide; nX++)
                {

                    bool bnComplete = true;                    
                    for (int nY = 0; nY < m_nSide; nY++)
                    {

                        if (m_achCells[nX, nY].m_chPlayer != chPlayer)
                        {

                            bnComplete = false;
                            break;
                        }
                    }

                    if (bnComplete)
                        return "C" + nX;
                }
            }

            { //diagonal "\" ?

                bool bnComplete = true;
                int nC;
                for (nC = 0; nC < m_nSide; nC++)
                {

                    if (m_achCells[nC, nC].m_chPlayer != chPlayer)
                    {

                        bnComplete = false;
                        break;
                    }
                }

                if (bnComplete)
                    return "DL";
            }

            { //diagonal "/" ?

                bool bnComplete = true;
                int nC;
                for (nC = 0; nC < m_nSide; nC++)
                {

                    if (m_achCells[m_nSide - nC - 1, nC].m_chPlayer != chPlayer)
                    {

                        bnComplete = false;
                        break;
                    }
                }

                if (bnComplete)
                    return "DR";
            }

            return null;
        }

        bool TestForEmptyCells()
        {

            {

                for (int nY = 0; nY < m_nSide; nY++)
                {

                    for (int nX = 0; nX < m_nSide; nX++)
                    {

                        if (m_achCells[nX, nY].m_chPlayer == ' ')
                            return true;
                    }
                }
            }

            return false;
        }

        public void GetAvailableMoves(List<Move> lMoves, char chPlayer)
        {
            { 
                for (int nY = 0; nY < m_nSide; nY++)
                {

                    for (int nX = 0; nX < m_nSide; nX++)
                    {
                        if (m_achCells[nX, nY].m_chPlayer == ' ')
                            lMoves.Add(new Move(nX, nY, chPlayer));
                    }
                }
            }
        }

        void UpdateGameState()
        {

            if (CheckPlayerWins('X') != null)
                m_eState = GameState.PlayerXWins;
            else if (CheckPlayerWins('O') != null)
                m_eState = GameState.PlayerOWins;
            else if (TestForEmptyCells())
                m_eState = GameState.IsPlaying;
            else
                m_eState = GameState.IsDraw;
        }

        public float EvaluateTable(char chPlayer)
        {

            if ((chPlayer == 'X') && (m_eState == GameState.PlayerXWins))
                return 1f;

            if ((chPlayer == 'O') && (m_eState == GameState.PlayerOWins))
                return 1f;

            if ((chPlayer == 'X') && (m_eState == GameState.PlayerOWins))
                return -1f;

            if ((chPlayer == 'O') && (m_eState == GameState.PlayerXWins))
                return -1f;

            return 0f;
        }

    }
}
