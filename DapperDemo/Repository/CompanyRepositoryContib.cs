using Dapper.Contrib.Extensions;
using DapperDemo.Models;
using Microsoft.Data.SqlClient;
using System.Data;

namespace DapperDemo.Repository;

public class CompanyRepositoryContib : ICompanyRepository
{

    private IDbConnection db;

    public CompanyRepositoryContib(IConfiguration configuration)
    {
        this.db = new SqlConnection(configuration.GetConnectionString("DefaultConnection"));
    }




    //////////////////////////////////////////////
    /////////////////////////////////////////////////
    public Company Add(Company company)
    {
        // .Insert devuelve automaticamente el id d lo q creo
        var id = db.Insert(company);

        company.CompanyId = (int)id;

        return company;
    }


    //////////////////////////////////////////////
    /////////////////////////////////////////////////
    public Company Find(int id)
    {
        return db.Get<Company>(id);
    }


    //////////////////////////////////////////////
    /////////////////////////////////////////////////
    public List<Company> GetAll()
    {
        return db.GetAll<Company>().ToList();
    }


    //////////////////////////////////////////////
    /////////////////////////////////////////////////
    public void Remove(int id)
    {
        db.Delete(new Company { CompanyId = id });
    }


    //////////////////////////////////////////////
    /////////////////////////////////////////////////
    public Company Update(Company company)
    {
        db.Update(company);

        return company;
    }

}
