namespace Vienna.Resources
{
    public class DefaultResourceLoader : ResourceLoader
    {
        public DefaultResourceLoader(string path, int cacheMb)
            : base(path, cacheMb)
        {
        }

        public override ResourceData FilterCatalog(long size, string fileName)
        {
            var lastDot = fileName.LastIndexOf(".") + 1;
            var fileType = fileName.Substring(lastDot, fileName.Length - lastDot);

            var data = new ResourceData(size, fileName, fileType);
            return data;
        }
    }
}