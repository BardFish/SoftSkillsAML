using System;
using System.Collections.Generic;

namespace SoftSkillsAML.Models;

public partial class AnswerSoftSkill
{
    public int Id { get; set; }

    public int Answer { get; set; }

    public int SoftSkill { get; set; }

    public int Points { get; set; }

    public virtual Answer AnswerNavigation { get; set; } = null!;

    public virtual SoftSkill SoftSkillNavigation { get; set; } = null!;
}
