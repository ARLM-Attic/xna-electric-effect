using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;
using System;
using Microsoft.Xna.Framework;

namespace JasonElectricEffect
{
    public class ElectricFlow
    {
        #region Properties

        public List<ElectricNode> ElectricNodes = new List<ElectricNode>();
        SpriteBatch _SpriteBatch = null;
        private List<Texture2D> Textures = null;
        private Texture2D CurrentTexture = null;
        public TimeSpan LifeSpan = TimeSpan.FromSeconds(0.2);
        public TimeSpan Age = TimeSpan.Zero;
        public Color LineColor = Color.White;
        public int Density = 10;
        public ElectricEffectType EffectType = ElectricEffectType.CatmullRom;

        #endregion

        #region Constructor

        /// <summary>
        /// LifeSpan controls the frequency of the texture switching
        /// </summary>
        /// <param name="spriteBatch"></param>
        /// <param name="textures"></param>
        /// <param name="lifeSpan"></param>
        public ElectricFlow(SpriteBatch spriteBatch, List<Texture2D> textures, TimeSpan? lifeSpan = null)
        {
            _SpriteBatch = spriteBatch;
            Textures = textures;
            if (lifeSpan != null)
            {
                LifeSpan = (TimeSpan)lifeSpan;
            }

            Reset();
        }

        #endregion

        #region Update & Draw

        public void Update(TimeSpan elapsedTime, TimeSpan totalTime)
        {
            if (Age < LifeSpan)
            {
                Age += elapsedTime;
            }
            else
            {
                Reset();
            }

            foreach (var node in ElectricNodes)
            {
                node.Update(elapsedTime, totalTime);
            }
        }

        public void Draw(TimeSpan elapsedTime, TimeSpan totalTime)
        {
            if (ElectricNodes.Count < 1)
            {
                return;
            }

            VertexPositionColor[] points = null;
            switch (EffectType)
            {
                case ElectricEffectType.Line:
                    points = PrimitiveUtil.DrawLine(ElectricNodes, Density, LineColor);
                    break;
                case ElectricEffectType.Bezier:
                    points = PrimitiveUtil.DrawBezierCurve(ElectricNodes, Density, LineColor);
                    break;
                case ElectricEffectType.CatmullRom:
                    points = PrimitiveUtil.DrawCatmullRomCurve(ElectricNodes, Density, LineColor);
                    break;
                default:
                    break;
            }

            Vector2 position = new Vector2();
            Vector2 origin = new Vector2() { X = CurrentTexture.Width / 2, Y = CurrentTexture.Width / 2 };
            Color color = new Color(128, 128, 128, random.Next(32, 192));
            foreach (var point in points)
            {
                position.X = point.Position.X;
                position.Y = point.Position.Y;

                _SpriteBatch.Draw(CurrentTexture, position, null, color, 0f, origin, 1f, SpriteEffects.None, 0);
            }
        }

        #endregion

        private static Random random = new Random();
        private void Reset()
        {
            Age = TimeSpan.Zero;
            CurrentTexture = Textures[random.Next(Textures.Count)];
        }

        public void AddNode(Vector2 fiducialPoint, float radius, float? speedFactor = null)
        {
            if (speedFactor == null)
            {
                speedFactor = 1f;
            }
            ElectricNode node = new ElectricNode(fiducialPoint, radius, speedFactor);//, CurrentTexture);
            this.ElectricNodes.Add(node);
        }

    }
}
