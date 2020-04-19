using Domain.Entities;

namespace Domain.Dtos {
  public class CompanyDto {
    public string No { get; set; }
    public string Name { get; set; }
    public int Status { get; set; }
    
    public Address Address { get; set; }
  }
}