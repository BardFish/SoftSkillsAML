using System;
using System.Collections.Generic;

namespace SoftSkillsAML.Models;

public partial class Question
{
    public int Id { get; set; }

    public string Text { get; set; } = null!;

    public byte[]? Image { get; set; }

    public bool HasImage { get; set; }

    public int Department { get; set; }

    public int NumberInDepartment { get; set; }

    public virtual ICollection<Answer> Answers { get; set; } = new List<Answer>();

    public virtual Department DepartmentNavigation { get; set; } = null!;

    public virtual ICollection<UserQuestion> UserQuestions { get; set; } = new List<UserQuestion>();
}
