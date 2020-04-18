using System;

namespace ServerWeb.Services {
  public interface IServiceSample : IDisposable {
    void PrintHastCode(string ahead);
  }
}