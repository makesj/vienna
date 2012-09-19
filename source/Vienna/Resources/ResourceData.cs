namespace Vienna.Resources
{
    public class ResourceData
    {
        public string FileName { get; protected set; }
        public long Size { get; protected set; }
        public string FileType { get; protected set; }

        public ResourceData(long size, string fileName, string fileType)
        {
            FileName = fileName.ToLower().Replace("\\","/");
            FileType = fileType.ToLower();
            Size = size;
        }
    }
}