namespace OzonTech
{
    internal class Program
    {
        static void Main(string[] args)
        {
            using var input = new StreamReader(Console.OpenStandardInput());
            using var output = new StreamWriter(Console.OpenStandardOutput());

            int fieldsCount = int.Parse(input.ReadLine()!);
            Field[] fields = new Field[fieldsCount];

            for (int i = 0; i < fieldsCount; i++)
            {
                string[] size = input.ReadLine()!.Split(' ');
                fields[i].rowCount = int.Parse(size[0]);
                fields[i].colCount = int.Parse(size[1]);
                fields[i].rows = new string[int.Parse(size[0])];
                for (int j = 0; j < fields[i].rowCount; j++)
                {
                    fields[i].rows[j] = input.ReadLine()!;
                }
            }

            foreach (Field field in fields)
            {
                string result = field.Process();
                output.WriteLine(result);
            }
        }
    }

    struct InputFieldData
    {
        public string size;
        public string[] rows;
    }

    struct Field
    {
        public int rowCount;
        public int colCount;
        public string[] rows;

        public string Process()
        {
            string result = string.Empty;
            List<int> results = new();

            char[,] rectangle = ParseRectangle();

            var frames = ParseFrames(rectangle);
            for (int i = 0; i < frames.Count; i++)
            {
                int internalCount = 0;
                for (int j = 0; j < frames.Count; j++)
                {
                    if (Frame.InternalTo(frames[i], frames[j]))
                    {
                        internalCount++;
                    }
                }
                results.Add(internalCount);
            }
            results.Sort();
            foreach (var r in results)
                result = $"{result}{r} ";
            return result;
        }

        private char[,] ParseRectangle()
        {
            char[,] rectangle = new char[rowCount, colCount];
            for (int i = 0; i < rows.Length; i++)
            {
                for (int j = 0; j < rows[i].Length; j++)
                {
                    rectangle[i, j] = rows[i][j];
                }
            }
            return rectangle;
        }

        private List<Frame> ParseFrames(char[,] rectangle)
        {
            List<Frame> frames = new();
            for (int i = 0; i < rowCount; i++)
            {
                for (int j = 0; j < colCount; j++)
                {
                    if (rectangle[i, j] == '*')
                    {
                        if (i < rowCount - 1 && j < colCount - 1 && rectangle[i, j + 1] == '*' && rectangle[i + 1, j] == '*') // topLeft
                        {
                            frames.Add(new Frame() { topLeft = new Coordinate(i, j) });
                        }
                        if (i < rowCount - 1 && j != 0 && rectangle[i, j - 1] == '*' && rectangle[i + 1, j] == '*') // topRight
                        {
                            var farame = frames.Last();
                            int index = frames.IndexOf(farame);
                            farame.topRight = new Coordinate(i, j);
                            frames[index] = farame;
                        }
                        if (i != 0 && j < colCount - 1 && rectangle[i - 1, j] == '*' && rectangle[i, j + 1] == '*') // bottomLeft
                        {
                            Coordinate bottomLeft = new(i, j);
                            var farame = frames.First(f => f.bottomLeft == null && f.bottomRight == null && f.topLeft!.Value.col == bottomLeft.col);
                            int index = frames.IndexOf(farame);
                            farame.bottomLeft = bottomLeft;
                            frames[index] = farame;
                        }
                        if (i != 0 && j != 0 && rectangle[i - 1, j] == '*' && rectangle[i, j - 1] == '*') // bottomRight
                        {
                            Coordinate bottomRight = new(i, j);
                            var farame = frames.First(f => f.bottomRight == null && f.topRight!.Value.col == bottomRight.col && f.bottomLeft!.Value.row == bottomRight.row);
                            int index = frames.IndexOf(farame);
                            farame.bottomRight = bottomRight;
                            frames[index] = farame;
                        }

                    }
                }
            }
            return frames;
        }
    }

    struct Frame
    {
        public Coordinate? topLeft;
        public Coordinate? topRight;
        public Coordinate? bottomLeft;
        public Coordinate? bottomRight;

        public static bool InternalTo(Frame me, Frame other)
        {
            if (me.topLeft!.Value.col > other.topLeft!.Value.col &&
                me.topLeft!.Value.row > other.topLeft!.Value.row &&

                me.topRight!.Value.col < other.topRight!.Value.col &&
                me.topRight!.Value.row > other.topRight!.Value.row &&

                me.bottomLeft!.Value.col > other.bottomLeft!.Value.col &&
                me.bottomLeft!.Value.row < other.bottomLeft!.Value.row &&

                me.bottomRight!.Value.col < other.bottomRight!.Value.col &&
                me.bottomRight!.Value.row < other.bottomRight!.Value.row
                )
                return true;
            return false;
        }
    }

    struct Coordinate
    {
        public Coordinate(int row, int col)
        {
            this.row = row;
            this.col = col;
        }
        public int row;
        public int col;
    }
}