using System;
using System.Collections.Generic;
using System.Linq;
using Domain.Entities;

namespace Domain {
  public class SeedData {
    public static void Create(DomainContext dc) {
      if (dc.Companies.Any()) {
        return;
      }

      var address = new Address("平阳路{0}号", "太原", "030012");

      var companies = new List<Company>();
      for (var i = 0; i < 3; i++) {
        var company = new Company {
          Id = Guid.NewGuid(),
          No = (i + 1).ToString("00"),
          Name = $"公司-{i + 1:00}",
          Status = (int)EntityStatus.Normal,
          Address = new Address(address)
        };
        company.Address.Street = string.Format(company.Address.Street, 18 + i);
        companies.Add(company);
      }

      var employees = new List<Employee>();
      var mn = 13900000000;
      foreach (var company in companies) {
        for (var i = 0; i < 3; i++) {
          var employee = new Employee {
            Id = Guid.NewGuid(),
            CompanyId = company.Id,
            MobileNo = (mn + (Convert.ToInt32(company.No) * 1000) + i + 1).ToString(),
            Name = $"{company.Name}-{i + 1:000}",
            Address = new Address(company.Address),
            Status = (int)EntityStatus.Normal
          };
          employees.Add(employee);
        }
      }

      dc.Companies.AddRange(companies);
      dc.Employees.AddRange(employees);
      dc.SaveChanges();
    }
  }
}