using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

namespace JasonElectricEffect
{
    public class PrimitiveUtil
    {
        public static VertexPositionColor[] DrawLine(
            List<ElectricNode> nodes,
            int segments, Color color)
        {
            EnsureInitialization();

            int verticesCount = (nodes.Count - 1) * segments;
            VertexPositionColor[] vertices = new VertexPositionColor[verticesCount];

            for (int i = 0; i < (nodes.Count - 1); i++)
            {
                float amount = 0.0f;
                for (int j = 0; j < segments; j++)
                {
                    float x = nodes[i + 1].Position.X * amount + nodes[i].Position.X * (1f - amount);
                    float y = nodes[i + 1].Position.Y * amount + nodes[i].Position.Y * (1f - amount);
                    vertices[i * segments + j] = new VertexPositionColor();
                    vertices[i * segments + j].Position = new Vector3(x, y, 0);
                    vertices[i * segments + j].Color = color;
                    amount += 1f / segments;
                }
            }

            SharedGraphicsDeviceManager.Current.GraphicsDevice.DrawUserPrimitives<VertexPositionColor>(PrimitiveType.LineStrip, vertices, 0, verticesCount - 1);

            return vertices;
        }

        public static VertexPositionColor[] DrawBezierCurve(
            List<ElectricNode> nodes,
            int segments, Color color)
        {
            EnsureInitialization();

            Vector2 point1 = nodes[2].Position;
            Vector2 point2 = nodes[1].Position;
            Vector2 point3 = nodes[0].Position;

            int verticesCount = (nodes.Count - 1) * segments;
            VertexPositionColor[] vertices = new VertexPositionColor[verticesCount];

            for (int i = 0; i < nodes.Count - 1; i++)
            {
                int idx = (i + 2) >= nodes.Count ? nodes.Count - 1 : (i + 2);
                point1 = nodes[idx].Position;
                point2 = nodes[i + 1].Position;
                point3 = nodes[i].Position;

                float amount = 0f;
                for (int j = 0; j < segments; j++)
                {
                    float x = (float)Math.Pow(1 - amount, 2) * point3.X + 2.0f * (1 - amount) * amount * point2.X + amount * amount * point1.X;
                    float y = (float)Math.Pow(1 - amount, 2) * point3.Y + 2.0f * (1 - amount) * amount * point2.Y + amount * amount * point1.Y;
                    vertices[i * segments + j] = new VertexPositionColor();
                    vertices[i * segments + j].Position = new Vector3(x, y, 0);
                    vertices[i * segments + j].Color = color;
                    amount += 0.5f / segments;
                }
            }

            SharedGraphicsDeviceManager.Current.GraphicsDevice.DrawUserPrimitives<VertexPositionColor>(PrimitiveType.LineStrip, vertices, 0, verticesCount - 1);
            return vertices;
        }

        public static VertexPositionColor[] DrawCatmullRomCurve(
            List<ElectricNode> nodes,
            int segments,
            Color color)
        {
            EnsureInitialization();

            Vector2 point1 = nodes[0].Position;
            Vector2 point2 = nodes[0].Position;
            Vector2 point3 = nodes[0].Position;
            Vector2 point4 = nodes[0].Position;

            int verticesCount = (nodes.Count - 1) * segments;
            VertexPositionColor[] vertices = new VertexPositionColor[verticesCount];
            Vector2 point;
            for (int i = 2; i < nodes.Count + 1; i++)
            {
                int idx = i >= nodes.Count ? nodes.Count - 1 : i;
                point1 = nodes[idx].Position;

                if (i >= 1)
                {
                    idx = (i - 1) >= nodes.Count ? nodes.Count - 1 : (i - 1);
                    point2 = nodes[idx].Position;
                }

                if (i >= 2)
                {
                    idx = (i - 2) >= nodes.Count ? nodes.Count - 1 : (i - 2);
                    point3 = nodes[idx].Position;
                }
                if (i >= 3)
                {
                    idx = (i - 3) >= nodes.Count ? nodes.Count - 1 : (i - 3);
                    point4 = nodes[idx].Position;
                }

                float amount = 0.0f;
                for (int j = 0; j < segments; j++)
                {
                    point = Vector2.CatmullRom(point4, point3, point2, point1, amount);

                    vertices[(i - 2) * segments + j] = new VertexPositionColor();
                    vertices[(i - 2) * segments + j].Position = new Vector3(point.X, point.Y, 0);
                    vertices[(i - 2) * segments + j].Color = color;
                    amount += 1.0f / segments;
                }
            }

            SharedGraphicsDeviceManager.Current.GraphicsDevice.DrawUserPrimitives<VertexPositionColor>(PrimitiveType.LineStrip, vertices, 0, verticesCount - 1);
            return vertices;
        }

        private static void EnsureInitialization()
        {
            var device = SharedGraphicsDeviceManager.Current.GraphicsDevice;
            var basicEffect = new BasicEffect(device);
            basicEffect.Projection = Matrix.CreateOrthographicOffCenter
                (0, device.Viewport.Width,
                device.Viewport.Height, 0,
                0, 1);
            basicEffect.CurrentTechnique.Passes[0].Apply();
        }

    }
}
