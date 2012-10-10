using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Vienna.Physics
{
    public class MaterialData
    {
        public float Restitution { get; protected set; }
        public float Friction { get; protected set; }
        public MaterialData(float restitution, float friction)
        {
            Restitution = restitution;
            Friction = friction;
        }
    }
}
