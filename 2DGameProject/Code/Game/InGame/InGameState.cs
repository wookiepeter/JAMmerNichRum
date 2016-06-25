﻿using System;
using SFML;
using SFML.Graphics;
using SFML.Window;
using System.Collections.Generic;

namespace GameProject2D
{
    class InGameState : IGameState
    {
        Player player;
        Player player2;
        Background background;
        List<Plant> plants;
        Vector2 collisionPoint;
        
        public InGameState()
        {
            player = new Player(new Vector2f(50F, 10F),1);
            player2 = new Player(new Vector2f(680F, 10F),2); //neuer Spieler erstellt
            background = new Background();
            plants = new List<Plant>();

            plants.Add(new Plant(25F));
            plants.Add(new Plant(200F));
            plants.Add(new Plant(550F));
            plants.Add(new Plant(700F));
        }

        public GameState Update(float deltaTime)
        {
            player.update(deltaTime);
            player2.update(deltaTime);
            return GameState.InGame;

            //if (DoCollide(p, s, out collisionPoint))
            {

            }

        }

        public void Draw(RenderWindow win, View view, float deltaTime)
        {
            background.Draw(win, view);
            foreach (Plant t in plants) //t - variable
            {
                t.Draw(win, view);
            }
            player.draw(win, view);
            player2.draw(win, view);
        }

        public void DrawGUI(GUI gui, float deltaTime)
        {
        }
        private bool DoCollide(CircleShape a, CircleShape b, out Vector2 collisionPoint)
        {
            Vector2 deltaA = b.Position - a.Position; //Streckenlänge berechnen
            Vector2 deltaB = a.Position - b.Position;
            float radiusSum = a.Radius + b.Radius;

            if (deltaA.lengthSqr <= radiusSum * radiusSum) //radiusSumme quadrieren, um Math.sqrt zu umgehen
            {
                //Schnittpunkt = Kreismittelpunkt + r * Vektor mit Länge 1
                Vector2 pointA = (Vector2)a.Position + a.Radius * deltaA.normalized;
                Vector2 pointB = (Vector2)b.Position + b.Radius * deltaB.normalized;
                collisionPoint = (pointB + pointA) / 2;
                return true;
            }
            else
            {
                collisionPoint = Vector2.Zero; //Random Wert
                return false;
            }
        }
    }
}
