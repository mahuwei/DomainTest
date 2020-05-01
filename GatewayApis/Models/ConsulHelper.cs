using System;
using System.Threading.Tasks;
using Consul;
using Microsoft.Extensions.Configuration;

namespace Models {
  public class ConsulHelper {
    public static async Task<WriteResult> ServiceRegister(IConfiguration configuration) {
      var client = new ConsulClient(t => {
        //这是Consul的服务地址192.168.31.140
        t.Address = new Uri($"{configuration["Consul:ServicesIp"]}:{configuration["Consul:Port"]}");
        //储存名
        t.Datacenter = configuration["Consul:DataCenter"];
      });

      //注册一个实例
      return await client.Agent.ServiceRegister(new AgentServiceRegistration {
        //注册服务的IP
        Address = configuration["Consul:LocalIp"],
        //服务id唯一的
        ID = $"{configuration["Consul:ServiceName"]}-1",
        //服务名
        Name = configuration["Consul:ServiceName"],
        //端口
        Port = Convert.ToInt32(configuration["Consul:LocalPort"]),
        Tags = null,
        Check = new AgentServiceCheck {
          //健康检查的API地址
          HTTP =
            $"http://{configuration["Consul:LocalIp"]}:{configuration["Consul:LocalPort"]}{configuration["Consul:HealthCheck"]}",
          Status = HealthStatus.Passing,
          //间隔多少检查一次
          Interval = new TimeSpan(0, 0, 30),
          //多久注销不健康的服务
          DeregisterCriticalServiceAfter = new TimeSpan(0, 0, 30)
        }
      });
    }
  }

  public class BaseConfig {
    public string Kafka { get; set; }
    public string Mysql { get; set; }
    public string Redis { get; set; }
  }
}