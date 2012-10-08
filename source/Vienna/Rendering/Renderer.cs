using System;
using System.Collections.Generic;
using System.Drawing;
using OpenTK.Graphics;
using Vienna.Actors;
using Vienna.Maps;
using Vienna.Sprites;

namespace Vienna.Rendering
{
    public class Renderer
    {
        private readonly Dictionary<Batch, BatchBuffer> _buffers = new Dictionary<Batch, BatchBuffer>();

        public void Initialize(int width, int height)
        {
            Console.WriteLine("OpenGL v{0}", GL.GetString(StringName.Version));
            Console.WriteLine("Shader Model v{0}", GL.GetString(StringName.ShadingLanguageVersion));
            Console.WriteLine("Device: {0}", GL.GetString(StringName.Vendor));

            GL.ClearColor(Color.Black);
            GL.Enable(EnableCap.Blend);
            
            GL.BlendFunc(BlendingFactorSrc.SrcAlpha, BlendingFactorDest.OneMinusSrcAlpha);

            AddBuffer(TileBuffer.CreateTestObject());
            AddBuffer(SpriteBuffer.CreateTestObject());
        }

        public void AddBuffer(BatchBuffer buffer)
        {
            _buffers.Add(buffer.Target, buffer);
        }

        public void ClearBuffer()
        {
            foreach (var buffer in _buffers.Values)
            {
                buffer.Destroy();
            }
            _buffers.Clear();
        }

        public void BindToBuffer(Batch target, Actor actor)
        {
            if(!actor.CanRender) return;
            var buffer = _buffers[target];
            buffer.Add(actor);
        }

        public void Render(double time, Camera camera)
        {
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            foreach (var buffer in _buffers.Values)
            {
                buffer.Bind();
                buffer.Update();
                buffer.Render(time, camera);
            }

            GraphicsContext.CurrentContext.SwapBuffers();
        }
    }
}
