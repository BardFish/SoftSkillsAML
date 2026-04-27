using System;
using System.Collections.Generic;

namespace SoftSkillsAML.Models;

public partial class UserQuestion
{
    public int Id { get; set; }

    public int User { get; set; }

    public int Question { get; set; }

    public bool IsAnswered { get; set; }

    public int? Answer { get; set; }

    public virtual Answer? AnswerNavigation { get; set; }

    public virtual Question QuestionNavigation { get; set; } = null!;

    public virtual User UserNavigation { get; set; } = null!;
}
