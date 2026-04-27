using System;
using System.Collections.Generic;

namespace SoftSkillsAML.Models;

public partial class User
{
    public int Id { get; set; }

    public int Gender { get; set; }

    public string Login { get; set; } = null!;

    public byte[] Password { get; set; } = null!;

    public DateTime Birthday { get; set; }

    public bool IsBlocked { get; set; }

    public bool IsAdmin { get; set; }

    public virtual Gender GenderNavigation { get; set; } = null!;

    public virtual ICollection<UserAchievement> UserAchievements { get; set; } = new List<UserAchievement>();

    public virtual ICollection<UserQuestion> UserQuestions { get; set; } = new List<UserQuestion>();

    public virtual ICollection<UserSoftSkill> UserSoftSkills { get; set; } = new List<UserSoftSkill>();
}
