using System;

namespace Vienna.Processes
{
    public abstract class Process
    {
        // the child process, if any
        public Process Child { get; private set; }

        // the current state of the process
        public ProcessState State { get; set; }

        public bool IsAlive 
        { 
            get { return (State == ProcessState.Running || State == ProcessState.Paused); }
        }

        public bool IsDead
        { 
            get {  return (State == ProcessState.Succeeded 
                || State == ProcessState.Failed 
                || State == ProcessState.Aborted); }
        }

        public bool IsRemoved 
        { 
            get {  return State == ProcessState.Removed; }
        }

        public bool IsPaused 
        { 
            get { return State == ProcessState.Paused; }
        }

        public bool HasChild
        {
            get { return Child != null; }
        }

        protected Process()
        {
            State = ProcessState.Uninitialized;
        }

	    // Functions for ending the process.
	    public void Succeed()
	    {
            if (State == ProcessState.Running || State == ProcessState.Paused)
            {
                State = ProcessState.Succeeded;
                return;
            }
            throw new InvalidOperationException("Invalid process transition. Process must be either running or paused to succeed.");
	    }

        public void Fail()
        {
            if (State == ProcessState.Running || State == ProcessState.Paused)
            {
                State = ProcessState.Succeeded;
                return;
            }
            throw new InvalidOperationException("Invalid process transition. Process must be either running or paused to fail.");
        }
        	
	    // pause
	    public void Pause()
	    {
            if (State == ProcessState.Running)
            {
                State = ProcessState.Paused;
            }
            else
            {
                Logger.Warn("Attempting to pause a process that isn't running");
            }
	    }

        public void UnPause()
        {
            if (State == ProcessState.Paused)
            {
                State = ProcessState.Running;
            }
            else
            {
                Logger.Warn("Attempting to unpause a process that isn't paused");
            }
        }

   	 
	    public void AttachChild(Process child)
	    {
            if (Child == null)
            {
                Child = child;
            }
            else
            {
                //allow chaining from the top
                Child.AttachChild(child);
            }
	    }

	    public Process RemoveChild()
	    {
	        var p = Child;
	        Child = null;
	        return p;
	    }

        // --- Template Methods ---
        // --- these should be overridden by the subclass as needed ---

        // called during the first update; responsible for setting the initial state (typically RUNNING)    
        public virtual void OnInit() { State = ProcessState.Running; } 

        public virtual void OnUpdate(long delta) { }

        // called if the process succeeds 
        public virtual void OnSuccess() { }

        // called if the process fails
        public virtual void OnFail() { }

        // called if the process is aborted
        public virtual void OnAbort() { }

        public virtual void Destroy() { } 
    }
}
