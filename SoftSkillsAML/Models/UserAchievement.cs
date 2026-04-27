using System;
using System.Collections.Generic;

namespace SoftSkillsAML.Models;

public partial class UserAchievement
{
    public int Id { get; set; }

    public int User { get; set; }

    public int Achievement { get; set; }

    public bool IsCompleted { get; set; }

    public virtual Achievement AchievementNavigation { get; set; } = null!;

    public virtual User UserNavigation { get; set; } = null!;
}
