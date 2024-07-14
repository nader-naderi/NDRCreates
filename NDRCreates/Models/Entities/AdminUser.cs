using Microsoft.AspNetCore.Identity;

namespace NDRCreates.Models.Entities
{
    public class BasicRole : IdentityRole
    {
        public virtual ICollection<BasicUserRole> UserRoles { get; set; }
        public virtual ICollection<BasicRoleClaim> RoleClaims { get; set; }
    }

    public class BasicUserRole : IdentityUserRole<string>
    {
        public virtual BasicUser User { get; set; }
        public virtual BasicRole Role { get; set; }
    }

    public class BasicUserClaim : IdentityUserClaim<string>
    {
        public virtual BasicUser User { get; set; }
    }

    public class BasicUserLogin : IdentityUserLogin<string>
    {
        public virtual BasicUser User { get; set; }
    }

    public class BasicRoleClaim : IdentityRoleClaim<string>
    {
        public virtual BasicRole Role { get; set; }
    }

    public class BasicUserToken : IdentityUserToken<string>
    {
        public virtual BasicUser User { get; set; }
    }

    public class BasicUser : IdentityUser
    {
        public virtual ICollection<BasicUserClaim> Claims { get; set; }
        public virtual ICollection<BasicUserLogin> Logins { get; set; }
        public virtual ICollection<BasicUserToken> Tokens { get; set; }
        public virtual ICollection<BasicUserRole> Role { get; set; }

        public ICollection<BlogPost> BlogPosts { get; set; }
        public ICollection<Comment> Comments { get; set; }
        public ICollection<Like> Likes { get; set; }
    }

    public class BlogPost
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public string UserId { get; set; }
        public BasicUser Author { get; set; }
        public DateTime PublishedDate { get; set; }
        public ICollection<Comment> Comments { get; set; }

        // Change the type of CategoryId to int
        public int CategoryId { get; set; }
        public Category Category { get; set; }
        public ICollection<Tag> Tags { get; set; }
        public ICollection<Like> Likes { get; set; }
    }


    public class Comment
    {
        public int Id { get; set; }
        public int BlogPostId { get; set; }
        public BlogPost BlogPost { get; set; }
        public string Content { get; set; }
        public string UserId { get; set; }
        public BasicUser Author { get; set; }
        public DateTime PostedDate { get; set; }
    }

    public class Tag
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public ICollection<BlogPost> BlogPosts { get; set; }
    }

    public class Category
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public ICollection<BlogPost> BlogPosts { get; set; }
    }

    public class Like
    {
        public int Id { get; set; }
        public int BlogPostId { get; set; }
        public BlogPost BlogPost { get; set; }
        public string UserId { get; set; }
        public BasicUser User { get; set; }
    }
}
