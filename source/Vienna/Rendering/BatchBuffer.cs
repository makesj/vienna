﻿using System;
using System.Collections.Generic;
using OpenTK.Graphics.OpenGL;
using Vienna.Actors;

namespace Vienna.Rendering
{
    public abstract class BatchBuffer
    {
        private byte[] _blocks;

        public int BufferSize { get; private set; }
        public RenderPass RenderPass { get; private set; }
        public Batch Target { get; private set; }

        protected Dictionary<int, BatchBufferInstance> Instances;
        protected Shader Shader { get; private set; }
        protected TextureAtlas Atlas { get; private set; }

        protected int Length;
        protected int Vbohandle;
        protected int Vbahandle;
        

        protected BatchBuffer(int size, RenderPass pass, Batch target, Shader shader, TextureAtlas atlas)
        {
            BufferSize = size;
            RenderPass = pass;
            Target = target;
            Shader = shader;
            Atlas = atlas;

            _blocks = new byte[BufferSize];
            Instances = new Dictionary<int, BatchBufferInstance>(BufferSize);
        }

        public abstract void Initialize();

        public abstract void Render(double time, Camera camera);

        public int Allocate()
        {
            for (int index = 0; index < _blocks.Length; index++)
            {
                if (_blocks[index] != 0) continue;
                _blocks[index] = 1;
                return index;
            }
            return -1;
        }

        public void Add(Actor actor)
        {
            var index = Allocate();
            if (index == -1)
            {
                Console.WriteLine("Sprite Buffer out of blocks!");
                return;
            }

            var instance = new BatchBufferInstance(actor, index, 0);
            instance.BindBuffer(this);
            Instances.Add(instance.Id, instance);
        }

        public void Release(Actor actor)
        {
            Release(actor.Id);
        }

        public void Release(int id)
        {
            //free the block and remove the id reference
            var instance = Instances[id];

            _blocks[instance.Index] = 0;

            Instances.Remove(instance.Id);
            
            //clear the gpu memory
            var temp = new Vertex[6];
            var vertexTotalBytes = new IntPtr(Vertex.SizeInBytes * temp.Length);
            var vertexOffestBytes = new IntPtr(Vertex.SizeInBytes * instance.Index);
            

            GL.BufferSubData(BufferTarget.ArrayBuffer, vertexOffestBytes, vertexTotalBytes, IntPtr.Zero);
        }

        public abstract void Process(BatchBufferInstance instance);

        public void Destroy()
        {
            Instances.Clear();
            _blocks = null;
            BufferSize = 0;
            GlHelper.DeleteVertexArray(Vbahandle);
            GlHelper.DeleteBuffer(Vbohandle);
        }

        public void Update()
        {
            foreach (var instance in Instances.Values)
            {
                Process(instance);
            }
        }

        public void Bind()
        {
            Shader.Bind();
            Atlas.Bind();
            GL.BindVertexArray(Vbahandle);
            GL.BindBuffer(BufferTarget.ArrayBuffer, Vbohandle);
        }

        protected void BufferData(BatchBufferInstance instance, Vertex[] vertices)
        {
            UpdateBufferLength(instance, vertices);

            var vertexTotalBytes = new IntPtr(Vertex.SizeInBytes * instance.Length);
            var vertexOffsetBytes = new IntPtr(Vertex.SizeInBytes * instance.Offset);

            GL.BufferSubData(BufferTarget.ArrayBuffer, vertexOffsetBytes, vertexTotalBytes, vertices);

            instance.Changed = false;
        }

        private void UpdateBufferLength(BatchBufferInstance instance, Vertex[] vertices)
        {
            if (instance.Length != vertices.Length)
            {
                Length -= instance.Length;
                Length += vertices.Length;
                instance.Length = vertices.Length;
                if (Length > BufferSize)
                {
                    Console.WriteLine("{0} buffer out of memory", Target.ToString("g"));
                }
            }
        }
    }
}