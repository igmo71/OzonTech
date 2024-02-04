using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

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
            }


            foreach (var dataSet in inputDataSetList)
            {

            }
        }

        class Comment
        {
            //public Comment(int id, int parentId, string text)
            //{
            //    Id = id;
            //    ParentId = parentId;
            //    Text = text;
            //}
            public int Id { get; set; }
            public int ParentId { get; set; }
            public required string Text { get; set; }

            public void Print()
            {

            }
        }
    }
}
