using OpenTK;
using Vienna.Actors;

namespace Vienna.Rendering
{
    public class BatchBufferInstance
    {
        public Actor Actor { get; private set; }
        public int Id { get; private set; }
        public int Length { get; set; }
        public int Index { get; private set; }

        public int Offset
        {
            get { return Index * Length; }
        }

        public bool CanTransform
        {
            get { return Actor.CanTranform; }
        }

        public bool Changed
        {
            get
            {
                var r = Actor.CanRender ? Actor.RenderComponent.Changed : false;
                var t = Actor.CanTranform ? Actor.TransformComponent.Changed : false;
                return t || r;
            }
            set
            {
                if(Actor.CanRender)
                {
                    Actor.RenderComponent.Changed = value;
                }
                if (Actor.CanTranform)
                {
                    Actor.TransformComponent.Changed = value;
                }
            }
        }

        public Vector2[] Vertices
        {
            get { return Actor.RenderComponent.Vertices; }
        }

        public Vector2[] Normals
        {
            get { return Actor.RenderComponent.Normals; }
        }

        public int Frame
        {
            get { return Actor.RenderComponent.Frame; }
        }

        public BatchBufferInstance(Actor actor, int index, int length)
        {
            Id = actor.Id;
            Actor = actor;
            Index = index;
            Length = length;
        }

        public void BindBuffer(BatchBuffer buffer)
        {
            Actor.RenderComponent.Buffer = buffer;
        }

        public Matrix4 GetTransform()
        {
            return Actor.TransformComponent.GetTransform();
        }
    }
}
