namespace OzonTech.Contest
{
    internal class T1SeaBattle
    {
        internal static void Run(string[] args)
        {
            if (!int.TryParse(Console.ReadLine(), out int inputDataSetCount))
                return;

            List<int[]> inputDataSetList = new();
            for (int i = 0; i < inputDataSetCount; i++)
            {
                // Without validation
                inputDataSetList.Add(Console.ReadLine()!.Split(' ').Select(c => int.Parse(c)).ToArray());
            }

            Dictionary<int, int> OriginalDataSet = new() { [1] = 4, [2] = 3, [3] = 2, [4] = 1 };

            foreach (var dataSet in inputDataSetList)
            {
                bool result = true;
                foreach (var originalData in OriginalDataSet)
                {
                    if (originalData.Value != dataSet.Where(d => d == originalData.Key).Count())
                    {
                        result = false;
                        continue;
                    }
                }
                Console.WriteLine(result ? "yes" : "no");
            }
        }
        
    }
}
