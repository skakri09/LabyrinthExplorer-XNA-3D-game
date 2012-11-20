using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace LabyrinthExplorer
{
    public class Lever : EnvironmentObject, IInteractableObject
    {
        private bool isUsed;

        private Model closedModel;
        private Model usedModel;

        public Lever(ContentManager content,
                     Vector3 position, Vector3 rotation,
                    float scale, Vector3 openFromDirection,
                    bool isUsed = false)
            : base(@"Models\Environment\Lever",
                content, position, rotation, scale)
        {
            this.isUsed = isUsed;
            closedModel = base.GetModel();
            usedModel = content.Load<Model>(@"Models\Environment\LeverUsed");
            CreateUseAABB(openFromDirection);
            Interactables.AddInteractable(this);
        }

        private void CreateUseAABB(Vector3 useDirection)
        {
            Vector3 minPoint, maxPoint;
            float outVal = 100; //distance from middle of chest and in the direction of aabb
            float sideVal = 100;//distance from middle of chest and out in both sideways directions

            if (useDirection == Vector3.Left)
            {
                minPoint = new Vector3(Position.X - outVal, 0, Position.Z - sideVal);
                maxPoint = new Vector3(Position.X, GameConstants.WALL_HEIGHT, Position.Z + sideVal);
            }
            else if (useDirection == Vector3.Right)
            {
                minPoint = new Vector3(Position.X, 0, Position.Z - sideVal);
                maxPoint = new Vector3(Position.X + outVal, GameConstants.WALL_HEIGHT, Position.Z + sideVal);
            }
            else if (useDirection == Vector3.Up)
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
            if (!isUsed)
            {
                Game.SoundManager.PlaySound("LeverUsed");
                base.SetModel(usedModel);
                isUsed = true;
            }
        }

    }
}
