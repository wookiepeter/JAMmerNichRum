﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFML;
using SFML.Graphics;
using SFML.Window;

namespace GameProject2D
{
    public class Player
    {
        public CircleShape circle;
        AnimatedSprite sprite;
        Vector2f position { get { return circle.Position; } set { circle.Position = value; } }
        Vector2f movement { get; set; }
        //Vector2f size { get { return sprite.Size; } set { sprite.Size = value; } }

        Vector2f gravity = new Vector2f(0F, 2.2F); //Gravitation
        bool isJumping = true;
        int index; //Nummerirung der Spieler

        

        public Player(Vector2f position, int index) //Konstruktor
        {
            this.circle = new CircleShape(40.0F);
            if (index == 2)
            {
                this.circle.FillColor = new Color(150, 0, 2);
            }
            else
            {
                this.circle.FillColor = new Color(0, 50, 255);
            }

            this.position = position;
            this.movement = new Vector2f(0F, 0F);
            this.index = index;

            ChangeSprite(Animation.running);
            //this.size = new Vector2f(50F, 50F);
        }

        enum Animation
        {
            jumping,
            running
        }

        private void ChangeSprite(Animation animation)
        {
            Vector2 FrameSize;

            switch (animation)
            {
                case Animation.running:
                    FrameSize = new Vector2(124, 125);
                    if (index == 1)
                        sprite = new AnimatedSprite(AssetManager.GetTexture(AssetManager.TextureName.Farmer1Running), 0.02F, 32, FrameSize);
                    else
                    {
                        sprite = new AnimatedSprite(AssetManager.GetTexture(AssetManager.TextureName.Farmer2Running), 0.02F, 32, FrameSize);
                        sprite.Scale = new Vector2(-1, 1);
                    }
                    sprite.Origin = FrameSize * 0.5F;
                    break;

                case Animation.jumping:
                    FrameSize = new Vector2(130, 125);
                    if (index == 1)
                        sprite = new AnimatedSprite(AssetManager.GetTexture(AssetManager.TextureName.Farmer1Jumping), 0.03F, 32, FrameSize);
                    else
                    {
                        sprite = new AnimatedSprite(AssetManager.GetTexture(AssetManager.TextureName.Farmer2Jumping), 0.03F, 32, FrameSize);
                        sprite.Scale = new Vector2(-1, 1);
                    }
                    sprite.Origin = FrameSize * 0.5F;
                    break;

            }
        }

        public void update(float deltaTime)
        {
            //Console.WriteLine(position.Y);
            float speed = 650F; //Geschwindigkeit
            
            Vector2f inputMovement = new Vector2f(0F, 0F);

            bool startJumping = false;
             
            //inputMovement.Y += Keyboard.IsKeyPressed(Keyboard.Key.Down) ? 10 : 0F;
            //inputMovement.Y += Keyboard.IsKeyPressed(Keyboard.Key.Up) ? -speed : 0F;
            if (index == 2)
            {
                if (GamePadInputManager.IsConnected(1))
                {
                    inputMovement.X = GamePadInputManager.GetLeftStick(1).X;
                    if ((circle.Position.Y + circle.Radius * 2) > Program.win.Size.Y * 0.7F - 1)
                    {
                        if (isJumping)
                        {
                            ChangeSprite(Animation.running);
                        }
                        isJumping = false;
                    }
                    startJumping = !isJumping && GamePadInputManager.IsPressed(GamePadButton.A, 1);
                }

                else
                {
                    inputMovement.X += Keyboard.IsKeyPressed(Keyboard.Key.Left) ? -1 : 0F;
                    inputMovement.X += Keyboard.IsKeyPressed(Keyboard.Key.Right) ? 1 : 0F;

                    if ((circle.Position.Y + circle.Radius * 2) > Program.win.Size.Y * 0.7F - 1)
                    {
                        if (isJumping)
                        {
                            ChangeSprite(Animation.running);
                        }
                        isJumping = false;
                    }
                    startJumping = !isJumping && KeyboardInputManager.IsPressed(Keyboard.Key.Up);
                }
            }
            else
            {

                if (GamePadInputManager.IsConnected(0))
                {
                    inputMovement.X = GamePadInputManager.GetLeftStick(0).X;
                    if ((circle.Position.Y + circle.Radius * 2) > Program.win.Size.Y * 0.7F - 1)
                    {
                        if (isJumping)
                        {
                            ChangeSprite(Animation.running);
                        }
                        isJumping = false;
                    }
                    startJumping = !isJumping && GamePadInputManager.IsPressed(GamePadButton.A, 0);
                }

                else
                {
                    inputMovement.X += Keyboard.IsKeyPressed(Keyboard.Key.A) ? -1 : 0F;
                    inputMovement.X += Keyboard.IsKeyPressed(Keyboard.Key.D) ? 1 : 0F;

                    if ((circle.Position.Y + circle.Radius * 2) > Program.win.Size.Y * 0.7F - 1)
                    {
                        if (isJumping)
                        {
                            ChangeSprite(Animation.running);
                        }
                        isJumping = false;
                    }
                    startJumping = !isJumping && KeyboardInputManager.IsPressed(Keyboard.Key.W);
                }
            }

            if (startJumping)
            {
                ChangeSprite(Animation.jumping);
                Program.jumpsSound.Play();
                movement = new Vector2(inputMovement.X * speed, -750F); //Springhoehe
                isJumping = true;
            }
            else
                movement = new Vector2(inputMovement.X * speed, movement.Y + gravity.Y * speed * deltaTime);
            
            position += movement * deltaTime;



            

           if (position.Y < 0)
            {
                position = new Vector2f(position.X,0);
               // movement *= Vector2.Up;
            }            

             if (position.Y > Program.win.Size.Y*0.7F - circle.Radius*2)
            {
                position = new Vector2f (position.X, Program.win.Size.Y*0.7F - circle.Radius*2);
                //movement *= Vector2.Up;
            }

            if (index == 1)
            {
                if (position.X > (Program.win.Size.X *0.43F) - circle.Radius*1.7F)
                {
                    position = new Vector2f((Program.win.Size.X *0.43F) - circle.Radius*1.7F, position.Y);
                    //movement *= Vector2.Left;
                }
                if (position.X < Program.win.Size.X * 0.03F - circle.Radius/3)
                {
                    position = new Vector2f(Program.win.Size.X * 0.03F - circle.Radius/3, position.Y);
                    //movement *= Vector2.Left;
                }
            }
            if (index == 2)
            {
                if (position.X < ((1F - 0.43F) * Program.win.Size.X)  - circle.Radius/2.5F)
                {
                    position = new Vector2f(((1F - 0.43F) * Program.win.Size.X) - circle.Radius/2.5F, position.Y);
                    //movement *= Vector2.Left;
                }
                if (position.X > (1F-0.03F)*Program.win.Size.X - circle.Radius*1.7F)
                {
                    position = new Vector2f((1F - 0.03F)*Program.win.Size.X - circle.Radius*1.7F, position.Y);
                    //movement *= Vector2.Left;
                }
            }


        }

        public void draw(RenderWindow win, View view, float deltaTime)
        {
            sprite.UpdateFrame(deltaTime);
            sprite.Position = (Vector2) position + Vector2.One * circle.Radius;
            //Hardcoded values !!!! Caution!!!
            sprite.Position = sprite.Position - new Vector2f(-8,10);

            //win.Draw(circle);
            win.Draw(sprite);
        }
    }
}
