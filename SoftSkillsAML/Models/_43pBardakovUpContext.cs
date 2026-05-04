using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace SoftSkillsAML.Models;

public partial class _43pBardakovUpContext : DbContext
{
    public _43pBardakovUpContext()
    {
    }

    public _43pBardakovUpContext(DbContextOptions<_43pBardakovUpContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Achievement> Achievements { get; set; }

    public virtual DbSet<Answer> Answers { get; set; }

    public virtual DbSet<AnswerSoftSkill> AnswerSoftSkills { get; set; }

    public virtual DbSet<Department> Departments { get; set; }

    public virtual DbSet<Gender> Genders { get; set; }

    public virtual DbSet<Question> Questions { get; set; }

    public virtual DbSet<SoftSkill> SoftSkills { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<UserAchievement> UserAchievements { get; set; }

    public virtual DbSet<UserQuestion> UserQuestions { get; set; }

    public virtual DbSet<UserSoftSkill> UserSoftSkills { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseNpgsql("Host=edu.pg.ngknn.ru;Port=5442;Database=43P_Bardakov_UP;Username=43P;Password=444444");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Achievement>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("achievements_pk");

            entity.ToTable("achievements", "Diplom");

            entity.HasIndex(e => e.Name, "achievements_unique").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Description)
                .HasColumnType("character varying")
                .HasColumnName("description");
            entity.Property(e => e.Image).HasColumnName("image");
            entity.Property(e => e.Name)
                .HasColumnType("character varying")
                .HasColumnName("name");
        });

        modelBuilder.Entity<Answer>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("answers_pk");

            entity.ToTable("answers", "Diplom");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Image).HasColumnName("image");
            entity.Property(e => e.IsCorrect).HasColumnName("is_correct");
            entity.Property(e => e.IsImage).HasColumnName("is_image");
            entity.Property(e => e.Question).HasColumnName("question");
            entity.Property(e => e.Text)
                .HasColumnType("character varying")
                .HasColumnName("text");

            entity.HasOne(d => d.QuestionNavigation).WithMany(p => p.Answers)
                .HasForeignKey(d => d.Question)
                .HasConstraintName("answers_questions_fk");
        });

        modelBuilder.Entity<AnswerSoftSkill>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("answer_soft_skills_pk");

            entity.ToTable("answer_soft_skills", "Diplom");

            entity.HasIndex(e => new { e.Answer, e.SoftSkill }, "answer_soft_skills_unique").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Answer).HasColumnName("answer");
            entity.Property(e => e.Points).HasColumnName("points");
            entity.Property(e => e.SoftSkill).HasColumnName("soft_skill");

            entity.HasOne(d => d.AnswerNavigation).WithMany(p => p.AnswerSoftSkills)
                .HasForeignKey(d => d.Answer)
                .HasConstraintName("answer_soft_skills_answers_fk");

            entity.HasOne(d => d.SoftSkillNavigation).WithMany(p => p.AnswerSoftSkills)
                .HasForeignKey(d => d.SoftSkill)
                .HasConstraintName("answer_soft_skills_soft_skills_fk");
        });

        modelBuilder.Entity<Department>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("departments_pk");

            entity.ToTable("departments", "Diplom");

            entity.HasIndex(e => e.Name, "departments_unique").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Department1).HasColumnName("department");
            entity.Property(e => e.Image).HasColumnName("image");
            entity.Property(e => e.Description)
                .HasColumnType("character varying")
                .HasColumnName("description");
            entity.Property(e => e.Image).HasColumnName("image");
            entity.Property(e => e.Name)
                .HasColumnType("character varying")
                .HasColumnName("name");
        });

        modelBuilder.Entity<Gender>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("genders_pk");

            entity.ToTable("genders", "Diplom");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Name)
                .HasColumnType("character varying")
                .HasColumnName("name");
        });

        modelBuilder.Entity<Question>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("question_pk");

            entity.ToTable("questions", "Diplom");

            entity.Property(e => e.Id)
                .HasDefaultValueSql("nextval('\"Diplom\".question_id_seq'::regclass)")
                .HasColumnName("id");
            entity.Property(e => e.Department).HasColumnName("department");
            entity.Property(e => e.HasImage).HasColumnName("has_image");
            entity.Property(e => e.Image).HasColumnName("image");
            entity.Property(e => e.NumberInDepartment).HasColumnName("number_in_department");
            entity.Property(e => e.Text)
                .HasColumnType("character varying")
                .HasColumnName("text");

            entity.HasOne(d => d.DepartmentNavigation).WithMany(p => p.Questions)
                .HasForeignKey(d => d.Department)
                .HasConstraintName("questions_departments_fk");
        });

        modelBuilder.Entity<SoftSkill>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("soft_skills_pk");

            entity.ToTable("soft_skills", "Diplom");

            entity.HasIndex(e => e.Name, "soft_skills_unique").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Description)
                .HasColumnType("character varying")
                .HasColumnName("description");
            entity.Property(e => e.MaxPoints).HasColumnName("max_points");
            entity.Property(e => e.Name)
                .HasColumnType("character varying")
                .HasColumnName("name");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("users_pk");

            entity.ToTable("users", "Diplom");

            entity.HasIndex(e => e.Login, "users_unique").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Birthday)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("birthday");
            entity.Property(e => e.Gender).HasColumnName("gender");
            entity.Property(e => e.IsAdmin).HasColumnName("is_admin");
            entity.Property(e => e.IsBlocked).HasColumnName("is_blocked");
            entity.Property(e => e.Login)
                .HasColumnType("character varying")
                .HasColumnName("login");
            entity.Property(e => e.Password).HasColumnName("password");

            entity.HasOne(d => d.GenderNavigation).WithMany(p => p.Users)
                .HasForeignKey(d => d.Gender)
                .HasConstraintName("users_genders_fk");
        });

        modelBuilder.Entity<UserAchievement>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("user_achievements_pk");

            entity.ToTable("user_achievements", "Diplom");

            entity.HasIndex(e => new { e.User, e.Achievement }, "user_achievements_unique").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Achievement).HasColumnName("achievement");
            entity.Property(e => e.IsCompleted).HasColumnName("is_completed");
            entity.Property(e => e.User).HasColumnName("user");

            entity.HasOne(d => d.AchievementNavigation).WithMany(p => p.UserAchievements)
                .HasForeignKey(d => d.Achievement)
                .HasConstraintName("user_achievements_achievements_fk");

            entity.HasOne(d => d.UserNavigation).WithMany(p => p.UserAchievements)
                .HasForeignKey(d => d.User)
                .HasConstraintName("user_achievements_users_fk");
        });

        modelBuilder.Entity<UserQuestion>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("user_question_pk");

            entity.ToTable("user_question", "Diplom");

            entity.HasIndex(e => new { e.User, e.Question }, "user_question_unique").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Answer).HasColumnName("answer");
            entity.Property(e => e.IsAnswered).HasColumnName("is_answered");
            entity.Property(e => e.Question).HasColumnName("question");
            entity.Property(e => e.User).HasColumnName("user");

            entity.HasOne(d => d.AnswerNavigation).WithMany(p => p.UserQuestions)
                .HasForeignKey(d => d.Answer)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("user_question_answers_fk");

            entity.HasOne(d => d.QuestionNavigation).WithMany(p => p.UserQuestions)
                .HasForeignKey(d => d.Question)
                .HasConstraintName("user_question_questions_fk");

            entity.HasOne(d => d.UserNavigation).WithMany(p => p.UserQuestions)
                .HasForeignKey(d => d.User)
                .HasConstraintName("user_question_users_fk");
        });

        modelBuilder.Entity<UserSoftSkill>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("user_soft_skills_pk");

            entity.ToTable("user_soft_skills", "Diplom");

            entity.HasIndex(e => new { e.User, e.SoftSkill }, "user_soft_skills_unique").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Points).HasColumnName("points");
            entity.Property(e => e.SoftSkill).HasColumnName("soft_skill");
            entity.Property(e => e.User).HasColumnName("user");

            entity.HasOne(d => d.SoftSkillNavigation).WithMany(p => p.UserSoftSkills)
                .HasForeignKey(d => d.SoftSkill)
                .HasConstraintName("user_soft_skills_soft_skills_fk");

            entity.HasOne(d => d.UserNavigation).WithMany(p => p.UserSoftSkills)
                .HasForeignKey(d => d.User)
                .HasConstraintName("user_soft_skills_users_fk");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
