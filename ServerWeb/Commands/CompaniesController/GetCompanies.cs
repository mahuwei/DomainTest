using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Domain;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

#pragma warning disable 1591

namespace ServerWeb.Commands.CompaniesController {
  public class GetCompanies : IRequest<List<Company>> {
  }

  public class GetCompaniesHandler : IRequestHandler<GetCompanies, List<Company>> {
    private readonly DomainContext _dc;

    public GetCompaniesHandler(DomainContext dc) {
      _dc = dc;
    }

    public async Task<List<Company>> Handle(GetCompanies request, CancellationToken cancellationToken) {
      // var list = _dc.Companies.AsNoTracking().Where(d => d.Status == (int)EntityStatus.Normal).ToList();
      // return Task.FromResult(list);

      var list = await _dc.Companies.AsNoTracking()
        .Where(d => d.Status == (int)EntityStatus.Normal)
        .ToListAsync(cancellationToken);
      return list;
    }
  }
}