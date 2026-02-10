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
        public DbSet<QuestionFlagEntity> QuestionFlags { get; set; } = null!;
        public DbSet<AnswerFlagEntity> AnswerFlags { get; set; } = null!;
        public DbSet<UserEntity> Users { get; set; } = null!;
        public DbSet<RefreshTokenEntity> RefreshTokens { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // -----------------------------
            // Question
            // -----------------------------
            modelBuilder.Entity<QuestionEntity>(entity =>
            {
                entity.ToTable("questions");
                entity.HasKey(q => q.Id);

                entity.HasIndex(q => q.UserId);
                entity.HasIndex(q => q.DateCreated);

                entity.Property(q => q.Title)
                    .IsRequired()
                    .HasMaxLength(200);

                entity.Property(q => q.Body)
                    .IsRequired()
                    .HasMaxLength(10000);

                entity.Property(q => q.IsFlagged)
                    .IsRequired()
                    .HasDefaultValue(false);

                entity.Property(q => q.IsDeleted)
                    .IsRequired()
                    .HasDefaultValue(false);

                entity.Property(q => q.DateCreated)
                    .IsRequired()
                    .HasDefaultValueSql("now()");

                entity.Property(q => q.DateUpdated)
                    .IsRequired()
                    .HasDefaultValueSql("now()");

                entity.Property(q => q.DateDeleted)
                    .IsRequired(false);

                // User -> Questions
                entity.HasOne(q => q.User)
                    .WithMany(u => u.Questions)
                    .HasForeignKey(q => q.UserId)
                    .IsRequired()
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(q => q.DeletedByUser)
                    .WithMany()
                    .HasForeignKey(q => q.DeletedByUserId)
                    .OnDelete(DeleteBehavior.Restrict);
            });



            // -----------------------------
            // QuestionFlags
            // -----------------------------
            modelBuilder.Entity<QuestionFlagEntity>(entity =>
            {
                entity.ToTable("question_flags");
                entity.HasKey(f => f.Id);

                entity.HasIndex(f => f.QuestionId);
                entity.HasIndex(f => f.ReportedByUserId);
                entity.HasIndex(f => f.DateCreated);

                entity.HasIndex(f => f.IsResolved);
                entity.HasIndex(f => f.IsDeleted);

                entity.Property(f => f.Reason)
                    .IsRequired();

                entity.Property(f => f.IsResolved)
                    .IsRequired()
                    .HasDefaultValue(false);

                entity.Property(f => f.IsDeleted)
                    .IsRequired()
                    .HasDefaultValue(false);

                entity.Property(f => f.DateCreated)
                    .IsRequired()
                    .HasDefaultValueSql("now()");

                entity.Property(f => f.DateUpdated)
                    .IsRequired()
                    .HasDefaultValueSql("now()");

                entity.Property(f => f.DateDeleted)
                    .IsRequired(false);

                entity.Property(f => f.DateResolved)
                    .IsRequired(false);

                entity.Property(f => f.ResolvedByUserId)
                    .IsRequired(false);

                entity.Property(f => f.DeletedByUserId)
                    .IsRequired(false);

                // Flag -> Question
                entity.HasOne(f => f.Question)
                    .WithMany(q => q.QuestionFlag)
                    .HasForeignKey(f => f.QuestionId)
                    .IsRequired()
                    .OnDelete(DeleteBehavior.Restrict);

                // Flag -> QuestionOwnerUser
                entity.HasOne(f => f.QuestionOwnerUser)
                    .WithMany(u => u.OwnedQuestionFlag)
                    .HasForeignKey(f => f.QuestionOwnerUserId)
                    .OnDelete(DeleteBehavior.Restrict);

                // Flag -> ReportedByUser
                entity.HasOne(f => f.ReportedByUser)
                    .WithMany(u => u.ReportedQuestionFlag)
                    .HasForeignKey(f => f.ReportedByUserId)
                    .OnDelete(DeleteBehavior.Restrict);

                // Flag -> ResolvedByUser
                entity.HasOne(f => f.ResolvedByUser)
                    .WithMany(u => u.ResolvedQuestionFlag)
                    .HasForeignKey(f => f.ResolvedByUserId)
                    .OnDelete(DeleteBehavior.Restrict);

                // Flag -> DeletedByUser
                entity.HasOne(f => f.DeletedByUser)
                    .WithMany(u => u.DeletedQuestionFlag)
                    .HasForeignKey(f => f.DeletedByUserId)
                    .OnDelete(DeleteBehavior.Restrict);
            });


            // -----------------------------
            // Answer
            // -----------------------------
            modelBuilder.Entity<AnswerEntity>(entity =>
            {
                entity.ToTable("answers");
                entity.HasKey(a => a.Id);

                entity.HasIndex(a => a.QuestionId);
                entity.HasIndex(a => a.UserId);
                entity.HasIndex(a => a.DateCreated);

                entity.Property(a => a.Body)
                    .IsRequired();

                entity.Property(a => a.IsFlagged)
                    .IsRequired()
                    .HasDefaultValue(false);

                entity.Property(a => a.IsDeleted)
                    .IsRequired()
                    .HasDefaultValue(false);

                entity.Property(a => a.DateCreated)
                    .IsRequired()
                    .HasDefaultValueSql("now()");

                entity.Property(a => a.DateUpdated)
                    .IsRequired()
                    .HasDefaultValueSql("now()");

                entity.Property(a => a.DateDeleted)
                    .IsRequired(false);

                // Answer -> Question
                entity.HasOne(a => a.Question)
                    .WithMany(q => q.Answers)
                    .HasForeignKey(a => a.QuestionId)
                    .IsRequired()
                    .OnDelete(DeleteBehavior.Restrict);

                // Answer -> User
                entity.HasOne(a => a.User)
                    .WithMany(u => u.Answers)
                    .HasForeignKey(a => a.UserId)
                    .IsRequired()
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasMany(a => a.AnswerFlag)
                    .WithOne(f => f.Answer)
                    .HasForeignKey(f => f.AnswerId)
                    .IsRequired()
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(a => a.DeletedByUser)
                    .WithMany()
                    .HasForeignKey(a => a.DeletedByUserId)
                    .OnDelete(DeleteBehavior.Restrict);
            });


            // -----------------------------
            // AnswerFlag
            // -----------------------------
            modelBuilder.Entity<AnswerFlagEntity>(entity =>
            {
                entity.ToTable("answer_flags");
                entity.HasKey(f => f.Id);

                entity.HasIndex(f => f.AnswerId);
                entity.HasIndex(f => f.ReportedByUserId);
                entity.HasIndex(f => f.DateCreated);
                entity.HasIndex(f => f.IsResolved);
                entity.HasIndex(f => f.IsDeleted);

                entity.Property(f => f.Reason)
                    .IsRequired();

                entity.Property(f => f.IsResolved)
                    .IsRequired()
                    .HasDefaultValue(false);

                entity.Property(f => f.IsDeleted)
                    .IsRequired()
                    .HasDefaultValue(false);

                entity.Property(f => f.DateCreated)
                    .IsRequired()
                    .HasDefaultValueSql("now()");

                entity.Property(f => f.DateUpdated)
                    .IsRequired()
                    .HasDefaultValueSql("now()");

                entity.Property(f => f.DateDeleted)
                    .IsRequired(false);

                entity.Property(f => f.DateResolved)
                    .IsRequired(false);

                entity.Property(f => f.ResolvedByUserId)
                    .IsRequired(false);

                entity.Property(f => f.DeletedByUserId)
                    .IsRequired(false);

                // Flag -> Answer
                entity.HasOne(f => f.Answer)
                    .WithMany(a => a.AnswerFlag)
                    .HasForeignKey(f => f.AnswerId)
                    .IsRequired()
                    .OnDelete(DeleteBehavior.Restrict);

                // Flag -> AnswerOwnerUser
                entity.HasOne(f => f.AnswerOwnerUser)
                    .WithMany(u => u.OwnedAnswerFlag)
                    .HasForeignKey(f => f.AnswerOwnerUserId)
                    .OnDelete(DeleteBehavior.Restrict);

                // Flag -> ReportedByUser
                entity.HasOne(f => f.ReportedByUser)
                    .WithMany(u => u.ReportedAnswerFlag)
                    .HasForeignKey(f => f.ReportedByUserId)
                    .OnDelete(DeleteBehavior.Restrict);

                // Flag -> ResolvedByUser (optional)
                entity.HasOne(f => f.ResolvedByUser)
                    .WithMany(u => u.ResolvedAnswerFlag)
                    .HasForeignKey(f => f.ResolvedByUserId)
                    .OnDelete(DeleteBehavior.Restrict);

                // Flag -> DeletedByUser (optional)
                entity.HasOne(f => f.DeletedByUser)
                    .WithMany(u => u.DeletedAnswerFlag)
                    .HasForeignKey(f => f.DeletedByUserId)
                    .OnDelete(DeleteBehavior.Restrict);
            });



            //User
            modelBuilder.Entity<UserEntity>(entity =>
            {
                entity.ToTable("users");
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
                    .HasDefaultValue(false);

                entity.Property(u => u.IsActive)
                    .IsRequired()
                    .HasDefaultValue(true);

                entity.Property(u => u.IsInActive)
                    .IsRequired()
                    .HasDefaultValue(false);

                entity.Property(u => u.IsPending)
                     .IsRequired()
                     .HasDefaultValue(false);

                entity.Property(u => u.IsSuspended)
                    .IsRequired()
                    .HasDefaultValue(false);

                entity.Property(u => u.IsBanned)
                    .IsRequired()
                    .HasDefaultValue(false);

                entity.Property(u => u.LastLogin)
                    .HasDefaultValueSql("now()");

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
                entity.ToTable("refresh_tokens");
                entity.HasKey(r => r.Id);

                entity.HasIndex(r => r.Token);

                entity.Property(r => r.Token)
                    .IsRequired()
                    .HasMaxLength(256);

                //Relationship from token to user
                entity.HasOne(r => r.User)
                .WithMany(u => u.RefreshTokens) //user can have multiple refresh token ~ multiple devices
                .HasForeignKey(r => r.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            });
            
        }
    }
}
