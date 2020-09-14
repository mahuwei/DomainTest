using System;
using System.ComponentModel.DataAnnotations;
using MediatR;

namespace Domain.Entities {
  /// <summary>
  ///   企业员工
  /// </summary>
  public class Employee : Entity<Guid>, IRequest<Employee> {
    public Guid CompanyId { get; set; }

    /// <summary>
    ///   手机号码
    /// </summary>

    [Required]
    [MaxLength(20)]
    public string MobileNo { get; set; }

    /// <summary>
    ///   名称
    /// </summary>
    [Required]
    [MaxLength(20)]
    public string Name { get; set; }

    /// <summary>
    ///   地址
    /// </summary>
    public Address Address { get; set; }
  }
}