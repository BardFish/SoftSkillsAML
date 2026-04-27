using System;
using System.Collections.Generic;

namespace SoftSkillsAML.Models;

public partial class UserSoftSkill
{
    public int Id { get; set; }

    public int User { get; set; }

    public int SoftSkill { get; set; }

    public int Points { get; set; }

    public virtual SoftSkill SoftSkillNavigation { get; set; } = null!;

    public virtual User UserNavigation { get; set; } = null!;
}
