using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace LabyrinthExplorer
{
    public class Chest : EnvironmentObject
    {
        private bool isClosed;
        private bool subForOpen = false;
        private float openTimer = 0;
        private float openedDuration = 0;

        private Model closedModel;
        private Model openModel;
        
        AABB openingAABB;
        
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
        }

        public override void Update(float deltaTime)
        {
            if (!isClosed)
            {
                openedDuration += deltaTime;

                if (openedDuration >= 5.0f)
                {
                    Close();
                }
            }
            else if (isClosed && subForOpen)
            {
                openTimer += deltaTime;
                if (openTimer >= 0.5f)
                {
                    Open();
                    subForOpen = false;
                }
            }
        }
        
        private void Open()
        {
            Game.SoundManager.PlaySound("ChestOpen");
            base.SetModel(openModel);
            isClosed = false;
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
            else if (openingDirection == Vector3.Up)
            {
                minPoint = new Vector3(Position.X - sideVal, 0, Position.Z);
                maxPoint = new Vector3(Position.X + sideVal, GameConstants.WALL_HEIGHT, Position.Z + outVal);
            }
            else //Down
            {
                minPoint = new Vector3(Position.X - sideVal, 0, Position.Z - outVal);
                maxPoint = new Vector3(Position.X + sideVal, GameConstants.WALL_HEIGHT, Position.Z + 0);
            }
            openingAABB = new AABB(minPoint, maxPoint);
        }

        public void Open(Camera cam)
        {
            if (isClosed)
            {
                if (Vector3.Distance(base.Position, cam.Position) < 50)
                    //if is within chestroom, or something more precise then 50
                {
                    subForOpen = true;
                    openTimer = 0;
                }
            }
        }

        public void Close()
        {
            Game.SoundManager.PlaySound("ChestClose");
            base.SetModel(closedModel);
            isClosed = true;
        }
    }
}
