using System;
using System.Collections.Generic;

namespace SoftSkillsAML.Models;

public partial class Answer
{
    public int Id { get; set; }

    public int Question { get; set; }

    public bool IsCorrect { get; set; }

    public string? Text { get; set; }

    public byte[]? Image { get; set; }

    public bool IsImage { get; set; }

    public virtual ICollection<AnswerSoftSkill> AnswerSoftSkills { get; set; } = new List<AnswerSoftSkill>();

    public virtual Question QuestionNavigation { get; set; } = null!;

    public virtual ICollection<UserQuestion> UserQuestions { get; set; } = new List<UserQuestion>();
}
