using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BulletSharp;
using Vector = OpenTK.Vector3;
using Matrix = OpenTK.Matrix4;

namespace Vienna.Physics
{
    public class ActorMotionState : MotionState
    {
        private Matrix worldToPositionTransform { get; set; }

        public ActorMotionState(Matrix startingTransform)
        {
            worldToPositionTransform = startingTransform;
        }

        public override Matrix WorldTransform
        {
            get
            {
                return worldToPositionTransform;
            }
            set
            {
                worldToPositionTransform = value;
            }
        }
    }
}
