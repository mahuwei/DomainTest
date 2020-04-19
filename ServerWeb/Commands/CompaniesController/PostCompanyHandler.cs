using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Domain;
using Domain.Dtos;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

#pragma warning disable 1591

namespace ServerWeb.Commands.CompaniesController {
  public class PostCompany : IRequest<Company> {
    public PostCompany(CompanyDto dto) {
      Dto = dto;
    }
    public CompanyDto Dto { get; set; }
  }

  public class PostCompanyHandler : IRequestHandler<PostCompany, Company> {
    private readonly DomainContext _dc;
    private readonly IMapper _mapper;

    public PostCompanyHandler(IMapper mapper, DomainContext dc) {
      _mapper = mapper;
      _dc = dc;
    }

    public async Task<Company> Handle(PostCompany request, CancellationToken cancellationToken) {
      var company = _mapper.Map<Company>(request.Dto);
      if (await _dc.Companies.AnyAsync(d => d.No == company.No, cancellationToken)) {
        throw new Exception($"已存在相同编号：{company.No}公司");
      }

      company.Id = Guid.NewGuid();
      await _dc.Companies.AddAsync(company, cancellationToken);
      await _dc.SaveChangesAsync(cancellationToken);
      return company;
    }
  }
}