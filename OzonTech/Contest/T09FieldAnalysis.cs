using System.Runtime.CompilerServices;

namespace OzonTech.Contest
{
    internal class T09FieldAnalysis
    {
        internal static void Run(string[] args)
        {
            int fieldsCount = int.Parse(Console.ReadLine()!);
            Field[] fields = new Field[fieldsCount];
            for (int i = 0; i < fieldsCount; i++)
            {
                string[] size = Console.ReadLine()!.Split(' ');
                int rowCount = int.Parse(size[0]);
                int colCount = int.Parse(size[1]);
                char[,] rectangle = new char[rowCount, colCount];
                for (int j = 0; j < rowCount; j++)
                {
                    char[] row = Console.ReadLine()!.ToCharArray();
                    for (int k = 0; k < colCount; k++)
                        rectangle[j, k] = row[k];
                }
                fields[i] = new Field { rowCount = rowCount, colCount = colCount, rectangle = rectangle };
            }

            foreach (Field field in fields)
            {
                string result = field.Process();
                Console.WriteLine(result);
            }
        }
    }

    struct Field
    {
        public int rowCount;
        public int colCount;
        public char[,] rectangle;

        public string Process()
        {
            string result = string.Empty;
            List<int> results = new();
            var frames = ParseFrames();
            for (int i = 0; i < frames.Count; i++)
            {
                int internalCount = 0;
                for (int j = 0; j < frames.Count; j++)
                {
                    if (frames[i].InternalTo(frames[j]))
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


        private List<Frame> ParseFrames()
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
                            Coordinate topLeft = new(i, j);
                            Frame frame = new() { topLeft = topLeft };
                            frames.Add(frame);
                        }
                        if (i < rowCount - 1 && j != 0 && rectangle[i, j - 1] == '*' && rectangle[i + 1, j] == '*') // topRight
                        {
                            Coordinate topRight = new(i, j);
                            var lastFrame = frames.Last();
                            int index = frames.IndexOf(lastFrame);
                            lastFrame.topRight = topRight;
                            frames[index] = lastFrame;
                        }
                        if (i != 0 && j < colCount - 1 && rectangle[i - 1, j] == '*' && rectangle[i, j + 1] == '*') // bottomLeft
                        {
                            Coordinate bottomLeft = new(i, j);
                            var thisFrames = frames
                                .Where(f => f.bottomLeft == null && f.bottomRight == null && f.topLeft!.Value.col == bottomLeft.col)
                                .ToList();
                            if (thisFrames.Count > 1)
                                Console.WriteLine("To many frames!");
                            var thisFrame = thisFrames[0];
                            int index = frames.IndexOf(thisFrame);
                            thisFrame.bottomLeft = bottomLeft;
                            frames[index] = thisFrame;
                        }
                        if (i != 0 && j != 0 && rectangle[i - 1, j] == '*' && rectangle[i, j - 1] == '*') // bottomRight
                        {
                            Coordinate bottomRight = new(i, j);
                            var thisFrames = frames
                                .Where(f => f.bottomRight == null && f.topRight!.Value.col == bottomRight.col && f.bottomLeft!.Value.row == bottomRight.row)
                                .ToList();
                            if (thisFrames.Count > 1)
                                Console.WriteLine("To many frames!");
                            var thisFrame = thisFrames[0];
                            int index = frames.IndexOf(thisFrame);
                            thisFrame.bottomRight = bottomRight;
                            frames[index] = thisFrame;
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
    }

    struct Coordinate
    {
        public Coordinate(int row, int col)
        {
            this.row = row;
            this.col = col;
        }
        public int row = -1;
        public int col = -1;
    }

    static class FrameExtensions
    {
        public static bool InternalTo(this Frame me, Frame other)
        {
            var topLeftCol = me.topLeft!.Value.col > other.topLeft!.Value.col;
            var topLeftRow = me.topLeft!.Value.row > other.topLeft!.Value.row;

            var topRightCol = me.topRight!.Value.col < other.topRight!.Value.col;
            var topRightRow = me.topRight!.Value.row > other.topRight!.Value.row;

            var bottomLeftCol = me.bottomLeft!.Value.col > other.bottomLeft!.Value.col;
            var bottomLeftRow = me.bottomLeft!.Value.row < other.bottomLeft!.Value.row;

            var bottomRightCol = me.bottomRight!.Value.col < other.bottomRight!.Value.col;
            var bottomRightRow = me.bottomRight!.Value.row < other.bottomRight!.Value.row;

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
}
