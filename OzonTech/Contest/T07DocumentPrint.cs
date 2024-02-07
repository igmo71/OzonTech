namespace OzonTech.Contest
{
    internal class T07DocumentPrint
    {
        internal static void Run(string[] args)
        {
            int documentsCount = int.Parse(Console.ReadLine()!);
            Document[] documents = new Document[documentsCount];
            for (int i = 0; i < documentsCount; i++)
            {
                Document document = new()
                {
                    pageCount = int.Parse(Console.ReadLine()!),
                    printedPagesString = Console.ReadLine()!
                };
                documents[i] = document;
            }

            foreach (Document document in documents)
            {
                string remainingPages = document.Process();
                Console.WriteLine(remainingPages);
            }
        }

        struct Document
        {
            internal int pageCount;
            internal string printedPagesString;

            internal string Process()
            {
                int[] printedPages = ParsePrintedPagesString();

                string remainingPagesString = BuildRemainingPages(printedPages);

                return remainingPagesString;
            }

            private int[] ParsePrintedPagesString()
            {
                int[] printedPages = new int[pageCount];
                Array.Fill<int>(printedPages, 0);

                string[] printed = printedPagesString.Split(',');
                foreach (string print in printed)
                {
                    if (int.TryParse(print, out int printedPageNumber))
                        printedPages[printedPageNumber - 1] = 1;
                    else
                    {
                        string[] printedPageNumbers = print.Split('-');
                        for (int i = int.Parse(printedPageNumbers[0]) - 1; i < int.Parse(printedPageNumbers[1]); i++)
                            printedPages[i] = 1;
                    }
                }
                return printedPages;
            }

            private static string BuildRemainingPages(int[] pages)
            {
                string result = string.Empty;
                int beginRange = -1;
                int endRange = -1;
                for (int i = 0; i < pages.Length; i++)
                {
                    if (pages[i] == 0)
                    {
                        if (beginRange == -1 && endRange == -1)
                        {
                            beginRange = i;
                            endRange = i;
                        }
                        else
                        {
                            endRange = i;
                        }
                    }

                    if ((pages[i] == 1 || i == pages.Length - 1) && beginRange != -1 && endRange != -1)
                    {
                        if (beginRange == endRange)
                        {
                            result = $"{result}{beginRange + 1},";
                        }
                        else
                        {
                            result = $"{result}{beginRange + 1}-{endRange + 1},";
                        }
                        beginRange = -1;
                        endRange = -1;
                    }

                    if (i == pages.Length - 1)
                    {
                        result = result.TrimEnd(',');
                    }
                }
                return result;
            }
        }
    }
}
