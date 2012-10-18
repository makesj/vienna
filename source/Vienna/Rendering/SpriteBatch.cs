using System;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using Vienna.Actors;
using Vienna.Extensions;

namespace Vienna.Rendering
{
    public class SpriteBatch
    {
        public int PoolSize { get; private set; }
        
        private readonly SpriteBatcher _batcher;
        private readonly Shader _shader;

        private BlendState _blendState;
        private SpriteSortMode _sortMode;
        private Camera _camera;
        private TextureAtlas _atlas;
        private bool _beginCalled;

        public SpriteBatch(int poolSize, Shader shader)
        {
            PoolSize = poolSize;
            _shader = shader;
            _batcher = new SpriteBatcher(PoolSize, shader);
        }

        public void Begin(TextureAtlas atlas, Camera camera)
        {
            if (_beginCalled) throw new Exception("Begin called twice without a call to end.");

            _atlas = atlas;
            _camera = camera;
            _blendState = BlendState.AlphaBlend;
            _sortMode = SpriteSortMode.None;
            _beginCalled = true;
        }

        public void Draw(Actor actor)
        {
            if (!actor.CanTranform || !actor.CanRender)
            {
                Console.WriteLine("Actor must be able to transform and render in order to be drawn.");
                return;
            }

            var transform = actor.TransformComponent.GetTransform();
            var renderer = actor.RenderComponent;
            Draw(renderer.Vertices, renderer.Depth, renderer.Frame, ref transform);
        }

        public void Draw(Vector2[] position, int depth, int frame, ref Matrix4 transform)
        {
            if(position.Length != 4) throw new Exception("Sprites must have 4 position vertices.");

            var t = _atlas.GetFrame(frame);

            var item = _batcher.CreateBatchItem();
            item.Depth = depth;

            item.Vertices[0].Position = position[0].Transform(ref transform);
            item.Vertices[0].Texture = t[0];
            item.Vertices[1].Position = position[1].Transform(ref transform);
            item.Vertices[1].Texture = t[1];
            item.Vertices[2].Position = position[2].Transform(ref transform);
            item.Vertices[2].Texture = t[2];
            item.Vertices[3].Position = position[3].Transform(ref transform);
            item.Vertices[3].Texture = t[3];

            if (_batcher.IsFull) Flush();            
        }

        public void End()
        {
            GL.Disable(EnableCap.Blend);

            if (_blendState == BlendState.NonPremultiplied)
            {
                GL.BlendFunc(BlendingFactorSrc.SrcAlpha, BlendingFactorDest.OneMinusSrcAlpha);
                GL.Enable(EnableCap.Blend);
            }

            if (_blendState == BlendState.AlphaBlend)
            {
                GL.BlendFunc(BlendingFactorSrc.One, BlendingFactorDest.OneMinusSrcAlpha);
                GL.Enable(EnableCap.Blend);
            }

            if (_blendState == BlendState.Additive)
            {
                GL.BlendFunc(BlendingFactorSrc.SrcAlpha, BlendingFactorDest.One);
                GL.Enable(EnableCap.Blend);
            }            

            GL.BindTexture(TextureTarget.Texture2D, _atlas.Texture.Handle);
            GL.UseProgram(_shader.Handle);

            _shader.SetUniformMatrix4("projection_matrix", ref _camera.ProjectionMatrix);
            _shader.SetUniformMatrix4("view_matrix", ref _camera.ViewMatrix);

            _batcher.DrawBatch(_sortMode);

            GL.BindTexture(TextureTarget.Texture2D, 0);
            GL.UseProgram(0);
            _beginCalled = false;
        }

        public void Flush()
        {
            End();
            Begin(_atlas, _camera);
        }
    }
}
