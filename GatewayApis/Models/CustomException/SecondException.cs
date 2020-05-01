using System;

namespace Models.CustomException {
  public class SecondException : Exception {
    public SecondException(string message = null, Exception exception = null) : base(message, exception) {
    }
  }
}