using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Domain.Entities {
  /// <summary>
  ///   地址信息
  /// </summary>
  public class Address : ValueObject {
    /// <summary>
    ///   构造函数
    /// </summary>
    public Address() { }

    /// <summary>
    ///   构造函数
    /// </summary>
    /// <param name="street"></param>
    /// <param name="city"></param>
    /// <param name="zipcode"></param>
    public Address(string street, string city, string zipcode) {
      Street = street;
      City = city;
      ZipCode = zipcode;
    }

    /// <summary>
    ///   构造函数
    /// </summary>
    /// <param name="address"></param>
    public Address(Address address) {
      Street = address.Street;
      City = address.City;
      ZipCode = address.ZipCode;
    }

    /// <summary>
    ///   街道
    /// </summary>
    [MaxLength(30)]
    public string Street { get; set; }

    /// <summary>
    ///   城市
    /// </summary>
    [MaxLength(20)]
    public string City { get; set; }

    /// <summary>
    ///   区号
    /// </summary>
    [MaxLength(10)]
    public string ZipCode { get; set; }

    /// <summary>
    ///   返回值
    /// </summary>
    /// <returns></returns>
    protected override IEnumerable<object> GetAtomicValues() {
      // Using a yield return statement to return each element one at a time
      yield return Street;
      yield return City;
      yield return ZipCode;
    }
  }
}