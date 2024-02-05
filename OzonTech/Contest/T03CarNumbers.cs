using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace OzonTech.Contest
{
    internal class T03CarNumbers
    {
        internal static void Run(string[] args)
        {
            if (!int.TryParse(Console.ReadLine(), out int inputDataSetCount))
                return;

            List<string> inputDataSetList = new();
            for (int i = 0; i < inputDataSetCount; i++)
            {
                // Without validation
                inputDataSetList.Add(Console.ReadLine()!);
            }

            Regex regex = new(@"[A-Z]\d{2}[A-Z]{2}|[A-Z]\d[A-Z]{2}");

            foreach (var dataSet in inputDataSetList)
            {
                var result = string.Empty;
                var matches = regex.Matches(dataSet);
                if (matches.Count > 0 && matches.Sum(m => m.Length) == dataSet.Length)
                    foreach (Match match in matches)
                        result += $"{match.Value} ";
                else
                    result = "-";
                    Console.WriteLine(result);
            }
        }
    }
}
