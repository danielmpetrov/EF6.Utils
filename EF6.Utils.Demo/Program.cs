using EF6.Utils; // add this import to your project
using EF6.Utils.Demo.Data;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace EF6.Utils.Demo
{
    public class Program
    {
        public static async Task Main()
        {
            Console.WriteLine("Creating a new comment...");
            CreateComment();

            Console.WriteLine("Waiting 3 seconds...");
            await Task.Delay(3000);

            Console.WriteLine("Updating a comment...");
            UpdateComment();
        }

        private static void CreateComment()
        {
            var newComment = new Comment { Content = "EF6.Utils is awesome!" };

            using (var context = new AppDbContext())
            {
                context.Comments.Add(newComment);
                context.SaveChangesTimestamped();
            }
        }

        private static void UpdateComment()
        {
            using (var context = new AppDbContext())
            {
                var comment = context.Comments.LatestCreatedOrDefault(); // or: context.LatestOrDefault<Comment>();
                comment.Content = "EF6.Utils is REALLY awesome!!!";
                context.SaveChangesTimestamped();
            }
        }
    }
}
