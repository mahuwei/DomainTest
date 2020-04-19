using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Domain.Events;

namespace Domain.Entities {
  public class Entity<TKey> {
    public TKey Id { get; set; }

    [DefaultValue(0)]
    public int Status { get; set; }

    [Timestamp]
    public byte[] RowFlag { get; set; }


    public string Memo { get; set; }

    #region

    private List<IDomainEvent> _domainEvents;
    public IReadOnlyCollection<IDomainEvent> DomainEvents => _domainEvents?.AsReadOnly();

    public void AddDomainEvent(IDomainEvent eventItem) {
      _domainEvents ??= new List<IDomainEvent>();
      _domainEvents.Add(eventItem);
    }

    public void RemoveDomainEvent(IDomainEvent eventItem) {
      _domainEvents?.Remove(eventItem);
    }

    public void ClearDomainEvents() {
      _domainEvents?.Clear();
    }

    #endregion
  }

  public enum EntityStatus {
    Normal = 0,
    Deleted = 1000
  }
}