using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace JasonElectricEffect
{
    public class ElectricNode
    {
        #region Properties

        Vector2 FiducialPoint { get; set; }
        public Vector2 Position { get; set; }
        public float Radius = 10;
        public Vector2 Velocity { get; set; }
        private float SpeedFactor = 1f;

        #endregion

        #region Constructor

        public ElectricNode(Vector2 fiducialPoint, float radius, float? speedFactor=null)
        {
            FiducialPoint = fiducialPoint;
            Radius = radius;
            SpeedFactor = speedFactor??1f;

            Reset();
        }

        #endregion

        #region Update & Draw

        public void Update(TimeSpan elapsedTime, TimeSpan totalTime)
        {
            if (MovementTime < MovementDuration)
            {
                Position += Velocity;
                MovementTime++;
            }
            else
            {
                Reset();
            }
        }

        #endregion

        #region Movement

        private static Random random = new Random();
        private static int MinSpeed = 1;
        private static int MaxSpeed = 10;
        private int MovementDuration = 0;
        private int MovementTime = 0;

        public void Reset()
        {
            //reset position
            this.Position = FiducialPoint;
            
            //reset Velocity
            Vector2 rawVelocity = new Vector2(random.Next(MinSpeed, MaxSpeed), 0);
            float angle = MathHelper.ToRadians(random.Next(360));
            Velocity = Vector2.Transform(rawVelocity, Matrix.CreateRotationZ(angle));
            Velocity = Velocity * SpeedFactor;
            MovementDuration = (int)(Radius / Velocity.Length());
            MovementTime = 0;
        }

        #endregion

    }
}
