using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities {
  public class Company : Entity<Guid> {
    public string No { get; set; }
    public string Name { get; set; }
    public Address Address { get; set; }

    public List<Employee> Employees { get; set; }
  }
}