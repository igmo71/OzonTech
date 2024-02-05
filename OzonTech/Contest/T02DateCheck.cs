namespace OzonTech.Contest
{
    internal class T02DateCheck
    {
        static void Run(string[] args)
        {
            if (!int.TryParse(Console.ReadLine(), out int inputDataSetCount))
                return;

            List<int[]> inputDataSetList = new();
            for (int i = 0; i < inputDataSetCount; i++)
            {
                // Without validation
                inputDataSetList.Add(Console.ReadLine()!.Split(' ').Select(c => int.Parse(c)).ToArray());
            }

            foreach (var dataSet in inputDataSetList)
            {
                if (DateTime.TryParse($"{dataSet[2]}-{dataSet[1]}-{dataSet[0]}", out DateTime dateResult))
                    Console.WriteLine("yes");
                else
                    Console.WriteLine("no");
            }
        }
    }
}
