using System.Collections.Generic;
using Vienna.Rendering;

namespace Vienna.Actors
{
    public class Actor
    {
        public IRenderingComponent RenderComponent { get; private set; }
        public ITransformComponent TransformComponent { get; private set; }

        protected Dictionary<int, IComponent> Components = new Dictionary<int, IComponent>();
        public int Id { get; private set; }
        public string Name { get; private set; }
        public bool IsDestroyed { get; private set; }

        public Actor(int id, string name)
        {
            Id = id;
            Name = name;
        }

        public T GetComponent<T>(int componentId) where T : class, IComponent
        {
            return Components[componentId] as T;
        }

        public void AddComponent(IComponent component)
        {
            if (Components.ContainsKey(component.Id))
            {
                Components[component.Id] = component;
            }
            else
            {
                Components.Add(component.Id, component);                
            }

            // Most actors will most likely have rendering and tranform components

            if (component is IRenderingComponent)
                RenderComponent = component as IRenderingComponent;

            if (component is ITransformComponent)
                TransformComponent = component as ITransformComponent;
        }

        public void Initialize()
        {
            foreach (var component in Components)
            {
                component.Value.Initialize(this);
            }
        }

        public void Update(double time)
        {
            foreach (var component in Components)
            {
                component.Value.Update(time);
            }
        }

        public void Destroy()
        {
            foreach (var component in Components)
            {
                component.Value.Destroy();
            }
            IsDestroyed = true;
        }

        public bool CanTranform
        {
            get { return TransformComponent != null; }
        }

        public bool CanRender
        {
            get { return RenderComponent != null; }
        }

        public override string ToString()
        {
            return string.Format("{1}({0})", Name, Id);
        }
    }
}
