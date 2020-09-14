using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;

#pragma warning disable 1591

namespace ServerWeb.Commands.Wf.MediatPublish {
  public class Ping : INotification {
    public string Info { get; set; }
  }

  public class PongA : INotificationHandler<Ping> {
    private readonly ILogger<PongA> _logger;

    public PongA(ILogger<PongA> logger) {
      _logger = logger;
    }

    public Task Handle(Ping notification, CancellationToken cancellationToken) {
      _logger.LogDebug("PongA notification:{@notification}", notification);
      return Task.CompletedTask;
    }
  }

  public class PongB : INotificationHandler<Ping> {
    private readonly ILogger<PongB> _logger;

    public PongB(ILogger<PongB> logger) {
      _logger = logger;
    }

    public Task Handle(Ping notification, CancellationToken cancellationToken) {
      _logger.LogDebug("PongB notification:{@notification}", notification);
      return Task.CompletedTask;
    }
  }
}