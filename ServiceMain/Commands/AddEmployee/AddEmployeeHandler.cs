using System;
using System.Threading;
using System.Threading.Tasks;
using Domain;
using Domain.Entities;
using MediatR;
using Microsoft.Extensions.Logging;
using ServiceMain.Services;

namespace ServiceMain.Commands.AddEmployee {
  public class AddEmployeeHandler : IRequestHandler<Employee, Employee> {
    private readonly ILogger<AddEmployeeHandler> _logger;
    private readonly SampleSingletonService _service;
    private readonly ISampleService _transientService;

    public AddEmployeeHandler(ILogger<AddEmployeeHandler> logger,
      //IOptionsSnapshot<ConfigSetting> config,
      SampleSingletonService service,
      SampleTransientService transientService) {
      _logger = logger;
      _service = service;
      _transientService = transientService;
      //_logger.LogDebug("config：{@config}", config.Value);
    }

    public Task<Employee> Handle(Employee request, CancellationToken cancellationToken) {
      _logger.LogDebug("获取HashCode:{@code}", GetHashCode());
      _service.PrintHastCode();
      _transientService.PrintHastCode();
      if (request.Id == Guid.Empty) {
        request.Id = Guid.NewGuid();
      }
      else {
        request.Name = $"{request.Name}-修改";
      }

      return Task.FromResult(request);
    }
  }
}