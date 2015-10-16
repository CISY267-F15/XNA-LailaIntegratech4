using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;

namespace XNA_LailaIntegratech4
{
    class PlayerManager
    {
        public Sprite playerSprite_front;
        public Sprite playerSprite_back;
        public Sprite playerSprite_left;
        public Sprite playerSprite_right;
        public Sprite activeSprite;

        private float playerSpeed = 160.0f;
        private Rectangle playerAreaLimit;

        public long PlayerScore = 0;
        public int LivesRemaining = 3;
        public bool Destroyed = false;

        private Vector2 gunOffset = new Vector2(25, 10);
        private float shotTimer = 0.0f;
        private float minShotTimer = 0.2f;
        private int playerRadius = 15;
        

        public PlayerManager(
            Texture2D texture,  
            Rectangle initialFrame,
            int frameCount,
            Rectangle screenBounds)
        {
            playerSprite_front = new Sprite(
                new Vector2(200, 200),
                texture,
                initialFrame,
                Vector2.Zero);

            for (int x = 1; x < frameCount; x++)
            {
                playerSprite_front.AddFrame(
                    new Rectangle(
                        initialFrame.X + (initialFrame.Width * x),
                        initialFrame.Y,
                        initialFrame.Width,
                        initialFrame.Height));
            }
            playerSprite_front.CollisionRadius = playerRadius;

            playerSprite_back = new Sprite(
                new Vector2(200, 200),
                texture,
                initialFrame,
                Vector2.Zero);

            for (int x = 1; x < frameCount; x++)
            {
                playerSprite_back.AddFrame(
                    new Rectangle(
                        initialFrame.X + (initialFrame.Width * x),
                        initialFrame.Y,
                        initialFrame.Width,
                        initialFrame.Height));
            }
            playerSprite_back.CollisionRadius = playerRadius;

            playerAreaLimit =
                new Rectangle(
                    0,
                    0,
                    screenBounds.Width,
                    screenBounds.Height);


        }

        private void FireShot()
        {
            if (shotTimer >= minShotTimer)
            {
                //PlayerShotManager.FireShot(
                //    playerSprite.Location + gunOffset,
                //    new Vector2(0, -1),
                //    true);
                //shotTimer = 0.0f;
            }
        }

        private void HandleKeyboardInput(KeyboardState keyState)
        {
            if (keyState.IsKeyDown(Keys.Up))
            {
                playerSprite_front.Velocity += new Vector2(0, -1);
            }

            if (keyState.IsKeyDown(Keys.Down))
            {
                playerSprite_front.Velocity += new Vector2(0, 1);
            }

            if (keyState.IsKeyDown(Keys.Left))
            {
                playerSprite_front.Velocity += new Vector2(-1, 0);
            }

            if (keyState.IsKeyDown(Keys.Right))
            {
                playerSprite_front.Velocity += new Vector2(1, 0);
            }

            if (keyState.IsKeyDown(Keys.Space))
            {
                FireShot();
            }
        }

        private void HandleGamepadInput(GamePadState gamePadState)
        {
            playerSprite_front.Velocity +=
                new Vector2(
                    gamePadState.ThumbSticks.Left.X,
                    -gamePadState.ThumbSticks.Left.Y);

            if (gamePadState.Buttons.A == ButtonState.Pressed)
            {
                FireShot();
            }
        }

        private void imposeMovementLimits()
        {
            Vector2 location = playerSprite_front.Location;

            if (location.X < playerAreaLimit.X)
                location.X = playerAreaLimit.X;

            if (location.X >
                (playerAreaLimit.Right - playerSprite_front.Source.Width))
                location.X =
                    (playerAreaLimit.Right - playerSprite_front.Source.Width);

            if (location.Y < playerAreaLimit.Y)
                location.Y = playerAreaLimit.Y;

            if (location.Y >
                (playerAreaLimit.Bottom - playerSprite_front.Source.Height))
                location.Y =
                    (playerAreaLimit.Bottom - playerSprite_front.Source.Height);

            playerSprite_front.Location = location;
        }

        public void Update(GameTime gameTime)
        {
            //PlayerShotManager.Update(gameTime);

            if (!Destroyed)
            {
                playerSprite_front.Velocity = Vector2.Zero;

                shotTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;

                HandleKeyboardInput(Keyboard.GetState());
                HandleGamepadInput(GamePad.GetState(PlayerIndex.One));

                playerSprite_front.Velocity.Normalize();
                playerSprite_front.Velocity *= playerSpeed;

                playerSprite_front.Update(gameTime);
                imposeMovementLimits();
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            //PlayerShotManager.Draw(spriteBatch);

            if (!Destroyed)
            {
                playerSprite_front.Draw(spriteBatch);
            }
        }

    }
}
