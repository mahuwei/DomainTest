using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Domain.Entities {
  public class Address : ValueObject {
    public Address() { }

    public Address(string street, string city, string zipcode) {
      Street = street;
      City = city;
      ZipCode = zipcode;
    }

    public Address(Address address) {
      Street = address.Street;
      City = address.City;
      ZipCode = address.ZipCode;
    }

    [MaxLength(30)]
    public string Street { get; set; }

    [MaxLength(20)]
    public string City { get; set; }

    [MaxLength(10)]
    public string ZipCode { get; set; }

    protected override IEnumerable<object> GetAtomicValues() {
      // Using a yield return statement to return each element one at a time
      yield return Street;
      yield return City;
      yield return ZipCode;
    }
  }
}