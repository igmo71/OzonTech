namespace OzonTech
{
    internal class Program
    {
        static void Main(string[] args)
        {
            if (!int.TryParse(Console.ReadLine(), out int inputDataSetCount))
                return;

            List<List<Comment>> inputDataSetList = new();
            for (int i = 0; i < inputDataSetCount; i++)
            {
                int commentsCount = int.Parse(Console.ReadLine()!);
                List<Comment> CommentList = new();
                for (int j = 0; j < commentsCount; j++)
                {
                    string commentStr = Console.ReadLine()!;
                    int indexOfId = commentStr.IndexOf(" ");
                    int indexOfParentId = commentStr.IndexOf(" ", indexOfId + 1);
                    Comment comment = new()
                    {
                        Id = int.Parse(commentStr.Substring(0, indexOfId)),
                        ParentId = int.Parse(commentStr.Substring(indexOfId + 1, indexOfParentId - indexOfId)),
                        Text = commentStr.Substring(indexOfParentId + 1)
                    };
                    CommentList.Add(comment);
                }
                inputDataSetList.Add(CommentList);
            }

            foreach (var commentList in inputDataSetList)
            {
                var parentCommentList = commentList.Where(c => c.ParentId == -1).OrderBy(c => c.Id).ToArray();
                foreach (var parentComment in parentCommentList)
                {
                    Console.WriteLine(parentComment.Text);
                    PrintChildrenComments(parentComment, string.Empty, commentList);
                    Console.WriteLine();
                }
            }
        }

        private static void PrintChildrenComments(Comment parentComment, string parentGraf, List<Comment> commentList)
        {
            var childrenCommentList = commentList.Where(c => c.ParentId == parentComment.Id).OrderBy(c => c.Id).ToArray();
            if (childrenCommentList.Any())
            {
                for (int i = 0; i < childrenCommentList.Length; i++)
                {
                    Console.WriteLine($"{parentGraf}|");
                    Console.WriteLine($"{parentGraf}|--{childrenCommentList[i].Text}");

                    var childGraf = i < childrenCommentList.Length - 1 ? $"{parentGraf}|  " : $"{parentGraf}   ";

                    PrintChildrenComments(childrenCommentList[i], childGraf, commentList);
                }
            }
        }
    }

    internal class Comment
    {
        public int Id { get; set; }
        public int ParentId { get; set; }
        public required string Text { get; set; }
    }
}
