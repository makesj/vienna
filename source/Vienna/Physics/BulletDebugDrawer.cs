using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BulletSharp;
using Vector = OpenTK.Vector3;
using Matrix = OpenTK.Matrix4;

namespace Vienna.Physics
{
    public class BullettDebugDrawer : BulletSharp.DebugDraw
    {        
        public override DebugDrawModes DebugMode { get; set; }

        public override void Draw3dText(ref Vector location, string textString)
        {
            System.Diagnostics.Trace.WriteLine(string.Format("BullettDebugDrawer-Draw3dText: location:{0} textString{1}", location, textString));
        }       

        public override void ReportErrorWarning(string warningString)
        {
            System.Diagnostics.Trace.WriteLine(string.Format("BullettDebugDrawer-ReportError Warning:{0}", warningString));
        }

        public void ReadOptions()
        {
            throw new NotImplementedException();
        }

        public override void DrawContactPoint(ref Vector pointOnB, ref Vector normalOnB, float distance, int lifeTime, OpenTK.Graphics.Color4 color)
        {
            System.Diagnostics.Trace.WriteLine(string.Format("BullettDebugDrawer-DrawContactPoint: pointOnB:{0} normalOnB:{1} distance:{2} lifeTime:{3} color{4}", pointOnB, normalOnB, distance, lifeTime, color));
        }

        public override void DrawLine(ref Vector from, ref Vector to, OpenTK.Graphics.Color4 color)
        {
            System.Diagnostics.Trace.WriteLine(string.Format("BullettDebugDrawer-DrawLine: From:{0} To:{1} Color:{2}", from, to, color));
        }
    }
}
