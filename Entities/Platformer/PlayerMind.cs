﻿using ADS.Entities;
using Engine.Events.CollisionEvent;
using Engine.Events.KeyboardEvent;
using Engine.Events.MouseEvent;
using Engine.Managers.Collision;
using Engine.Managers.EntityRelated;
using Engine.Tilemaps;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Entities
{
    public class PlayerMind : Mind
    {
        int bulletTimer = 0;
        int bulletSpeed = 35;
        GameTime GameTime;
        int maxSpeed = 4;
        bool input = true;
        bool isColliding = false;

        private List<Direction> directions = new List<Direction>();

        string currentDirection = "Up";
       public PlayerMind()
        {
            isCollidable = true;
            MouseHandler.Instance.MouseClick += OnMouseDown;
            KeyHandler.Instance.KeyDown += OnKeyDown;
            KeyHandler.Instance.KeyHeld += OnKeyHeld;
            DetectionManger.Instance.OnCollision += OnCollision;
        }



        public override void Update(GameTime gameTime)
        {
            if (bulletTimer > 0)
                bulletTimer = bulletTimer-1;
            GameTime = gameTime;
            applyVelocityRules();
            Friction();

            for(int i= 0; i < directions.Count; i ++)
            {
                Console.WriteLine(directions[i]);
            }
            directions.Clear();
            base.Update(gameTime);
        }


        public override void Unload()
        {
            KeyHandler.Instance.KeyDown -= OnKeyDown;
            KeyHandler.Instance.KeyHeld -= OnKeyHeld;
            MouseHandler.Instance.MouseClick -= OnMouseDown;
        }

        public void Friction()
        {
            //I have no idea how this works, but it simulates friction
            Velocity -= Velocity * new Vector2(.1f, .1f);
        }



        public void OnKeyDown(object sender, KeyEventArgs m)
        {
        

          
                
              

            //pseudo
           // EntityManager.Instance.createProjectileCamDrawable<bulletEntity>(Position, "bullet", this.DIRECTION);


        }

        public void OnKeyHeld(object sender, KeyEventArgs m)
        {
            if (input)
            {
                if (m.key == Keys.D)
                {
                    directions.Add(Direction.right);
                    e.Position = new Vector2(e.Position.X + maxSpeed, e.Position.Y);// *(float)GameTime.ElapsedGameTime.TotalMilliseconds;
                  //  velocity.X += Acceleration.X *(float)GameTime.ElapsedGameTime.TotalMilliseconds;
                }
                if (m.key == Keys.A)
                {
                    directions.Add(Direction.left);
                    e.Position = new Vector2(e.Position.X - maxSpeed, e.Position.Y);// *(float)GameTime.ElapsedGameTime.TotalMilliseconds;
                 //   velocity.X -= Acceleration.X * (float)GameTime.ElapsedGameTime.TotalMilliseconds;

                }
                if (m.key == Keys.S)
                {
                    directions.Add(Direction.down);
                    e.Position = new Vector2(e.Position.X , e.Position.Y+maxSpeed);// *(float)GameTime.ElapsedGameTime.TotalMilliseconds;
                  //  velocity.Y += Acceleration.Y * (float)GameTime.ElapsedGameTime.TotalMilliseconds;

                }

                if (m.key == Keys.W)
                {
                    directions.Add(Direction.up);
                    e.Position = new Vector2(e.Position.X , e.Position.Y - maxSpeed);// *(float)GameTime.ElapsedGameTime.TotalMilliseconds;

                  //  velocity.Y -= Acceleration.Y * (float)GameTime.ElapsedGameTime.TotalMilliseconds;
                }

                Console.WriteLine(bulletTimer);
                if (bulletTimer == 0)
                {
                    switch (m.key)
                    {
                        case Keys.Left:
                            bulletTimer = bulletSpeed;
                            EntityManager.Instance.createProjectile<Projectile>(new Vector2(this.Bounds.Center.X, this.Bounds.Center.Y), "bullet1", ADS.Entities.Direction.left);
                            break;
                        case Keys.Right:
                            bulletTimer = bulletSpeed;
                            EntityManager.Instance.createProjectile<Projectile>(new Vector2(this.Bounds.Center.X, this.Bounds.Center.Y), "bullet1", ADS.Entities.Direction.right);
                            break;
                        case Keys.Up:
                            bulletTimer = bulletSpeed;
                            EntityManager.Instance.createProjectile<Projectile>(new Vector2(this.Bounds.Center.X, this.Bounds.Center.Y), "bullet1", ADS.Entities.Direction.up);
                            break;
                        case Keys.Down:
                            bulletTimer = bulletSpeed;
                            EntityManager.Instance.createProjectile<Projectile>(new Vector2(this.Bounds.Center.X, this.Bounds.Center.Y), "bullet1", ADS.Entities.Direction.down);
                            break;



                    }

                }





            }

        }

        public void OnMouseDown(object sender, MouseEventArgs m)
        {
        }

        public void OnCollision(object sender, CollisionEventArgs cae )
        {
           //Position
            if (cae.A == this)
            {
                isColliding = true;
                Position += GetMinimumTranslation(cae.B);
            }

          
        }

        public Vector2 GetMinimumTranslation(ICollidable B1)
        {
            //Vector for the minimum translation distance
            Vector2 mtd = new Vector2();
            Rectangle A = this.Bounds;
            Rectangle B = B1.Bounds;
            //Calculate corners of both Bounding Boxes
            float xAMin = A.X;
            float xAMax = A.X + A.Width;
            float yAMin = A.Y;
            float yAMax = A.Y + A.Height;

            float xBMin = B.X;
            float xBMax = B.X + B.Width;
            float yBMin = B.Y;
            float yBMax = B.Y + B.Height;


            float left = (xBMin - xAMax);
            float right = (xBMax - xAMin);
            float top = (yBMin - yAMax);
            float bottom = (yBMax - yAMin);

        //    if (left > 0 || right < 0) Console.WriteLine("no intersection");
            //if (top > 0 || bottom < 0) Console.WriteLine("no intersection");


            //Select direction that we need to move the ICollidable back by
            if (Math.Abs(left) < right)
            {
                mtd.X = left;

            }
            else
            {

                mtd.X = right;

            }

            if (Math.Abs(top) < bottom)
            {
                mtd.Y = top;

            }
            else
            {
                mtd.Y = bottom;
            }

            // 0 is the axis with the largest translation value/depth
            if (Math.Abs(mtd.X) < Math.Abs(mtd.Y))
            {
                mtd.Y = 0;
            }
            else
            {
                mtd.X = 0;

            }


            return mtd;
        }


        public void applyVelocityRules()
        {
            if (velocity.X > maxSpeed)
            {
                velocity.X = maxSpeed;
            }

            if (velocity.X < -maxSpeed)
            {
                velocity.X = -maxSpeed;
            }

            if (velocity.Y > maxSpeed)
            {
                velocity.Y = maxSpeed;
            }

            if (velocity.Y < -maxSpeed)
            {
                velocity.Y = -maxSpeed;
            }
        }
    }
}
              
    


