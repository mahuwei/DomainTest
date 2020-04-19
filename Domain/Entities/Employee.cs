using System;
using System.ComponentModel.DataAnnotations;
using MediatR;

namespace Domain.Entities {
  public class Employee : Entity<Guid>, IRequest<Employee> {
    public Guid CompanyId { get; set; }

    [Required]
    [MaxLength(20)]
    public string MobileNo { get; set; }

    [Required]
    [MaxLength(20)]
    public string Name { get; set; }


    public Address Address { get; set; }
  }
}