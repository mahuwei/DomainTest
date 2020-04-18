using System;
using MediatR;

namespace Domain.Entities {
  public class Employee : Entity<Guid>, IRequest<Employee> {
    public string Name { get; set; }
  }
}