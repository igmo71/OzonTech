using System.Diagnostics;

namespace OzonTech
{
    internal class Program
    {
        static void Main(string[] args)
        {
            using StreamReader input = new StreamReader(Console.OpenStandardInput(), bufferSize: 16384);
            using StreamWriter output = new StreamWriter(Console.OpenStandardOutput(), bufferSize: 16384);

            var iputStartTimestamp = Stopwatch.GetTimestamp();

            string? fieldsCountString = input.ReadLine();
            int fieldsCount = int.Parse(fieldsCountString!);
            Field[] fields = new Field[fieldsCount];

            for (int i = 0; i < fieldsCount; i++)
            {
                string? sizeString = input.ReadLine();
                string[] size = sizeString!.Split(' ');
                fields[i].rowCount = int.Parse(size[0]);
                fields[i].colCount = int.Parse(size[1]);
                fields[i].rows = new string[fields[i].rowCount];
                for (int j = 0; j < fields[i].rowCount; j++)
                {
                    fields[i].rows[j] = input.ReadLine()!;
                    Debug.WriteLine($"fields[{i}].rows[{j}]");
                }
            }
            Debug.WriteLine($"input: {Stopwatch.GetElapsedTime(iputStartTimestamp).TotalMilliseconds}(ms)");

            var processStartTimestamp = Stopwatch.GetTimestamp();
            foreach (Field field in fields)
            {
                string result = field.Process();
                output.WriteLine(result);
            }
            Debug.WriteLine($"process: {Stopwatch.GetElapsedTime(processStartTimestamp).TotalMilliseconds}(ms)");
        }
    }

    struct Field
    {
        public int rowCount;
        public int colCount;
        public string[] rows;

        public string Process()
        {
            List<int> internalCountList = new();

            var frames = ParseFrames();

            for (int i = 0; i < frames.Count; i++)
            {
                int internalCount = 0;
                for (int j = 0; j < frames.Count; j++)
                {
                    Debug.WriteLine($"Frame.InternalTo(frames[{i}], frames[{j}])");
                    if (i != j && Frame.InternalTo(frames[i], frames[j]))
                    {
                        internalCount++;
                    }
                }
                internalCountList.Add(internalCount);
            }
            internalCountList.Sort();

            string result = string.Join(' ', internalCountList);
            return result;
        }

        private List<Frame> ParseFrames()
        {
            List<Frame> frames = new();
            for (int i = 0; i < rowCount; i++)
            {
                for (int j = 0; j < colCount; j++)
                {
                    if (rows[i][j] == '*')
                    {
                        if (i < rowCount - 1 && j < colCount - 1 && rows[i][j + 1] == '*' && rows[i + 1][j] == '*') // topLeft
                        {
                            frames.Add(new Frame() { topLeft = new Coordinate(i, j) });
                            Debug.WriteLine($"frames.Count: {frames.Count}");
                        }
                        if (i < rowCount - 1 && j != 0 && rows[i][j - 1] == '*' && rows[i + 1][j] == '*') // topRight
                        {
                            var farame = frames.Last();
                            int index = frames.IndexOf(farame);
                            farame.topRight = new Coordinate(i, j);
                            frames[index] = farame;
                        }
                        if (i != 0 && j < colCount - 1 && rows[i - 1][j] == '*' && rows[i][j + 1] == '*') // bottomLeft
                        {
                            Coordinate bottomLeft = new(i, j);
                            var farame = frames.Single(f => f.bottomLeft == null && f.bottomRight == null && f.topLeft!.Value.col == bottomLeft.col);
                            int index = frames.IndexOf(farame);
                            farame.bottomLeft = bottomLeft;
                            frames[index] = farame;
                        }
                        if (i != 0 && j != 0 && rows[i - 1][j] == '*' && rows[i][j - 1] == '*') // bottomRight
                        {
                            Coordinate bottomRight = new(i, j);
                            var farame = frames.Single(f => f.bottomRight == null && f.topRight!.Value.col == bottomRight.col && f.bottomLeft!.Value.row == bottomRight.row);
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