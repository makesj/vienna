using System;
using System.Collections.Generic;
using OpenTK.Graphics.OpenGL;
using Vienna.Actors;

namespace Vienna.Rendering
{
    public abstract class BatchBuffer
    {
        private byte[] _blocks;

        public int BufferSize { get; private set; }
        public int VertexPerObject { get; private set; }
        public int MaxObjects { get; private set; }
        public RenderPass RenderPass { get; private set; }
        public Batch Target { get; private set; }

        protected Dictionary<int, BatchBufferInstance> Instances;
        protected Shader Shader { get; private set; }
        protected TextureAtlas Atlas { get; private set; }

        protected int Vbohandle;
        protected int Vbahandle;
        protected int Ibohandle;
        
        public int SizeInBytes
        {
            get { return Instances.Count*VertexPerObject*Vertex.SizeInBytes; }
        }

        protected BatchBuffer(int maxObjects, int objectSize, RenderPass pass, Batch target, Shader shader, TextureAtlas atlas)
        {
            BufferSize = maxObjects * objectSize;
            VertexPerObject = objectSize;
            MaxObjects = objectSize;
            RenderPass = pass;
            Target = target;
            Shader = shader;
            Atlas = atlas;

            _blocks = new byte[BufferSize];
            Instances = new Dictionary<int, BatchBufferInstance>(BufferSize);
        }

        public abstract void Initialize();

        public abstract void Render(double time, Camera camera);

        public abstract void Process(BatchBufferInstance instance, Camera camera);

        public void Add(Actor actor)
        {
            var index = Allocate();
            if (index == -1)
            {
                Console.WriteLine("Sprite Buffer out of blocks!");
                return;
            }

            var instance = new BatchBufferInstance(actor, index, 0);
            Instances.Add(instance.Id, instance);
        }

        public void Remove(Actor actor)
        {
            //free the block and remove the id reference
            var instance = Instances[actor.Id];

            Release(instance.Index);

            Instances.Remove(instance.Id);
            
            //clear the gpu memory
            var temp = new Vertex[VertexPerObject];
            for (var i = 0; i < temp.Length; i++)
            {
                temp[i] = Vertex.Zero;
            }
            
            BufferData(instance, temp);
        }

        public void Destroy()
        {
            Instances.Clear();
            _blocks = null;
            BufferSize = 0;
            GL.DeleteBuffer(Vbohandle);
            GL.DeleteVertexArray(Vbahandle);
            GL.DeleteVertexArray(Ibohandle);
       }

        public void Update(Camera camera)
        {
            foreach (var instance in Instances.Values)
            {
                Process(instance, camera);
            }
        }

        public void Bind()
        {
            Shader.Bind();
            Atlas.Bind();
            GL.BindVertexArray(Vbahandle);
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, Ibohandle);
            GL.BindBuffer(BufferTarget.ArrayBuffer, Vbohandle);
        }

        protected void BufferData(BatchBufferInstance instance, Vertex[] vertices)
        {
            if (vertices.Length != VertexPerObject)
            {
                Console.WriteLine("{0} instance contains invalid amount of vertices", Target.ToString("g"));
                return;
            }

            instance.Length = vertices.Length;

            var vertexTotalBytes = new IntPtr(Vertex.SizeInBytes * vertices.Length);
            var vertexOffsetBytes = new IntPtr(Vertex.SizeInBytes * instance.Offset);

            GL.BufferSubData(BufferTarget.ArrayBuffer, vertexOffsetBytes, vertexTotalBytes, vertices);

            instance.Changed = false;
        }

        private int Allocate()
        {
            for (var index = 0; index < _blocks.Length; index++)
            {
                if (_blocks[index] != 0) continue;
                _blocks[index] = 1;
                return index;
            }
            return -1;
        }

        private void Release(int index)
        {
            _blocks[index] = 0;
        }

        protected int[] BuildIndices(int amount, int[] template, int stride)
        {
            var indices = new int[amount * template.Length];
            var counter = 0;

            for (var i = 0; i < amount; i++)
            {
                for (var j = 0; j < template.Length; j++)
                {
                    indices[counter] = template[j] + i * stride;
                    counter++;
                }
            }

            return indices;
        }
    }
}