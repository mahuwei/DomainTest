using Domain.Entities;
using MediatR;

namespace Domain.Events {
  public interface IDomainEvent : INotification {
  }

  public interface IDomainEventHandler<in TDomainEvent> : INotificationHandler<TDomainEvent>
    where TDomainEvent : IDomainEvent {
  }

  public class CompanyCreatedEvent : IDomainEvent {
    public CompanyCreatedEvent(Company company) {
      Company = company;
    }

    public Company Company { get; }
  }
}