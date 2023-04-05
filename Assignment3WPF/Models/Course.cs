using System;
using System.Collections.Generic;

namespace Assignment3WPF.Models;

public partial class Course
{
    public string Coursecode { get; set; } = null!;

    public string Coursename { get; set; } = null!;

    public string Semester { get; set; } = null!;

    public string Teacher { get; set; } = null!;

    public virtual ICollection<Grade> Grades { get; } = new List<Grade>();
}
