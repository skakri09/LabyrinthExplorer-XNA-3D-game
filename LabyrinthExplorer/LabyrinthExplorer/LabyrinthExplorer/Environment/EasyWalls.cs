using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace LabyrinthExplorer.EasyWalls
{

    //Creates a wall along the x axis. The startPos must have the lowest X value.
    //The wall will grow 50px/width out on negative Z axis.
    public class XWallNegZ : SolidWall
    {
        //Creates a wall along the x axis. The startPos must have the lowest X value.
        //The wall will grow 50px/width out on negative Z axis.
        public XWallNegZ(GraphicsDevice device, Vector2 startPos, Vector2 endPos, float width = 50.0f)
            :base
            (
            device, startPos, endPos,
            new Vector2(endPos.X, endPos.Y + width),
            new Vector2(startPos.X, startPos.Y + width)
            )
        {
        }
    }

    //Creates a wall along the x axis. The startPos must have the lowest X value.
    //The wall will grow 50px/width out on positive Z axis.
    public class XWallPosZ : SolidWall
    {
        //Creates a wall along the x axis. The startPos must have the lowest X value.
        //The wall will grow 50px/width out on positive Z axis.
        public XWallPosZ(GraphicsDevice device, Vector2 startPos, Vector2 endPos, float width = 50.0f)
            : base
            (
            device,
            new Vector2(startPos.X, startPos.Y + width),
            new Vector2(endPos.X, endPos.Y + width),
            endPos, startPos
            )
        {
        }
    }

    //Creates a wall along the Z axis. Startpos must have the highest Z value.
    //The wall will grow 50px/width out on positive X axis
    public class ZWallPosX : SolidWall
    {
        //Creates a wall along the Z axis. Startpos must have the highest Z value.
        //The wall will grow 50px/width out on positive X axis
        public ZWallPosX(GraphicsDevice device, Vector2 startPos, Vector2 endPos, float width = 50.0f)
            :base
            (
            device, 
            startPos,
            new Vector2(startPos.X + width, startPos.Y),
            new Vector2(endPos.X + width, endPos.Y),
            endPos)
        {
        }
    }

    //Creates a wall along the Z axis. Startpos must have the highest Z value.
    //The wall will grow 50px/width out on negative X axis
    public class ZWallNegX : SolidWall
    {
        //Creates a wall along the Z axis. Startpos must have the highest Z value.
        //The wall will grow 50px/width out on negative X axis
        public ZWallNegX(GraphicsDevice device, Vector2 startPos, Vector2 endPos, float width = 50.0f)
            : base
            (
            device,
            new Vector2(startPos.X - width, startPos.Y),
            startPos,
            endPos,
            new Vector2(endPos.X - width, endPos.Y)
            )
        {

        }
    }
}
