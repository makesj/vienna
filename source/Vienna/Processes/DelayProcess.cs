namespace Vienna.Processes
{
    public class DelayProcess : Process
    {
        public long Delay { get; protected set; }
        private long _timePassed;

        public DelayProcess(int delay)
        {
            Delay = delay;
        }

        public override void OnAbort()
        {
            Logger.Debug("delay OnAbort");
        }

        public override void OnFail()
        {
            Logger.Debug("delay OnFail");
        }

        public override void OnSuccess()
        {
            Logger.Debug("delay OnSuccess");
        }

        public override void OnUpdate(long delta)
        {
            _timePassed += delta;
            if (_timePassed >= Delay)
            {
                Succeed();
            }
        }
    }
}
