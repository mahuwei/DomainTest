using System;
using System.Collections.Generic;

namespace Domain.Entities {
  /// <summary>
  ///   企业信息
  /// </summary>
  public class Company : Entity<Guid> {
    /// <summary>
    ///   企业编号
    /// </summary>
    public string No { get; set; }

    /// <summary>
    ///   企业名称
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    ///   企业地址
    /// </summary>
    public Address Address { get; set; }

    /// <summary>
    ///   企业员工
    /// </summary>
    public List<Employee> Employees { get; set; }
  }
}