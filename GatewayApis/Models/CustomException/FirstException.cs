using System;

namespace Models.CustomException {
  public class FirstException : Exception {
    public FirstException(string message = null, Exception exception = null) : base(message, exception) {
    }
  }
}