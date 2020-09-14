using Domain.Entities;

namespace Domain.Dtos {
  /// <summary>
  ///   企业Dto
  /// </summary>
  public class CompanyDto {
    /// <summary>
    ///   编号
    /// </summary>
    public string No { get; set; }

    /// <summary>
    ///   名称
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    ///   状态
    /// </summary>
    public int Status { get; set; }

    /// <summary>
    ///   地址
    /// </summary>
    public Address Address { get; set; }
  }
}