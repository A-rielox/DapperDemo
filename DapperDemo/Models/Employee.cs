using Dapper.Contrib.Extensions;

namespace DapperDemo.Models;

public class Employee
{
    // NO quiero la [key] de DataAnotations, quiero la de Dapper
    // tengo q instalar Dapper.contrib en el packageManager
    //[Key]
    public int EmployeeId { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    public string Phone { get; set; }
    public string Title { get; set; }

    public int CompanyId { get; set; }
    public virtual Company Company { get; set; }
}
