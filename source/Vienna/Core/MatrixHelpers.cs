using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenTK;

namespace Vienna.Core
{
    public static class MatrixHelpers
    {       
        public static Matrix4 CalculateMatrix(Matrix4 orginal, Vector3 Position, float ScaleFactor, float Rotation)
        {            
            var position = Matrix4.CreateTranslation(Position);
            var scale = Matrix4.Scale(ScaleFactor, ScaleFactor, 1);
            var rotation = Matrix4.CreateRotationZ(Rotation);
            return rotation * scale * position;             
        }
    }
}
