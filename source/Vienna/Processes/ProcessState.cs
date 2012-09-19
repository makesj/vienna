namespace Vienna.Processes
{
    public enum ProcessState
    {
        // Processes that are neither dead nor alive
        Uninitialized = 0,  // created but not running
        Removed,  // removed from the process list but not destroyed; this can happen when a process that is already running is parented to another process

        // Living processes
        Running,  // initialized and running
        Paused,  // initialized but paused

        // Dead processes
        Succeeded,  // completed successfully
        Failed,  // failed to complete
        Aborted,  // aborted; may not have started
    };
}