using System;
using Domain;
using Microsoft.AspNetCore.Mvc;

namespace ServerWeb.Commands {
  /// <summary>
  ///   统一Api返回对象
  /// </summary>
  public class ControllerApiBase : ControllerBase {
    /// <summary>
    ///   统一返回的结果格式；
    ///   不论操作是否功能，都返回该对象（HttpCode都是200），需要通过返回对象的status来判断最终是否成功。
    /// </summary>
    /// <param name="data">操作成功返回数据数据</param>
    /// <param name="status">操作成功失败的最终标示：0=成功；!0=失败</param>
    /// <param name="msg">操作失败消息</param>
    /// <param name="ex">错误信息</param>
    /// <returns></returns>
    internal ApiResponse<T> Output<T>(T data, int status = 0, string msg = "ok", Exception ex = null) {
      return new ApiResponse<T>(data, status, msg, ex);
    }
  }
}