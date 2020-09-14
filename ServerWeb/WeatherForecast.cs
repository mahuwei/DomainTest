using System;

#pragma warning disable 1591

namespace ServerWeb {
  /// <summary>
  ///   天气预报
  /// </summary>
  public class WeatherForecast {
    /// <summary>
    ///   日期
    /// </summary>
    public DateTime Date { get; set; }

    /// <summary>
    ///   摄氏度
    /// </summary>
    public int TemperatureC { get; set; }

    /// <summary>
    ///   华氏度
    /// </summary>
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);

    /// <summary>
    ///   摘要
    /// </summary>
    public string Summary { get; set; }
  }
}