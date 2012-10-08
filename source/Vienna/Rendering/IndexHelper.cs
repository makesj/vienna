namespace Vienna.Rendering
{
    public class IndexHelper
    {
        public static int[] BuildIndices(int amount, int[] template)
        {
            var indices = new int[amount * template.Length];
            var counter = 0;

            for (var i = 0; i < amount; i++)
            {
                for (var j = 0; j < template.Length; j++)
                {
                    indices[counter] = template[j] + i * 4;
                    counter++;
                }
            }

            return indices;
        }
    }
}
