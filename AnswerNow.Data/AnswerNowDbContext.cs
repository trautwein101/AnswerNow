using AnswerNow.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace AnswerNow.Data
{
    public class AnswerNowDbContext : DbContext
    {
        public AnswerNowDbContext(DbContextOptions<AnswerNowDbContext> options) 
            : base(options)
        {
        }

        public DbSet<QuestionEntity> Questions { get; set; } = null!;
        public DbSet<AnswerEntity> Answers { get; set; } = null!;
        public DbSet<UserEntity> Users { get; set; } = null!;
        public DbSet<RefreshTokenEntity> RefreshTokens { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            //Question
            // Question configuration
            modelBuilder.Entity<QuestionEntity>(entity =>
            {
                //entity.ToTable("Questions");  // Explicit table name
                entity.HasKey(q => q.Id);

                entity.Property(q => q.Title)
                    .IsRequired()
                    .HasMaxLength(200);

                entity.Property(q => q.Body)
                    .IsRequired();
            });

            //Answer
            modelBuilder.Entity<AnswerEntity>(entity =>
            {
                //entity.ToTable("Answers");
                entity.HasKey(a => a.Id);

                entity.Property(a => a.Body)
                    .IsRequired();

                entity.Property(a => a.DateCreated);

                //one question can have many answers, each answer has one question
                entity.HasOne(a => a.Question)
                    .WithMany() // later we can add ICollection<Answer> Answers on Question if we want
                    .HasForeignKey(a => a.QuestionId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            //User
            modelBuilder.Entity<UserEntity>(entity =>
            {
                //entity.ToTable("Users");
                entity.HasKey(u => u.Id);

                entity.HasIndex(u => u.Email)
                    .IsUnique();

                entity.Property(u => u.Email)
                    .IsRequired()
                    .HasMaxLength(256);

                entity.Property(u => u.DisplayName)
                    .IsRequired()
                    .HasMaxLength(100);
                        
                entity.Property(u => u.PasswordHash)
                    .IsRequired();
            });


            //RefreshToken
            modelBuilder.Entity<RefreshTokenEntity>(entity =>
            {
                //entity.ToTable("RefreshTokens"); //todo: maybe a better name
                entity.HasKey(r => r.Id);

                entity.HasIndex(r => r.Token);

                entity.Property(r => r.Token)
                    .IsRequired()
                    .HasMaxLength(256);

                //Relationship from token to user
                entity.HasOne(r => r.User)
                .WithMany() //user can have multiple refresh token ~ multiple devices
                .HasForeignKey(r => r.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            });
            
        }
    }
}
