using System;
using System.Collections.Generic;

namespace SoftSkillsAML.Models;

public partial class Department
{
    public string Name { get; set; } = null!;

    public string Description { get; set; } = null!;

    public int Id { get; set; }

    public byte[]? Image { get; set; }

    public byte[]? Image { get; set; }

    public byte[]? Image { get; set; }

    public virtual ICollection<Question> Questions { get; set; } = new List<Question>();
}
