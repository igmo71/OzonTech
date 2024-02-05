namespace OzonTech.Contest
{
    internal class T10CommentTree
    {
        internal static void Run(string[] args)
        {
            if (!int.TryParse(Console.ReadLine(), out int inputDataSetCount))
                return;

            List<List<Comment>> inputDataSetList = new();
            for (int i = 0; i < inputDataSetCount; i++)
            {
                int commentsCount = int.Parse(Console.ReadLine()!);
                List<Comment> CommentList = new List<Comment>();
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
            //List<List<Comment>> inputDataSetList = SeedTestData();

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

        class Comment
        {
            public int Id { get; set; }
            public int ParentId { get; set; }
            public required string Text { get; set; }
        }

        private static List<List<Comment>> SeedTestData()
        {
            List<string> testStrings = GetTestStrings();
            List<List<Comment>> inputDataSetList = new();
            List<Comment> CommentList = new List<Comment>();
            int commentsCount = testStrings.Count;
            for (int j = 0; j < commentsCount; j++)
            {
                string commentStr = testStrings[j];
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
            return inputDataSetList;
        }

        private static List<string> GetTestStrings()
        {
            List<string> result = new List<string>() {
                "75 22 I'm fine. Thank you.",
                "84 82     Ciao!",
                "26 22 So-so",
                "45 26 What's wrong?",
                "22 -1 How are you?",
                "72 45 Maybe I got sick",
                "81 72 I wish you a speedy recovery!",
                "97 26   Stick it!",
                "2 97 Thanks",
                "47 72 I also got sick recently.",
                "25 -1 Hi!",
                "82 -1 Bye",
                "17 82 Good day!",
                "29 72 Visit the doctor",
            };
            return result;
        }
    }
}
