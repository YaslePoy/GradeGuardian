using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace GGApp;

public abstract class DbEntity
{
    [Key] public int Id { get; set; }
}

[Table("Users", Schema = "public")]
public class User : DbEntity
{
    public string Name { get; set; }
    public string Surname { get; set; }
    public string Email { get; set; }
    public string Login { get; set; }
    public string Password { get; set; }
    public string Phone { get; set; }
    public int RoleId { get; set; }
}

[Table("Subjects", Schema = "public")]
public class Subject : DbEntity
{
    public string Name { get; set; }
    [ForeignKey("Teacher")] public int TeacherId { get; set; }
    public User Teacher { get; set; }
}

[Table("Lessons", Schema = "public")]
public class Lesson : DbEntity
{
    [ForeignKey("Subject")] public int SubjectId { get; set; }
    public Subject Subject { get; set; }
    public DateTime LessonDay { get; set; }
    public string Task { get; set; }
    public DateTime? Deadline { get; set; }
}

[Table("Grades", Schema = "public")]
public class Grade : DbEntity
{
    public int StudentId { get; set; }
    [ForeignKey("StudentId")] public User Student { get; set; }
    public int LessonId { get; set; }
    [ForeignKey("LessonId")] public Lesson Lesson { get; set; }

    public int Value { get; set; }
    public DateTime SetupDate { get; set; }
    public string Comment { get; set; }
}

[Table("Attendences", Schema = "public")]
public class Attendence : DbEntity
{
    public int StudentId { get; set; }
    [ForeignKey("StudentId")] public User Student { get; set; }
    public int LessonId { get; set; }
    [ForeignKey("LessonId")] public Lesson Lesson { get; set; }
    public bool IsPass { get; set; }
    public bool IsLate { get; set; }
}

public class GGContext : DbContext
{
    public DbSet<User> Users { get; set; }
    public DbSet<Lesson> Lessons { get; set; }
    public DbSet<Grade> Grades { get; set; }
    public DbSet<Subject> Subjects { get; set; }
    public DbSet<Attendence> Attendences { get; set; }
    
    public GGContext()
    {
        // Не создаём схему автоматически — используем существующие таблицы
        // Database.EnsureCreated();
    }
    
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseNpgsql("Host=micialware.ru;Port=5432;Database=ggdb;Username=trieco_admin;Password=trieco");
    }
}