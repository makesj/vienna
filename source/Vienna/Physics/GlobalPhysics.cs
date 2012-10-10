using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Vienna.Physics
{
    public class GlobalPhysics
    {
        private static IGamePhysics _instance;
        public static IGamePhysics Instance
        {
            get
            {
                if (_instance == null)
                {
                    throw new InvalidOperationException("An Instance of IGamePhysics has not been registered");
                }
                return _instance;
            }
        }
        public static IGamePhysics Register(IGamePhysics physics)
        {
            _instance = physics;
            return Instance;
        }

        public static void UnRegister()
        {            
            _instance = null;
        }

    }
}
