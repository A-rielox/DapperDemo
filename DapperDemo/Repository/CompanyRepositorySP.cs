using Dapper;
using DapperDemo.Data;
using DapperDemo.Models;
using Microsoft.Data.SqlClient;
using System.Data;

namespace DapperDemo.Repository;

public class CompanyRepositorySP : ICompanyRepository
{
    private IDbConnection db;

    public CompanyRepositorySP(IConfiguration configuration)
    {
        this.db = new SqlConnection(configuration.GetConnectionString("DefaultConnection"));
    }



    //////////////////////////////////////////////
    /////////////////////////////////////////////////
    public Company Add(Company company)
    {
        // esta forma de pasar parametros es para restringir mas lo que paso, pero tambien lo puedo hacer como en CompanyRepository.cs
        // que es pasar directo el "company"
        var parameters = new DynamicParameters();

        // @CompanyId es Output parameter en el sp
        parameters.Add("@CompanyId", 0, DbType.Int32, direction: ParameterDirection.Output);
        parameters.Add("@Name", company.Name);
        parameters.Add("@Address", company.Address);
        parameters.Add("@City", company.City);
        parameters.Add("@State", company.State);
        parameters.Add("@PostalCode", company.PostalCode);

        this.db.Execute("usp_AddCompany",
                         parameters,
                         commandType: CommandType.StoredProcedure);

        company.CompanyId = parameters.Get<int>("CompanyId");

        return company;
    }


    //////////////////////////////////////////////
    /////////////////////////////////////////////////
    public Company Find(int id)
    {
        return db.Query<Company>("usp_GetCompany",
                                  new { CompanyId = id },
                                  commandType: CommandType.StoredProcedure)
                 .Single();
    }


    //////////////////////////////////////////////
    /////////////////////////////////////////////////
    public List<Company> GetAll()
    {
        return db.Query<Company>("usp_GetALLCompany",
                                  commandType: CommandType.StoredProcedure)
                 .ToList();
    }


    //////////////////////////////////////////////
    /////////////////////////////////////////////////
    public void Remove(int id)
    {
        db.Execute("usp_RemoveCompany",
                    new { CompanyId = id },
                    commandType: CommandType.StoredProcedure);
    }


    //////////////////////////////////////////////
    /////////////////////////////////////////////////
    public Company Update(Company company)
    {
        var parameters = new DynamicParameters();

        parameters.Add("@CompanyId", company.CompanyId, DbType.Int32);
        parameters.Add("@Name", company.Name);
        parameters.Add("@Address", company.Address);
        parameters.Add("@City", company.City);
        parameters.Add("@State", company.State);
        parameters.Add("@PostalCode", company.PostalCode);

        this.db.Execute("usp_UpdateCompany",
                         parameters,
                         commandType: CommandType.StoredProcedure);

        return company;
    }
}
