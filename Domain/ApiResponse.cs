using System;
using FluentValidation;

namespace Domain {
  /// <summary>
  ///   Api返回对象
  /// </summary>
  /// <typeparam name="T"></typeparam>
  public class ApiResponse<T> {
    /// <summary>
    ///   构造函数
    /// </summary>
    /// <param name="data"></param>
    /// <param name="status"></param>
    /// <param name="msg"></param>
    /// <param name="ex"></param>
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

    /// <summary>
    ///   错误消息
    /// </summary>
    public string Msg { get; set; }

    /// <summary>
    ///   返回数据
    /// </summary>
    public T Data { get; set; }

    /// <summary>
    ///   错误信息
    /// </summary>
    public BadRequestMessage ErrorDetail { get; set; }
  }

  /// <summary>
  ///   错误信息
  /// </summary>
  public class BadRequestMessage {
    /// <summary>
    ///   错误类型
    /// </summary>
    public BadRequestMessageType MessageType { get; set; }

    /// <summary>
    ///   错误信息
    /// </summary>
    public string Message { get; set; }

    /// <summary>
    ///   生成返回错误
    /// </summary>
    /// <param name="e"></param>
    /// <param name="message"></param>
    /// <returns></returns>
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

  /// <summary>
  ///   错误类型
  /// </summary>
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