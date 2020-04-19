using System.Collections.Generic;
using System.Threading.Tasks;
using Domain;
using Domain.Dtos;
using Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ServerWeb.Commands;
using ServerWeb.Commands.CompaniesController;

#pragma warning disable 1591

namespace ServerWeb.Controllers {
  [ApiExplorerSettings(GroupName = "v1")]
  [ApiController]
  [Route("[controller]")]
  public class CompaniesController : ControllerApiBase {
    private readonly ILogger<CompaniesController> _logger;
    private readonly IMediator _mediator;

    public CompaniesController(ILogger<CompaniesController> logger, IMediator mediator) {
      _logger = logger;
      _mediator = mediator;
    }

    [HttpGet]
    public async Task<ActionResult<ApiResponse<List<Company>>>> Get() {
      var result = await _mediator.Send(new GetCompanies());
      return Ok(Output(result));
    }
    
    [HttpPost]
    public async Task<ActionResult<ApiResponse<Company>>> Post([FromBody]CompanyDto dto) {
      var result = await _mediator.Send(new PostCompany(dto));
      return Ok(Output(result));
    }
  }
}