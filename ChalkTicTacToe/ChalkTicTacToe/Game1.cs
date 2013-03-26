using System;
using System.Collections.Generic;
using System.Linq;
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
    public enum GameState { IsPlaying, IsDraw, PlayerXWins, PlayerOWins };

    public enum GameFlow { MainMenu, GameStart, PlayerMove, ComputerMove, Results };

    public enum GameMode { OnePlayer, TwoPlayers };

    public class Game1 : Microsoft.Xna.Framework.Game
    {

        GraphicsDeviceManager m_graphics;
        SpriteBatch m_spriteBatch;
        
        Texture2D m_GridCell, m_Tex_O, m_Tex_X, m_Line;
        Texture2D m_Background;
        Texture2D m_BackgroundGrid;

        Vector2 m_vTableCenter;

        Table m_Table;

        GameFlow m_eGameFlow = GameFlow.MainMenu;

        MouseState m_oPrevMouseState = Mouse.GetState();

        Main m_mainMenu;
        Score m_score;

        Point m_frameSize = new Point(150, 150);

        AnimatedTexture m_spriteLineH, m_spriteLineV, m_spriteLineDL, m_spriteLineDR;

        SoundEffect m_soundX, m_soundO, m_soundClick;

        GameMode m_gameMode = GameMode.OnePlayer;

        float m_computerMoveDelay;
        float m_currentMatchTime = 0;

        char m_currentPlayer;
        char m_chComputer = 'O';
        char m_chPlayer = 'X';

        bool m_increaseScore = true;

        public Game1()
        {

            m_graphics = new GraphicsDeviceManager(this);

            m_graphics.PreferredBackBufferWidth = 800;
            m_graphics.PreferredBackBufferHeight = 520;
            m_graphics.ApplyChanges();

            m_vTableCenter = new Vector2(500.0f, 240.0f);

            Content.RootDirectory = "Content";

            IsMouseVisible = true;

            //m_Table.m_achCells[ 0, 0 ] = 'X';
            //m_Table.m_achCells[ 2, 2 ] = 'O';
            Window.Title = "Tic Tac Toe";

            Reset();

        }

        protected override void Initialize()
        {
            base.Initialize();
        }

        protected override void LoadContent()
        {

            m_spriteBatch = new SpriteBatch(GraphicsDevice);

            m_mainMenu = new Main(this);

            m_score = new Score(this);

            m_GridCell = Content.Load<Texture2D>(@"sprites/BlackboardGridCell");

            m_Tex_X = Content.Load<Texture2D>(@"sprites/SheetX");
            m_Tex_O = Content.Load<Texture2D>(@"sprites/SheetO");

            m_Line = Content.Load<Texture2D>(@"sprites/SheetLine");

            m_Background = Content.Load<Texture2D>(@"sprites/Blackboard");
            m_BackgroundGrid = Content.Load<Texture2D>(@"sprites/BlackboardGrid");

            m_spriteLineH = new AnimatedTexture();
            m_spriteLineH.Load(m_Line, new Point(510, 35), new Point(2, 1));

            m_spriteLineV = new AnimatedTexture(rotation: 45.55f);
            m_spriteLineV.Load(m_Line, new Point(510, 35), new Point(2, 1));

            m_spriteLineDL = new AnimatedTexture(rotation: 44.8f);
            m_spriteLineDL.Load(m_Line, new Point(510, 35), new Point(2, 1));

            m_spriteLineDR = new AnimatedTexture(rotation: 2.35f);
            m_spriteLineDR.Load(m_Line, new Point(510, 35), new Point(2, 1));

            m_soundX = Content.Load<SoundEffect>(@"sounds/SoundX");
            m_soundO = Content.Load<SoundEffect>(@"sounds/SoundO");
            m_soundClick = Content.Load<SoundEffect>(@"sounds/SoundClick");
        }

        protected override void UnloadContent() { }

        protected override void Update(GameTime gameTime)
        {

            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            MouseState oMouseState = Mouse.GetState();

            switch (m_eGameFlow)
            {
                case GameFlow.GameStart:
                    m_score.HandleTouch(oMouseState);
                    break;
                case GameFlow.PlayerMove:
                    if ((oMouseState.LeftButton == ButtonState.Pressed) &&
                        (m_oPrevMouseState.LeftButton != ButtonState.Pressed))
                    {
                        if (PlayerMove(oMouseState, m_currentPlayer))
                        {
                            if (m_Table.m_eState == GameState.IsPlaying)
                            {
                                if (m_gameMode == GameMode.OnePlayer)
                                {
                                    m_eGameFlow = GameFlow.ComputerMove;
                                }
                                m_currentPlayer = (m_currentPlayer == m_chPlayer) ? m_chComputer : m_chPlayer;
                                m_computerMoveDelay = (float)gameTime.TotalGameTime.TotalMilliseconds + 1500;
                            }
                            else
                            {
                                m_eGameFlow = GameFlow.Results;
                            }
                        }
                    }
                    m_oPrevMouseState = oMouseState;
                    break;

                case GameFlow.ComputerMove:
                    if (gameTime.TotalGameTime.TotalMilliseconds > m_computerMoveDelay)
                    {
                        ComputerMove(m_currentPlayer);
                        m_currentPlayer = (m_currentPlayer == m_chPlayer) ? m_chComputer : m_chPlayer;
                        if (m_Table.m_eState == GameState.IsPlaying)
                        {
                            m_eGameFlow = GameFlow.PlayerMove;
                        }
                        else
                        {
                            m_eGameFlow = GameFlow.Results;
                        }

                    }
                    break;

                case GameFlow.MainMenu:
                    m_mainMenu.HandleTouch(oMouseState);
                    break;

                case GameFlow.Results:
                    m_score.HandleTouch(oMouseState);
                    if (m_increaseScore)
                    {
                        m_score.IncreaseScore(m_Table.m_eState.ToString());
                        m_increaseScore = false;
                    }
                    break;

            }
            //Debug.WriteLine(" gametime update ", gameTime.TotalGameTime.TotalMilliseconds);
            m_currentMatchTime += (float)gameTime.TotalGameTime.TotalMinutes;
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {

            GraphicsDevice.Clear(Color.CornflowerBlue);
            //float elapsed = (float)gameTime.ElapsedGameTime.TotalMilliseconds;            
            float elapsed = (float)gameTime.ElapsedGameTime.TotalSeconds;

            m_spriteBatch.Begin();

            m_spriteBatch.Draw(m_Background, Vector2.Zero, Color.White);

            switch (m_eGameFlow)
            {
                case GameFlow.MainMenu:
                    m_mainMenu.Draw(m_spriteBatch, gameTime);
                    break;
                case GameFlow.GameStart:                    
                case GameFlow.PlayerMove:
                case GameFlow.ComputerMove:
                case GameFlow.Results:

                    m_score.Draw(m_spriteBatch, gameTime);

                    if (m_eGameFlow == GameFlow.GameStart)
                    {
                        m_score.DrawStartButton(m_spriteBatch, gameTime);
                    }
                    else {
                        switch (m_Table.m_eState)
                        {
                            case GameState.IsPlaying:
                                if (m_currentPlayer == m_chPlayer)
                                {
                                    m_score.DrawMessage(m_spriteBatch, gameTime, 0);
                                }
                                else
                                {
                                    m_score.DrawMessage(m_spriteBatch, gameTime, 1);
                                }
                                break;
                            case GameState.PlayerXWins:
                                m_score.DrawMessage(m_spriteBatch, gameTime, 2);
                                
                                break;
                            case GameState.PlayerOWins:
                                m_score.DrawMessage(m_spriteBatch, gameTime, 3);
                                
                                break;
                            case GameState.IsDraw:
                                m_score.DrawMessage(m_spriteBatch, gameTime, 4);
                                
                                break;
                        }
                    
                    }

                    if (m_Table.m_eState == GameState.PlayerOWins || m_Table.m_eState == GameState.PlayerXWins || m_Table.m_eState == GameState.IsDraw)
                    {
                        m_score.DrawMenuButton(m_spriteBatch, gameTime);
                    }
               
                    m_spriteBatch.Draw(m_BackgroundGrid, new Vector2(280, 20), Color.White);
                    Vector2 vCellSize = new Vector2(m_GridCell.Width, m_GridCell.Height);                  
                    Vector2 vXSize = new Vector2(m_frameSize.X, m_frameSize.Y);             
                    Vector2 vOSize = new Vector2(m_frameSize.X, m_frameSize.Y);
                    Vector2 vCorner = m_vTableCenter - vCellSize * ((float)m_Table.m_nSide * 0.5f);                 

                    for (int nY = 0; nY < m_Table.m_nSide; nY++)
                    {
                        for (int nX = 0; nX < m_Table.m_nSide; nX++)
                        {

                            Vector2 vGridPos = new Vector2(nX, nY);
                            Vector2 vFinalPos = vCorner + vCellSize * vGridPos;

                            //draw cell background
                            m_spriteBatch.Draw(m_GridCell, vFinalPos, Color.White);

                            if (m_Table.m_achCells[nX, nY].m_chPlayer == m_chPlayer)
                            { //draw X symbol:
                                Vector2 vOffset = 0.5f * (vCellSize - vXSize);
                                AnimatedTexture spriteMove = m_Table.m_achCells[nX, nY].m_sprite;
                                spriteMove.DrawFrame(m_spriteBatch, vFinalPos + vOffset);
                                spriteMove.UpdateFrame(elapsed);
                                // Debug.WriteLine(" player {0}", m_playerMarksPos);
                            }

                            if (m_Table.m_achCells[nX, nY].m_chPlayer == m_chComputer)
                            { //draw O symbol:
                                Vector2 vOffset = 0.5f * (vCellSize - vOSize);
                                AnimatedTexture spriteMove = m_Table.m_achCells[nX, nY].m_sprite;
                                spriteMove.DrawFrame(m_spriteBatch, vFinalPos + vOffset);
                                spriteMove.UpdateFrame(elapsed);
                            }
                            //Debug.WriteLine("move {0} - player {1} x{2} y{3}",m_currentMove,m_Table.m_achCells[nX, nY], nX,nY);
                        }
                    }

                    if (m_eGameFlow == GameFlow.Results)
                    {
                        if (m_Table.m_eState == GameState.PlayerOWins || m_Table.m_eState == GameState.PlayerXWins)
                        {
                            string po = m_Table.CheckPlayerWins(m_chPlayer);
                            if (po == null)
                            {
                                po = m_Table.CheckPlayerWins(m_chComputer);
                            }

                            switch (po)
                            {
                                case "L0":
                                    m_spriteLineH.DrawFrame(m_spriteBatch, new Vector2(240, 70)); //L0
                                    m_spriteLineH.UpdateFrame(elapsed);
                                    break;
                                case "L1":
                                    m_spriteLineH.DrawFrame(m_spriteBatch, new Vector2(250, 210)); //L1
                                    m_spriteLineH.UpdateFrame(elapsed);
                                    break;
                                case "L2":
                                    m_spriteLineH.DrawFrame(m_spriteBatch, new Vector2(250, 360)); //L2
                                    m_spriteLineH.UpdateFrame(elapsed);
                                    break;
                                case "C0":
                                    m_spriteLineV.DrawFrame(m_spriteBatch, new Vector2(385, 0)); //C0
                                    m_spriteLineV.UpdateFrame(elapsed);
                                    break;
                                case "C1":
                                    m_spriteLineV.DrawFrame(m_spriteBatch, new Vector2(520, 0)); //C1
                                    m_spriteLineV.UpdateFrame(elapsed);
                                    break;
                                case "C2":
                                    m_spriteLineV.DrawFrame(m_spriteBatch, new Vector2(670, 0)); //C2
                                    m_spriteLineV.UpdateFrame(elapsed);
                                    break;
                                case "DL":
                                    m_spriteLineDL.DrawFrame(m_spriteBatch, new Vector2(340, 40));
                                    m_spriteLineDL.UpdateFrame(elapsed);
                                    break;
                                case "DR":
                                    m_spriteLineDR.DrawFrame(m_spriteBatch, new Vector2(700, 65));
                                    m_spriteLineDR.UpdateFrame(elapsed);
                                    break;
                            }
                            //Debug.WriteLine(" po {0} state {1}", po, m_Table.m_eState);
                        }
                    }                 
                    break;
            }
            
            m_spriteBatch.End();
            base.Draw(gameTime);
        }

        public void handleScoreStartClick() 
        {
            //Debug.WriteLine("ScoreStart");
            m_soundClick.Play();

            if (m_gameMode == GameMode.OnePlayer)
            {
                if (m_currentPlayer == m_chPlayer)
                {
                    m_eGameFlow = GameFlow.PlayerMove;
                }
                else
                {
                    m_eGameFlow = GameFlow.ComputerMove;
                }

            }
            else {
                m_eGameFlow = GameFlow.PlayerMove;
            }
            

        }

        public void handleScoreRematchClick()
        {
            //Debug.WriteLine("ScoreReset");
            //m_soundClick.Play();
            m_eGameFlow = GameFlow.GameStart;
            Reset();
        }

        public void handleScoreMenuClick()
        {
            //Debug.WriteLine("ScoreReset");
            m_soundClick.Play();
            m_eGameFlow = GameFlow.MainMenu;
            m_score.ResetScore();
            Reset();                        
        }

        public void handleMenuClick(int id)
        {
            //Debug.WriteLine("Menu Click {0}", id);
            m_soundClick.Play();
            m_increaseScore = true;
            if (id == Main.MODE1_ID)
            {
                m_gameMode = GameMode.OnePlayer;
                m_eGameFlow = GameFlow.GameStart;
                m_currentPlayer = m_chPlayer;
            } 
            else if (id == Main.MODE1B_ID)
            {
                m_gameMode = GameMode.OnePlayer;
                m_eGameFlow = GameFlow.GameStart;
                m_currentPlayer = m_chComputer;
            }
            else if (id == Main.MODE2_ID)
            {
                m_gameMode = GameMode.TwoPlayers;
                m_eGameFlow = GameFlow.GameStart;
                m_currentPlayer = m_chPlayer;
            }
            else if (id == Main.QUIT_ID)
            {
                Quit();
            }
        }

        bool PlayerMove(MouseState mouseState, char chPlayer)
        {

            Vector2 vMousePos = new Vector2((float)mouseState.X, (float)mouseState.Y);
            Vector2 vCellSize = new Vector2(m_GridCell.Width, m_GridCell.Height);
            Vector2 vXSize = new Vector2(m_frameSize.X, m_frameSize.Y);
            Vector2 vOSize = new Vector2(m_frameSize.X, m_frameSize.Y);
            Vector2 vCorner = m_vTableCenter - vCellSize * ((float)m_Table.m_nSide * 0.5f);

            for (int nY = 0; nY < m_Table.m_nSide; nY++)
            {
                for (int nX = 0; nX < m_Table.m_nSide; nX++)
                {
                    Vector2 vGridPos = new Vector2(nX, nY);
                    Vector2 vFinalPos = vCorner + vCellSize * vGridPos;
                    Vector2 vEndPos = vFinalPos + vCellSize;

                    if ((vMousePos.X > vFinalPos.X) &&
                        (vMousePos.X < vEndPos.X) &&
                        (vMousePos.Y > vFinalPos.Y) &&
                        (vMousePos.Y < vEndPos.Y))
                    {

                        if (m_Table.m_achCells[nX, nY].m_chPlayer == ' ')
                        {
                            AnimatedTexture spriteMark = new AnimatedTexture();
                            
                            Texture2D texture;
                            SoundEffect sound;
                            if (chPlayer == m_chPlayer){
                                texture = m_Tex_X;
                                sound = m_soundX;
                            } else {
                                texture = m_Tex_O;
                                sound = m_soundO;
                            }
                            spriteMark.Load(texture, m_frameSize, new Point(2, 1),sound:sound);
                            m_Table.ApplyMove(new Move(nX, nY, chPlayer, spriteMark));
                            return true;
                        }
                    }
                }
            }
            return false;
        }

        void ComputerMove(char chComputer)
        {
            Move bestMove;
            Table TestTable = new Table(m_Table);
            float s = Minimax(TestTable, out bestMove, float.MinValue, float.MaxValue, chComputer);
            AnimatedTexture spriteMark = new AnimatedTexture();
            Texture2D texture;
            SoundEffect sound;
            if (chComputer == m_chComputer)
            {
                texture = m_Tex_O;
                sound = m_soundO;
            }
            else
            {
                texture = m_Tex_X;
                sound = m_soundX;
            }
            spriteMark.Load(texture, m_frameSize, new Point(2, 1),sound:sound);
            bestMove.m_sprite = spriteMark;
            Debug.WriteLine("bestMove {0}x{1} score: {2}", bestMove.m_nX, bestMove.m_nY, s);
            m_Table.ApplyMove(bestMove);
        }

        void Quit()
        {
            this.Exit();
        }

        void Reset()
        {
            m_currentMatchTime = 0;
            m_Table = new Table(3);
            m_increaseScore = true;
            //m_currentPlayer = m_chPlayer;
        }

        public float Minimax(Table TestTable, out Move best, float alpha, float beta, char chPlayer, int depth = 1)
        {
            char opponent = (chPlayer == m_chComputer) ? m_chPlayer : m_chComputer;

            best = null;
            var bestResult = -10f;
            Move TmpMove;

            if (TestTable.m_eState != GameState.IsPlaying)
            {
                return TestTable.EvaluateTable(chPlayer) / depth;
            }

            List<Move> lMoves = new List<Move>();
            TestTable.GetAvailableMoves(lMoves, chPlayer);

            foreach (Move move in lMoves)
            {
                Table TestTable2 = new Table(TestTable);
                TestTable2.ApplyMove(move);
                alpha = -Minimax(TestTable2, out TmpMove, -beta, -alpha, opponent, depth + 1);
                //Debug.WriteLine("move: {0}x{1} alpha: {2} depth: {3} player: {4} state: {5}",move.m_nX,move.m_nY,alpha,depth,chPlayer,TestTable2.m_eState);
                if (beta <= alpha)
                {
                    return alpha;
                }
                if (alpha > bestResult)
                {
                    best = move;
                    bestResult = alpha;
                }
            }
            return bestResult;
        }

    }
}
