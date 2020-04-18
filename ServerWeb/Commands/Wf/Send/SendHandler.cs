using System;
using System.Threading;
using System.Threading.Tasks;
using Domain.Entities;
using MediatR;

#pragma warning disable 1591

namespace ServerWeb.Commands.Wf.Send {
  public class GetSend : IRequest<Employee> {
  }

  public class SendHandler : IRequestHandler<GetSend, Employee> {
    public Task<Employee> Handle(GetSend request, CancellationToken cancellationToken) {
      var employee = new Employee { Id = Guid.NewGuid(), Name = $"姓名{DateTime.Now.ToLongTimeString()}" };
      return Task.FromResult(employee);
    }
  }
}