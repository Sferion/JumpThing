﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;


namespace JumpThing
{
    class PlayerSprite : Sprite
    {
        bool jumping, walking, falling, jumpIsPressed;
        const float jumpSpeed = 150f;
        const float walkSpeed = 100f;

        public PlayerSprite(Texture2D newSpriteSheet, Texture2D newCollisionTxr, Vector2 newLocation)
            : base(newSpriteSheet, newCollisionTxr, newLocation)
        {
            spriteOrigin = new Vector2(0.5f, 1f);
            isColliding = true;
            //drawCollision = true;
            collisionInsetMin = new Vector2(0.25f, 0.3f);
            collisionInsetMax = new Vector2(0.25f, 0f);

            frameTime = 0.2f;
            animations = new List<List<Rectangle>>();

            animations.Add(new List<Rectangle>());
            animations[0].Add(new Rectangle(0, 0, 48, 48));
            animations[0].Add(new Rectangle(48, 0, 48, 48));

            animations.Add(new List<Rectangle>());
            animations[1].Add(new Rectangle(48, 0, 48, 48));
            animations[1].Add(new Rectangle(96, 0, 48, 48));
            animations[1].Add(new Rectangle(48, 0, 48, 48));
            animations[1].Add(new Rectangle(144, 0, 48, 48));

            animations.Add(new List<Rectangle>());
            animations[2].Add(new Rectangle(96, 0, 48, 48));

            animations.Add(new List<Rectangle>());
            animations[3].Add(new Rectangle(0, 48, 48, 48));

            jumping = false;
            walking = false;
            falling = true;
            jumpIsPressed = false;
        }

        public void Update(GameTime gameTime, List<PlatformSprite> platforms)
        {
            if((falling || jumping) && spriteVelocity.Y < 500f) spriteVelocity.Y += 5f * (float)gameTime.ElapsedGameTime.TotalSeconds;
            spritePos += spriteVelocity;

            bool hasCollided = false;
            
            foreach (PlatformSprite platform in platforms)
            {
                if (checkCollisionBelow(platform))
                {
                    hasCollided = true;
                    while (checkCollision(platform)) spritePos.Y--;
                    spriteVelocity.Y = 0;
                    jumping = false;
                    falling = false;
                }
                else if (checkCollisionAbove(platform))
                {
                    hasCollided = true;
                    while (checkCollision(platform)) spritePos.Y++;
                    spriteVelocity.Y = 0;
                    jumping = false;
                    falling = true;
                }
                if (checkCollisionLeft(platform))
                {
                    hasCollided = true;
                    while (checkCollision(platform)) spritePos.X++;
                    spriteVelocity.X = 0;               
                }
                if (checkCollisionRight(platform))
                {
                    hasCollided = true;
                    while (checkCollision(platform)) spritePos.X--;
                    spriteVelocity.X = 0;
                }
                if (!hasCollided && walking) falling = true;
                if (jumping && spriteVelocity.Y > 0)
                {
                    jumping = false;
                    falling = true;
                }
            }

        }
        public void ResetPlayer(Vector2 newPos)
        {
            spritePos = newPos;
            spriteVelocity = new Vector2();
            jumping = false;
            walking = false;
            falling = true;

        }
    }
}
