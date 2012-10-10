using System;
using Vienna.Physics;

namespace Vienna.Core
{
    public class Runtime : IDisposable
    {
        private const int FramesPerSecond = 60;
        private const int UpdatesPerSecond = 60;

        public bool IsDisposed { get; protected set; }
        public bool IsInitialized { get; protected set; }
        public bool IsRunning { get; protected set; }

        private readonly Bootstrap _bootstrap = new Bootstrap();
        private readonly Unloader _unloader = new Unloader();
        private ViennaWindow _window;

        public void Initialize()
        {
            if(IsInitialized) return;

            try
            {
                _bootstrap.Initialize(out _window);
                IsInitialized = true;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Initialization Error: {0}", ex.Message);
                Console.WriteLine(ex.StackTrace);
                Console.ReadKey();
            }           
        }

        public void Run()
        {
            if(IsRunning) return;

            Console.WriteLine("Running...");

            try
            {
                if(!IsInitialized)
                    throw new Exception("Runtime has not been initialized.");

                if (IsDisposed)
                    throw new Exception("Runtime has been disposed.");

                IsRunning = true;
                _window.Run(FramesPerSecond, UpdatesPerSecond);                
            }
            catch(Exception ex)
            {
                Console.WriteLine("Runtime Error: {0}", ex.Message);
                Console.WriteLine(ex.StackTrace);
                Console.ReadKey();
            }
            finally  
            {
                IsRunning = false;
            }           
        }

        public void Shutdown()
        {
            if(IsDisposed) return;

            try
            {
                _unloader.Destroy(ref _window);
                GlobalPhysics.Instance.Dispose();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Shutdown Error: {0}", ex.Message);
                Console.WriteLine(ex.StackTrace);
                Console.ReadKey();
            }
            finally
            {
                IsDisposed = true;
                IsInitialized = false;
                IsRunning = false;
            }      
        }

        public void Dispose()
        {
            Shutdown();
        }       
    }
}