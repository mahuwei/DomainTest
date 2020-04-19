using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Domain;
using Domain.Dtos;
using Domain.Entities;
using DotNetCore.CAP;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using ServerWeb.DomainEventHandlers;

#pragma warning disable 1591

namespace ServerWeb.Commands.CompaniesController {
  public class PostCompany : IRequest<Company> {
    public PostCompany(CompanyDto dto) {
      Dto = dto;
    }

    public CompanyDto Dto { get; set; }
  }

  public class PostCompanyHandler : IRequestHandler<PostCompany, Company> {
    private readonly ICapPublisher _capBus;
    private readonly DomainContext _dc;
    private readonly IMapper _mapper;

    public PostCompanyHandler(IMapper mapper, DomainContext dc, ICapPublisher capPublisher) {
      _mapper = mapper;
      _dc = dc;
      _capBus = capPublisher ?? throw new ArgumentException(nameof(capPublisher));
    }

    public async Task<Company> Handle(PostCompany request, CancellationToken cancellationToken) {
      var company = _mapper.Map<Company>(request.Dto);
      if (await _dc.Companies.AnyAsync(d => d.No == company.No, cancellationToken)) {
        throw new Exception($"已存在相同编号：{company.No}公司");
      }

      await using var trans = _dc.Database.BeginTransaction(_capBus, true);
      company.Id = Guid.NewGuid();
      await _dc.Companies.AddAsync(company, cancellationToken);

      await _dc.SaveChangesAsync(cancellationToken);
      var headers =
        new Dictionary<string, string> { { "createdTime", DateTime.Now.ToString(CultureInfo.InvariantCulture) } };
      await _capBus.PublishAsync(EntitiesCreatedIntegrationEvent.Topic,
        new EntitiesCreatedIntegrationEvent(typeof(Company).FullName, false, JsonConvert.SerializeObject(company)),
        headers, cancellationToken);
      return company;
    }
  }
}