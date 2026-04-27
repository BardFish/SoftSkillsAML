using System;
using System.Collections.Generic;

namespace SoftSkillsAML.Models;

public partial class Achievement
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string Description { get; set; } = null!;

    public byte[] Image { get; set; } = null!;

    public virtual ICollection<UserAchievement> UserAchievements { get; set; } = new List<UserAchievement>();
}
