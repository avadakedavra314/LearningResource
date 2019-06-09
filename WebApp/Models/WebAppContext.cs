using Microsoft.EntityFrameworkCore;
using WebApp.Models;

namespace WebApp.Models
{
    public class WebAppContext : DbContext
    {
        public DbSet<Role> Roles { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Group> Groups { get; set; }
        public DbSet<Course> Courses { get; set; }
        public DbSet<Lecture> Lectures { get; set; }
        public DbSet<Test> Tests { get; set; }
        public DbSet<Question> Questions { get; set; }
        public DbSet<Answer> Answers { get; set; }
        public DbSet<Result> Results { get; set; }
        public DbSet<Message> Messages { get; set; }

        public WebAppContext(DbContextOptions<WebAppContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {             
            modelBuilder.Entity("WebApp.Models.Answer", b =>
            {
                b.HasOne("WebApp.Models.Question", "Question")
                    .WithMany()                 
                    .HasForeignKey("QuestionId")
                    .OnDelete(DeleteBehavior.Cascade);
            });
            modelBuilder.Entity("WebApp.Models.Question", b =>
            {
                b.HasOne("WebApp.Models.Test", "Test")
                    .WithMany()
                    .HasForeignKey("TestId")
                    .OnDelete(DeleteBehavior.Cascade);
            });
            modelBuilder.Entity("WebApp.Models.Result", b =>
            {
                b.HasOne("WebApp.Models.Test", "Test")
                    .WithMany()
                    .HasForeignKey("TestId")
                    .OnDelete(DeleteBehavior.Cascade);

                b.HasOne("WebApp.Models.User", "User")
                    .WithMany()
                    .HasForeignKey("UserId")
                    .OnDelete(DeleteBehavior.Cascade);
            });
            modelBuilder.Entity("WebApp.Models.Lecture", b =>
            {
                b.HasOne("WebApp.Models.Course", "Course")
                    .WithMany()
                    .HasForeignKey("CourseId")
                    .OnDelete(DeleteBehavior.Cascade);
            });
            modelBuilder.Entity("WebApp.Models.Question", b =>
            {
                b.HasOne("WebApp.Models.Test", "Test")
                    .WithMany()
                    .HasForeignKey("TestId")
                    .OnDelete(DeleteBehavior.Cascade);
            });
            modelBuilder.Entity("WebApp.Models.Test", b =>
            {
                b.HasOne("WebApp.Models.Course", "Course")
                    .WithMany()
                    .HasForeignKey("CourseId")
                    .OnDelete(DeleteBehavior.Cascade);
            });


            base.OnModelCreating(modelBuilder);
        }

    }
}