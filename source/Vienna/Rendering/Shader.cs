using System.Collections.Generic;
using System.IO;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace Vienna.Rendering
{
    public class Shader
    {
        private readonly Dictionary<string, int> _uniforms = new Dictionary<string, int>();
        
        public int Handle { get; protected set; }
        public string Name { get; protected set; }
        public string VertexShader { get; protected set; }
        public string FragmentShader { get; protected set; }

        public Shader(string name, string vertexShader, string fragmentShader)
        {
            Name = name;
            VertexShader = vertexShader;
            FragmentShader = fragmentShader;
        }

        public void Initialize()
        {
            var vertexShaderSource = File.ReadAllText(VertexShader);
            var fragmentShaderSource = File.ReadAllText(FragmentShader);

            var vertexShaderHandle = GL.CreateShader(ShaderType.VertexShader);
            var fragmentShaderHandle = GL.CreateShader(ShaderType.FragmentShader);

            GL.ShaderSource(vertexShaderHandle, vertexShaderSource);
            GL.ShaderSource(fragmentShaderHandle, fragmentShaderSource);

            GL.CompileShader(vertexShaderHandle);
            GL.CompileShader(fragmentShaderHandle);

            Handle = GL.CreateProgram();

            GL.AttachShader(Handle, vertexShaderHandle);
            GL.AttachShader(Handle, fragmentShaderHandle);

            GL.LinkProgram(Handle);
        }

        protected int AddUniform(string name)
        {
            var handle = GL.GetUniformLocation(Handle, name);
            _uniforms.Add(name, handle);
            return handle;
        }

        protected int GetUniform(string name)
        {
            return !_uniforms.ContainsKey(name) ? AddUniform(name) : _uniforms[name];
        }

        public void SetUniformMatrix4(string name, ref Matrix4 matrix)
        {
            GL.UniformMatrix4(GetUniform(name), false, ref matrix);
        }

        public Shader Bind()
        {
            GL.UseProgram(Handle);
            return this;
        }

        public override string ToString()
        {
            return string.Format("{0}:{1}", Name, Handle);
        }
    }
}
