using System;
using System.Collections.Generic;
using OpenTK.Graphics.OpenGL;

namespace Vienna.Rendering
{
    public class SpriteBatcher
    {
        private readonly int _poolSize;
        private readonly int _bufferSize;
        private readonly int _indexSize;

        private readonly List<SpriteBatchItem> _items;
        private readonly Queue<SpriteBatchItem> _itemPool;
        private readonly Vertex[] _vertices;
        private readonly ushort[] _indices;
        
        private readonly int _bufferHandle;
        private readonly int _vertexHandle;
        private readonly int _indexHandle;
        
        public bool IsFull
        {
            get { return _items.Count >= _poolSize; }
        }

        public SpriteBatcher(int poolSize, Shader shader)
        {
            _poolSize = poolSize;
            _bufferSize = _poolSize * 4; // 4 vertices per sprite
            _indexSize = _poolSize * 6; // 6 elements per sprite

            _items = new List<SpriteBatchItem>(_poolSize);
            _itemPool = new Queue<SpriteBatchItem>(_poolSize);

            _vertices = new Vertex[_bufferSize]; 
            _bufferHandle = GlHelper.CreateBuffer(_vertices, BufferTarget.ArrayBuffer, BufferUsageHint.StreamDraw);

            _indices = new ushort[_indexSize]; 

            // generate a massive index array for quads (2 triangles)
            for (var i = 0; i < _poolSize; i++)
            {
                _indices[i * 6 + 0] = (ushort)(i * 4);
                _indices[i * 6 + 1] = (ushort)(i * 4 + 1);
                _indices[i * 6 + 2] = (ushort)(i * 4 + 2);
                _indices[i * 6 + 3] = (ushort)(i * 4 + 2);
                _indices[i * 6 + 4] = (ushort)(i * 4 + 1);
                _indices[i * 6 + 5] = (ushort)(i * 4 + 3);
            }

            _indexHandle = GlHelper.CreateBuffer(_indices, BufferTarget.ElementArrayBuffer, BufferUsageHint.DynamicDraw);

            var attributes = shader.GetAttributes(_bufferHandle);
            _vertexHandle = GlHelper.CreateVertexArray(attributes);       
        }

        public SpriteBatchItem CreateBatchItem()
        {
            // we use a queue so we're not newing up tons of objects every frame
            var item = _itemPool.Count > 0 ? _itemPool.Dequeue() : new SpriteBatchItem();
            _items.Add(item);
            return item;
        }

        public void DrawBatch(SpriteSortMode sortMode)
        {
            if (_items.Count == 0) return;
          
            SortItems(sortMode);

            var index = 0;

            // build the vertices array to send to the GPU
            foreach (var item in _items)
            {
                _vertices[index++] = item.Vertices[0];
                _vertices[index++] = item.Vertices[1];
                _vertices[index++] = item.Vertices[2];
                _vertices[index++] = item.Vertices[3];

                //add this item back to the pool
                _itemPool.Enqueue(item);
            }

            GL.BindVertexArray(_vertexHandle);
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, _indexHandle);

            Draw(index, _items.Count);
            
            _items.Clear();
        }

        private void Draw(int numOfVertices, int numOfItems)
        {
            var size = numOfVertices * Vertex.SizeInBytes;
            GL.BufferSubData(BufferTarget.ArrayBuffer, new IntPtr(0), new IntPtr(size), _vertices);
            GL.DrawElements(BeginMode.Triangles, numOfItems * 6, DrawElementsType.UnsignedShort, 0);        
        }

        private void SortItems(SpriteSortMode sortMode)
        {
            if (sortMode == SpriteSortMode.None) return;

            switch (sortMode)
            {
                case SpriteSortMode.FrontToBack:
                    _items.Sort(CompareDepth);
                    break;
                case SpriteSortMode.BackToFront:
                    _items.Sort(CompareReverseDepth);
                    break;
            }
        }

        private int CompareDepth(SpriteBatchItem a, SpriteBatchItem b)
        {
            return a.Depth.CompareTo(b.Depth);
        }
        
        private int CompareReverseDepth(SpriteBatchItem a, SpriteBatchItem b)
        {
            return b.Depth.CompareTo(a.Depth);
        }
    }
}