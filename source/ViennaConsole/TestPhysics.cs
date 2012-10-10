using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vienna.Physics;
using Vienna.Actors;

namespace ViennaConsole
{
    [TestCase(CaseNumber = 12)]
    public class TestPhysics
    {

        public void Execute()
        {
            TestInitialize();
            TestComponent();
        }

        private void TestInitialize()
        {
            var physics = new BulletGamePhysics();
            physics.Initialize();
        }

        private void TestComponent()
        {            
         
        }

    
    }
}
