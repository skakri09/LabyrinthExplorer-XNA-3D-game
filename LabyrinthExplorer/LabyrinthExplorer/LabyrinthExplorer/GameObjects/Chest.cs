﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace LabyrinthExplorer
{
    public class Chest : EnvironmentObject, IInteractableObject
    {
        private bool isClosed;

        private Model closedModel;
        private Model openModel;
        
        public Chest(ContentManager content,
                     Vector3 position, Vector3 rotation,
                    float scale, Vector3 openFromDirection,
                    bool isClosed = true)
            : base(@"Models\Environment\ChestClosed", 
                content, position, rotation, scale)
        {
            this.isClosed = isClosed;
            closedModel = base.GetModel();
            openModel = content.Load<Model>(@"Models\Environment\ChestOpen");
            CreateOpeningAABB(openFromDirection);
            Interactables.AddInteractable(this);
        }

        private void CreateOpeningAABB(Vector3 openingDirection)
        {
            Vector3 minPoint, maxPoint;
            float outVal = 100; //distance from middle of chest and in the direction of aabb
            float sideVal = 100;//distance from middle of chest and out in both sideways directions

            if(openingDirection == Vector3.Left)
            {
                minPoint = new Vector3(Position.X - outVal, 0, Position.Z - sideVal);
                maxPoint = new Vector3(Position.X, GameConstants.WALL_HEIGHT, Position.Z + sideVal);
            }
            else if (openingDirection == Vector3.Right)
            {
                minPoint = new Vector3(Position.X, 0, Position.Z - sideVal);
                maxPoint = new Vector3(Position.X + outVal, GameConstants.WALL_HEIGHT, Position.Z + sideVal);
            }
            else if (openingDirection == Vector3.Forward)
            {
                minPoint = new Vector3(Position.X - sideVal, 0, Position.Z);
                maxPoint = new Vector3(Position.X + sideVal, GameConstants.WALL_HEIGHT, Position.Z + outVal);
            }
            else //Down
            {
                minPoint = new Vector3(Position.X - sideVal, 0, Position.Z - outVal);
                maxPoint = new Vector3(Position.X + sideVal, GameConstants.WALL_HEIGHT, Position.Z + 0);
            }
            SetAABB(minPoint, maxPoint);
        }

        public void Use()
        {
            if (isClosed)
            {
                Game.SoundManager.PlaySound("ChestOpen");
                base.SetModel(openModel);
                isClosed = false;
            }
            else
            {
                Game.SoundManager.PlaySound("ChestClose");
                base.SetModel(closedModel);
                isClosed = true;
            }
        }
    
    }
}
