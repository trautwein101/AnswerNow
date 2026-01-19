using AnswerNow.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

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
            modelBuilder.Entity<QuestionEntity>(entity =>
            {
                //entity.ToTable("Questions");
                entity.HasKey(q => q.Id);

                entity.HasIndex(q => q.UserId);

                entity.HasIndex(q => q.DateCreated);

                entity.Property(q => q.Title)
                    .IsRequired()
                    .HasMaxLength(200);

                entity.Property(q => q.Body)
                    .IsRequired()
                    .HasMaxLength(10000);

                entity.Property(q => q.CreatedBy)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(q => q.IsFlagged)
                    .IsRequired()
                    .HasDefaultValue(false);

                entity.Property(q => q.DateCreated)
                   .IsRequired()
                   .HasDefaultValueSql("now()");

                entity.Property(q => q.DateUpdated)
                    .IsRequired()
                    .HasDefaultValueSql("now()");

                // Relationship: User can have many Questions
                entity.HasOne(q => q.User)
                    .WithMany(u => u.Questions)
                    .HasForeignKey(q => q.UserId)
                    .IsRequired()
                    .OnDelete(DeleteBehavior.Restrict);
            });


            //Answer
            modelBuilder.Entity<AnswerEntity>(entity =>
            {
                //entity.ToTable("Answers");
                entity.HasKey(a => a.Id);

                entity.HasIndex(a => a.UserId);

                entity.HasIndex(a => a.DateCreated);

                entity.HasIndex(a => a.QuestionId);

                entity.Property(a => a.Body)
                    .IsRequired()
                    .HasMaxLength(10000);

                entity.Property(a => a.CreatedBy)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(a => a.UpVotes)
                    .IsRequired()
                    .HasDefaultValue(0);

                entity.Property(a => a.DownVotes)
                    .IsRequired()
                    .HasDefaultValue(0) ;

                entity.Property(a => a.IsFlagged)
                    .IsRequired()
                    .HasDefaultValue(false);

                entity.Property(a => a.DateCreated)
                   .IsRequired()
                   .HasDefaultValueSql("now()");

                entity.Property(a => a.DateUpdated)
                    .IsRequired()
                    .HasDefaultValueSql("now()");

                //one question can have many answers, each answer has one question
                entity.HasOne(a => a.Question)
                    .WithMany(q => q.Answers)
                    .HasForeignKey(a => a.QuestionId)
                    .IsRequired()
                    .OnDelete(DeleteBehavior.Cascade);

                // Relationship: User can have many Answers
                entity.HasOne(a => a.User)
                    .WithMany(u => u.Answers)
                    .HasForeignKey(a => a.UserId)
                    .OnDelete(DeleteBehavior.SetNull);
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

                entity.Property(u => u.Role)
                    .HasConversion<string>()
                    .HasMaxLength(50)
                    .IsRequired();

                entity.Property(u => u.IsProfessional)
                    .IsRequired()
                    .HasDefaultValue(true);

                entity.Property(u => u.IsActive)
                    .IsRequired()
                    .HasDefaultValue(true);

                entity.Property(u => u.IsBanned)
                    .IsRequired()
                    .HasDefaultValue(false);

                entity.Property(u => u.IsSuspended)
                    .IsRequired()
                    .HasDefaultValue(false);

                entity.Property(u => u.LastLogin);

                entity.Property(u => u.DateCreated)
                    .IsRequired()
                    .HasDefaultValueSql("now()");

                entity.Property(u => u.DateUpdated)
                    .IsRequired()
                    .HasDefaultValueSql("now()");

            });


            //RefreshToken
            modelBuilder.Entity<RefreshTokenEntity>(entity =>
            {
                //entity.ToTable("RefreshTokens");
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
