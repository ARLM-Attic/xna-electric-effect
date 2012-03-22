using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.Phone.Controls;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using JasonElectricEffect;

namespace JasonElectricEffectDemo
{
    public partial class GamePage : PhoneApplicationPage
    {
        ContentManager contentManager;
        GameTimer timer;
        SpriteBatch spriteBatch;

        public GamePage()
        {
            InitializeComponent();

            // Get the content manager from the application
            contentManager = (Application.Current as App).Content;

            // Create a timer for this page
            timer = new GameTimer();
            timer.UpdateInterval = TimeSpan.FromTicks(333333);
            timer.Update += OnUpdate;
            timer.Draw += OnDraw;
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            // Set the sharing mode of the graphics device to turn on XNA rendering
            SharedGraphicsDeviceManager.Current.GraphicsDevice.SetSharingMode(true);

            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(SharedGraphicsDeviceManager.Current.GraphicsDevice);

            // TODO: use this.content to load your game content here
            InitElectricEffect();

            // Start the timer
            timer.Start();

            base.OnNavigatedTo(e);
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            // Stop the timer
            timer.Stop();

            // Set the sharing mode of the graphics device to turn off XNA rendering
            SharedGraphicsDeviceManager.Current.GraphicsDevice.SetSharingMode(false);

            base.OnNavigatedFrom(e);
        }

        /// <summary>
        /// Allows the page to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        private void OnUpdate(object sender, GameTimerEventArgs e)
        {
            // TODO: Add your update logic here
            electricEffect.Update(e.ElapsedTime, e.TotalTime);
        }

        /// <summary>
        /// Allows the page to draw itself.
        /// </summary>
        private void OnDraw(object sender, GameTimerEventArgs e)
        {
            SharedGraphicsDeviceManager.Current.GraphicsDevice.Clear(Color.Black);

            // TODO: Add your drawing code here
            electricEffect.Draw(e.ElapsedTime, e.TotalTime);
        }

        #region ElectricEffect

        List<Texture2D> electricPointTextures = new List<Texture2D>();
        ElectricEffect electricEffect = null;
        TimeSpan LifeSpan = TimeSpan.FromSeconds(0.2);

        private void InitElectricEffect()
        {
            for (int i = 1; i < 4; i++)
            {
                var texture = contentManager.Load<Texture2D>("ElectricPoint" + i.ToString());
                electricPointTextures.Add(texture);
            }

            electricEffect = new ElectricEffect(spriteBatch, electricPointTextures);
            electricEffect.Density = 15;
            electricEffect.EffectType = (App.Current as App).EffectType;
            int nodeCount = 10;
            for (int i = 0; i < (App.Current as App).ElectricFlowCount; i++)
            {
                ElectricFlow flow = electricEffect.AddFlow(LifeSpan);
                Vector2 position = new Vector2() { X = 240, Y = 50 };
                for (int j = 0; j < nodeCount; j++)
                {
                    /* the nearer to the center, the larger radius */
                    var radius = ((nodeCount - 1f) / 2 - Math.Abs(j - (nodeCount - 1f) / 2)) * 30;
                    flow.AddNode(position, radius, 1f);
                    position.X += 0;
                    position.Y += 80;
                }
            }

        }

        #endregion
    }
}