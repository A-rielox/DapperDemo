using Dapper;
using DapperDemo.Models;
using Microsoft.Data.SqlClient;
using System.Data;
using System.Transactions;

namespace DapperDemo.Repository;

public class BonusRepository : IBonusRepository
{
    private IDbConnection db;

    public BonusRepository(IConfiguration configuration)
    {
        this.db = new SqlConnection(configuration.GetConnectionString("DefaultConnection"));
    }




    //////////////////////////////////////////////
    /////////////////////////////////////////////////
    public void AddTestCompanyWithEmployees(Company objComp)
    {
        var sql =   "INSERT INTO Companies (Name, Address, City, State, PostalCode) " +
                    "VALUES(@Name, @Address, @City, @State, @PostalCode);"

                 + "SELECT CAST(SCOPE_IDENTITY() as int); ";

        var id = db.Query<int>(sql, objComp).Single();

        objComp.CompanyId = id;

        //foreach(var employee in objComp.Employees)
        //{
        //    employee.CompanyId = objComp.CompanyId;
        //    var sql1 = "INSERT INTO Employees (Name, Title, Email, Phone, CompanyId) VALUES(@Name, @Title, @Email, @Phone, @CompanyId);"
        //           + "SELECT CAST(SCOPE_IDENTITY() as int); ";
        //    db.Query<int>(sql1, employee).Single();
        //}

        objComp.Employees.Select(c => { c.CompanyId = id; return c; }).ToList();

        var sqlEmp =    "INSERT INTO Employees (Name, Title, Email, Phone, CompanyId) " +
                        "VALUES(@Name, @Title, @Email, @Phone, @CompanyId);"
                 +      "SELECT CAST(SCOPE_IDENTITY() as int); ";

        db.Execute(sqlEmp, objComp.Employees);
    }


    //////////////////////////////////////////////
    /////////////////////////////////////////////////
    public void AddTestCompanyWithEmployeesWithTransaction(Company objComp)
    {
        using (var transaction = new TransactionScope())
        {
            try
            {
                var sql =   "INSERT INTO Companies (Name, Address, City, State, PostalCode) " +
                            "VALUES(@Name, @Address, @City, @State, @PostalCode);"

                          + "SELECT CAST(SCOPE_IDENTITY() as int); ";

                var id = db.Query<int>(sql, objComp).Single();
                
                objComp.CompanyId = id;

                objComp.Employees.Select(c => { c.CompanyId = id; return c; }).ToList();
                
                var sqlEmp =    "INSERT INTO Employees (Name, Title, Email, Phone, CompanyId) " +
                                "VALUES(@Name, @Title, @Email, @Phone, @CompanyId);"

                              + "SELECT CAST(SCOPE_IDENTITY() as int); ";

                db.Execute(sqlEmp, objComp.Employees);

                transaction.Complete();
            }
            catch (Exception ex)
            {

            }
        }
    }



    //////////////////////////////////////////////
    /////////////////////////////////////////////////
    /// one to many con 1 result ( el query me trae 1 tabla )
    /// one to many relation - 1 company -> many employees
    public List<Company> GetAllCompanyWithEmployees()
    {
        var sql = "SELECT C.*, E.* " +
                  "FROM Employees AS E " +
                  "INNER JOIN Companies AS C " +
                    "ON E.CompanyId = C.CompanyId ";

        var companyDic = new Dictionary<int, Company>();

        var company = db.Query<Company, Employee, Company>(sql, (c, e) =>
        {
            if (!companyDic.TryGetValue(c.CompanyId, out var currentCompany))
            {
                currentCompany = c;
                companyDic.Add(currentCompany.CompanyId, currentCompany);
            }

            currentCompany.Employees.Add(e);

            return currentCompany;

        }, splitOn: "EmployeeId");

        return company.Distinct().ToList();
    }


    //////////////////////////////////////////////
    /////////////////////////////////////////////////
    /// one to many con multiple results ( el query me trae 2 tablas )
    /// one to many relation - 1 company -> many employees
    public Company GetCompanyWithEmployees(int id)
    {
        var p = new
        {
            CompanyId = id
        };

        // este me devuelve 2 resultados ( las 2 tablas )
        var sql =   "SELECT * FROM Companies " +
                    "WHERE CompanyId = @CompanyId;"

                  + " SELECT * FROM Employees WHERE CompanyId = @CompanyId; ";

        Company company;

        // la "lists" va a tener los 2 resultados ( las 2 tablas )
        using (var lists = db.QueryMultiple(sql, p))
        {
            company = lists.Read<Company>().ToList().FirstOrDefault();
            company.Employees = lists.Read<Employee>().ToList();
        }

        return company;
    }


    //////////////////////////////////////////////
    /////////////////////////////////////////////////
    /// one to one relation - 1 employee -> 1 company
    public List<Employee> GetEmployeeWithCompany(int id)
    {
        var sql =   "SELECT E.*, C.* " +
                    "FROM Employees AS E " +
                    "INNER JOIN Companies AS C " +
                        "ON E.CompanyId = C.CompanyId ";

        if (id != 0)
        {
            sql += " WHERE E.CompanyId = @Id ";
        }

        // del query vamos a retirar los modelos Employee, Company, pero lo que hay q 
        // devolver es <_, _, Employee> ( tipo Employee )
        // va a agarrar la Company y hacer un populate en Employee (le va a agregar
        // los datos de la compania al Employee) y eso es lo q va a devolver.
        // luego con "(sql, (e, c) =>..." se le dice donde es que va a meterle la 
        // company al Employee
        var employee = db.Query<Employee, Company, Employee>(sql, (e, c) =>
        {
            e.Company = c;

            return e;
        }, new { id }, splitOn: "CompanyId");

        return employee.ToList();
    }


    //////////////////////////////////////////////
    /////////////////////////////////////////////////
    public void RemoveRange(int[] companyId)
    {
        db.Query("DELETE FROM Companies " +
                 "WHERE CompanyId IN @companyId",
                 new { companyId });
    }


    //////////////////////////////////////////////
    /////////////////////////////////////////////////
    public List<Company> FilterCompanyByName(string name)
    {
        return db.Query<Company>("SELECT * FROM Companies " +
                                 "WHERE Name like '%' + @name + '%' ",
                                 new { name })
                 .ToList();
    }


    
}
