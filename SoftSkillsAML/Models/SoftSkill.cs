using System;
using System.Collections.Generic;

namespace SoftSkillsAML.Models;

public partial class SoftSkill
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string Description { get; set; } = null!;

    public int MaxPoints { get; set; }

    public virtual ICollection<AnswerSoftSkill> AnswerSoftSkills { get; set; } = new List<AnswerSoftSkill>();

    public virtual ICollection<UserSoftSkill> UserSoftSkills { get; set; } = new List<UserSoftSkill>();
}
