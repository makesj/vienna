using System;
using Vienna.Actors;

namespace Vienna.AI
{
    public class SpinnerComponent : IComponent
    {
        public const int ComponentId = 3000;
        public int Id { get { return ComponentId; } }
        public Actor Parent { get; protected set; }

        public float Rotation { get; protected set; }
        public float Y { get; protected set; }
        public float X { get; protected set; }
        public double Time { get; protected set; }

        public bool Rotate;
        public bool MoveX;
        public bool MoveY;
        public int Speed;
        private Random Rand;


        public void Initialize(Actor parent)
        {
            Parent = parent;
            Rand = new Random(parent.Id);  
            RandomizeMovement();
        }

        public void Update(double time)
        {
            var transform = Parent.GetComponent<TransformComponent>(TransformComponent.ComponentId);

            Time += time;

            if (Time < 5)
            {
                if (Rotate) Rotation += 0.05f;
                if (MoveY) Y = Speed;
                if (MoveX) X = Speed;
            }
            else if (Time > 5 && Time < 10)
            {
                if (Rotate) Rotation -= 0.05f;
                if (MoveY) Y = -Speed;
                if (MoveX) X = -Speed;
            }
            else
            {
                Time = 0;
                RandomizeMovement();
                X = 0;
                Y = 0;
            }

            transform.Move(X, Y);
            transform.Rotate(Rotation);
        }

        private void RandomizeMovement()
        {
            MoveX = Rand.Next(0, 9) > 3;
            MoveY = Rand.Next(0, 9) > 3;
            Rotate = Rand.Next(0, 9) > 1;
            Speed = Rand.Next(1, 5);
        }

        public void Destroy()
        {
        }
    }
}
