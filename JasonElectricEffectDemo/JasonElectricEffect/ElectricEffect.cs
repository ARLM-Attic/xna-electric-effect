using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace JasonElectricEffect
{
    public class ElectricEffect
    {
        #region Properties

        public List<ElectricFlow> ElectricFlows = new List<ElectricFlow>();
        public List<Texture2D> Textures = new List<Texture2D>();
        SpriteBatch _SpriteBatch = null;
        public float SpeedFactor = 1f;
        public int Density = 10;
        public ElectricEffectType EffectType = ElectricEffectType.CatmullRom;

        #endregion

        #region Constructor

        public ElectricEffect(SpriteBatch spriteBatch, List<Texture2D> textures)
        {
            _SpriteBatch = spriteBatch;
            Textures = textures;
        }

        #endregion

        #region Update & Draw

        public void Update(TimeSpan elapsedTime, TimeSpan totalTime)
        {
            foreach (var flow in ElectricFlows)
            {
                flow.Update(elapsedTime, totalTime);
            }
        }

        public void Draw(TimeSpan elapsedTime, TimeSpan totalTime)
        {
            _SpriteBatch.Begin(SpriteSortMode.BackToFront,BlendState.Additive);
            foreach (var flow in ElectricFlows)
            {
                flow.Draw(elapsedTime, totalTime);
            }
            _SpriteBatch.End();
        }

        #endregion

        /// <summary>
        /// LifeSpan controls the frequency of the texture switching
        /// </summary>
        /// <param name="lifeSpan"></param>
        /// <returns></returns>
        public ElectricFlow AddFlow(TimeSpan lifeSpan)
        {
            ElectricFlow flow = new ElectricFlow(_SpriteBatch, Textures,lifeSpan);
            flow.EffectType = this.EffectType;
            flow.Density = Density;
            this.ElectricFlows.Add(flow);
            return flow;
        }

    }
}
