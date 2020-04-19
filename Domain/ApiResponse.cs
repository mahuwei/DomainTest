using System;
using FluentValidation;

namespace Domain {
  public class ApiResponse<T> {
    public ApiResponse(T data, int status = 0, string msg = "Ok", Exception ex = null) {
      Status = status;
      Msg = msg;
      Data = data;
      if (ex == null) {
        return;
      }

      ErrorDetail = BadRequestMessage.CreateMessage(ex, out var errMsg);
      Msg = errMsg;
    }

    /// <summary>
    ///   操作成功失败的最终标示：0=成功；!0=失败
    /// </summary>
    public int Status { get; set; }

    public string Msg { get; set; }
    public T Data { get; set; }
    public BadRequestMessage ErrorDetail { get; set; }
  }


  public class BadRequestMessage {
    public BadRequestMessageType MessageType { get; set; }
    public string Message { get; set; }

    public static BadRequestMessage CreateMessage(Exception e, out string message) {
      BadRequestMessage bm;
      if (e is ValidationException ev) {
        bm = new BadRequestMessage { MessageType = BadRequestMessageType.Validate, Message = ev.Message };
        message = "校验错误，查看详细信息。";
      }
      else {
        bm = new BadRequestMessage { MessageType = BadRequestMessageType.Other, Message = e.Message };
        message = e.Message;
      }

      return bm;
    }
  }

  public enum BadRequestMessageType {
    /// <summary>
    ///   数据校验错误
    /// </summary>
    Validate = 0,

    /// <summary>
    ///   其他错误
    /// </summary>
    Other = 100
  }
}