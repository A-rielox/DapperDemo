using Dapper.Contrib.Extensions;
using System.ComponentModel.DataAnnotations.Schema;

namespace DapperDemo.Models;

// p'q Dapper sepa el nombre de la tabla, ya que buscaba en "Companys" xdefault
[Dapper.Contrib.Extensions.Table("Companies")]
public class Company
{
    // NO quiero la [key] de DataAnotations, quiero la de Dapper
    // tengo q instalar Dapper.contrib en el packageManager
    [Key]
    public int CompanyId { get; set; }
    public string Name { get; set; }

    public string Address { get; set; }
    public string City { get; set; }
    public string State { get; set; }
    public string PostalCode { get; set; }

    // p' decirle a Mapper que esta prop no es writeable ( esta prop NO va a la tabla )
    [Write(false)]
    public List<Employee> Employees { get; set; } = new List<Employee>();
}
