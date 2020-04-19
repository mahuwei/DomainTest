using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading;
using System.Threading.Tasks;
using Domain.Entities;
using Domain.Events;
using DotNetCore.CAP;
using Newtonsoft.Json;

#pragma warning disable 1591

namespace ServerWeb.DomainEventHandlers {
  public class CompanyCreatedDomainEventHandler : IDomainEventHandler<CompanyCreatedEvent> {
    private readonly ICapPublisher _capPublisher;

    public CompanyCreatedDomainEventHandler(ICapPublisher capPublisher) {
      _capPublisher = capPublisher;
    }

    public async Task Handle(CompanyCreatedEvent notification, CancellationToken cancellationToken) {
      var headers =
        new Dictionary<string, string> { { "createdTime", DateTime.Now.ToString(CultureInfo.InvariantCulture) } };
      await _capPublisher.PublishAsync(EntitiesCreatedIntegrationEvent.Topic,
        new EntitiesCreatedIntegrationEvent(typeof(Company).FullName, false,
          JsonConvert.SerializeObject(notification.Company)),
        headers,
        cancellationToken);
    }
  }
}