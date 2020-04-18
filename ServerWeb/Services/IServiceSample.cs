using System;
#pragma warning disable 1591

namespace ServerWeb.Services {
  public interface IServiceSample : IDisposable {
    void PrintHastCode(string ahead);
  }
}