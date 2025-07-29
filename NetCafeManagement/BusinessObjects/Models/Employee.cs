using System;
using System.Collections.Generic;

namespace BusinessObjects.Models;

public partial class Employee
{
    public int EmployeeId { get; set; }

    public int? AccountId { get; set; }

    public string FullName { get; set; } = null!;

    public string? Phone { get; set; }

    public string? Role { get; set; }

    public bool? IsActive { get; set; }

    public virtual Account? Account { get; set; }

    public virtual ICollection<Shift> Shifts { get; set; } = new List<Shift>();
}
