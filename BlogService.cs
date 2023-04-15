using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

public class BlogService
{
    public static Dictionary<string, int> NumberOfCommentsPerUser(MyDbContext context)
    {
        var NumberOfCommentsPerUserList = context.BlogComments
            .GroupBy(c => c.UserName).Select(x => new {UserName = x.Key, Count = x.Count() }).ToList();
        

        Dictionary<string, int> NumberOfCommentsPerUser = NumberOfCommentsPerUserList.ToDictionary(o => o.UserName, o => o.Count);

        return NumberOfCommentsPerUser;
    }

    public static Dictionary<string, string> PostsOrderedByLastCommentDate(MyDbContext context)
    { 
        var OrderedPosts = context.BlogPosts.Select(p => new
        {
            Post = p,
            LastComment = p.Comments.OrderByDescending(x => x.CreatedDate).FirstOrDefault()
        }).OrderByDescending(x => x.LastComment.CreatedDate).ToList();

        Dictionary<string,string> OrderedPostsDictionary = OrderedPosts
            .ToDictionary(o => o.Post.Title, o => o.LastComment.CreatedDate.ToString("yyyy-MM-dd"));

        return OrderedPostsDictionary;
    }

    public static Dictionary<string, int> NumberOfLastCommentsLeftByUser(MyDbContext context)
    {
        var PostsWithLastComment = context.BlogPosts.Select(p => new
        {
            Post = p,
            LastComment = p.Comments.OrderByDescending(x => x.CreatedDate).FirstOrDefault(),
        });

        var NumberOfLastCommentsLeftByUser = PostsWithLastComment
            .GroupBy(c => c.LastComment.UserName)
            .Select(x => new {UserName = x.Key, Count = x.Count()})
            .ToDictionary(o => o.UserName, o => o.Count);

        return NumberOfLastCommentsLeftByUser;
    }
}
