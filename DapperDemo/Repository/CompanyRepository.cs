using Dapper;
using DapperDemo.Data;
using DapperDemo.Models;
using Microsoft.Data.SqlClient; // p' SqlConnection
using System.Data;

namespace DapperDemo.Repository;

public class CompanyRepository : ICompanyRepository
{
    private IDbConnection db;

    public CompanyRepository(IConfiguration configuration)
    {
        this.db = new SqlConnection(configuration.GetConnectionString("DefaultConnection"));
    }



    //////////////////////////////////////////////
    /////////////////////////////////////////////////
    public Company Add(Company company)
    {
        var sql = "INSERT INTO Companies (Name, Address, City, State, PostalCode) " +
                  "VALUES(@Name, @Address, @City, @State, @PostalCode);" +

                  "SELECT CAST(SCOPE_IDENTITY() as int); ";

        // var id = db.Query<int>(sql, new {
        //     @Name = company.Name,
        //     @Address = company.Address,
        //     @City = company.City,
        //     @State = company.State,
        //     @PostalCode = company.PostalCode
        // }).Single();

        // como los field names son ='s => Dapper lo hace solo
        var id = db.Query<int>(sql, company).Single();

        company.CompanyId = id;

        return company;
    }


    //////////////////////////////////////////////
    /////////////////////////////////////////////////
    public Company Find(int id)
    {
        var sql = "SELECT * FROM Companies " +
                  "WHERE CompanyId = @CompanyId";

        return db.Query<Company>(sql, new { @CompanyId = id }).Single();
    }


    //////////////////////////////////////////////
    /////////////////////////////////////////////////
    public List<Company> GetAll()
    {
        var sql = "SELECT * FROM Companies";

        return db.Query<Company>(sql).ToList();
    }


    //////////////////////////////////////////////
    /////////////////////////////////////////////////
    public void Remove(int id)
    {
        var sql = "DELETE FROM Companies WHERE CompanyId = @Id";

        db.Execute(sql, new { id });
    }


    //////////////////////////////////////////////
    /////////////////////////////////////////////////
    public Company Update(Company company)
    {
        // tiene q ir con @CompanyId no puede ser solo "Id" xq es lo q tengo en el modelo
        // y es lo q utiliza p' hacer el binding automatico y poder pasar solo "company" en el Execute
        var sql =   "UPDATE Companies " +
                    "SET Name = @Name, Address = @Address, City = @City, " +
                        "State = @State, PostalCode = @PostalCode " +
                    "WHERE CompanyId = @CompanyId";

        // Execute p' cuando no quiero q devuelva algo, solo q ejecute
        db.Execute(sql, company);

        return company;
    }
}
