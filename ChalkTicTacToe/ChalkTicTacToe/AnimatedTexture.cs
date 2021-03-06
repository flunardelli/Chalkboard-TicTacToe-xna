#region File Description
//-----------------------------------------------------------------------------
// AnimatedTexture.cs
//
// Microsoft XNA Community Game Platform
// Copyright (C) Microsoft Corporation. All rights reserved.
//-----------------------------------------------------------------------------
#endregion

using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;
using System.Diagnostics;

namespace ChalkTicTacToe
{
    public class AnimatedTexture
    {
        private int framecount;
        private Texture2D m_texture;
        private float TimePerFrame;
        private int Frame;
        private float TotalElapsed;
        private bool Paused;
        private bool drawFirstFrame;

        private Point m_frameSize;
        private Point m_sheetSize; 
        private Point m_currentFrame;

        public float Rotation, Scale, Depth;
        public Vector2 Origin;

        public SoundEffect m_sound;

        public AnimatedTexture(Vector2? origin = null, float rotation = 0,
            float scale = 1.0f, float depth = 0.5f)
        {
            this.Origin = origin ?? Vector2.Zero;
            this.Rotation = rotation;
            this.Scale = scale;
            this.Depth = depth;
            
            drawFirstFrame = false;
        }
        public void Load(Texture2D texture, 
            Point frameSize, Point sheetSize, Point? currentFrame = null, float framesPerSec = 30f, SoundEffect sound = null)
        {
            framecount = 6;
            m_frameSize = frameSize;
            m_sheetSize = sheetSize;
            m_currentFrame = currentFrame ?? new Point(0, 0);
            m_texture = texture;
            m_sound = sound;
            TimePerFrame = (float)1 / framesPerSec;
            Frame = 0;
            TotalElapsed = 0;
            Paused = false;
        }

        public void UpdateFrame(float elapsed)
        {
            if (Paused)
                return;
            TotalElapsed += elapsed;
            if (TotalElapsed > TimePerFrame && drawFirstFrame)
            {
                if (Frame == 0 && m_sound != null)
                {
                    m_sound.Play();
                }
                Frame++;
                Frame = Frame % framecount;
                TotalElapsed -= TimePerFrame;
                m_currentFrame.X++;
                if (m_currentFrame.X > m_sheetSize.X)
                {
                    m_currentFrame.X = 0;
                    m_currentFrame.Y++;
                    if (m_currentFrame.Y > m_sheetSize.Y)
                        m_currentFrame.Y = 0;
                }
                //Debug.WriteLine("frame update x{0} y{1} frame {2}", m_currentFrame.X, m_currentFrame.Y,Frame);
            }
            if (drawFirstFrame && Frame == 5)
            {
                this.Pause();
            }

        }

        public void DrawFrame(SpriteBatch batch, Vector2 screenPos)
        {
            DrawFrame(batch, Frame, screenPos);
        }
        public void DrawFrame(SpriteBatch batch, int frame, Vector2 screenPos)
        {
            Rectangle sourcerect = new Rectangle(m_currentFrame.X * m_frameSize.X,
                                   m_currentFrame.Y * m_frameSize.Y,
                                   m_frameSize.X,
                                   m_frameSize.Y);
            batch.Draw(m_texture, screenPos, sourcerect, Color.White,
                Rotation, Origin, Scale, SpriteEffects.None, Depth);
            drawFirstFrame = true;
            //Debug.WriteLine("frame draw x{0} y{1}", m_currentFrame.X, m_currentFrame.Y);            
        }

        public bool IsPaused
        {
            get { return Paused; }
        }
        public void Reset()
        {
            Frame = 0;
            TotalElapsed = 0f;
        }
        public void Stop()
        {
            Pause();
            Reset();
        }
        public void Play()
        {
            Paused = false;
        }
        public void Pause()
        {
            Paused = true;
        }

    }
}
