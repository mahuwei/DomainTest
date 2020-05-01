using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using Models.CustomException;

namespace Models.Filters {
  public class CustomExceptionFilterAttribute : ExceptionFilterAttribute {
    private readonly ILogger _logger;

    public CustomExceptionFilterAttribute(ILoggerFactory loggerFactory) {
      _logger = loggerFactory.CreateLogger<CustomExceptionFilterAttribute>();
    }

    public override void OnException(ExceptionContext context) {
      if (context.ExceptionHandled) {
        return;
      }

      switch (context.Exception) {
        case FirstException fe:
          _logger.LogWarning(fe, fe.Message);
          break;
        case SecondException se:
          _logger.LogError(se, se.Message);
          break;
        case { } ex:
          _logger.LogError(ex, ex.Message);
          break;
      }

      base.OnException(context);
    }

    public override Task OnExceptionAsync(ExceptionContext context) {
      if (context.ExceptionHandled) {
        return OnExceptionAsync(context);
      }

      switch (context.Exception) {
        case FirstException fe:
          _logger.LogError(fe, fe.Message);
          break;
        case SecondException se:
          _logger.LogError(se, se.Message);
          break;
        case { } ex:
          _logger.LogError(ex, ex.Message);
          break;
      }

      return base.OnExceptionAsync(context);
    }
  }
}