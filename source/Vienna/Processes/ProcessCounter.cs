namespace Vienna.Processes
{
    public struct ProcessCounter
    {
        public int Fail;
        public int Success;

        public ProcessCounter(int success, int fail)
        {
            Success = success;
            Fail = fail;
        }
    }
}