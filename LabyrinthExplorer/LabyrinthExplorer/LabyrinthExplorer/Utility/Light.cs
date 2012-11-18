using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace LabyrinthExplorer
{
    public enum LightType
    {
        DirectionalLight,
        PointLight,
        SpotLight
    }

    /// <summary>
    /// A light. This light structure is the same as the one defined in
    /// the parallax_normal_mapping.fx file. The only difference is the
    /// LightType enum.
    /// </summary>
    public struct Light
    {
        public LightType Type;
        public Vector3 Direction;
        public Vector3 Position;
        public Color Ambient;
        public Color Diffuse;
        public Color Specular;
        public float SpotInnerConeRadians;
        public float SpotOuterConeRadians;
        public float Radius;
    }
}
