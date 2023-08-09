using Microsoft.AspNetCore.Mvc;
using DapperDemo.Models;
using DapperDemo.Repository;

namespace DapperDemo.Controllers;

public class CompaniesController : Controller
{
    private readonly ICompanyRepository _compRepo;

    public CompaniesController(ICompanyRepository compRepo)
    {
        _compRepo = compRepo;
    }


    //////////////////////////////////////////////
    /////////////////////////////////////////////////
    // GET: Companies
    public async Task<IActionResult> Index()
    {
        return View(_compRepo.GetAll());
    }


    //////////////////////////////////////////////
    /////////////////////////////////////////////////
    // GET: Companies/Details/5
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var company = _compRepo.Find(id.GetValueOrDefault());
        if (company == null)
        {
            return NotFound();
        }

        return View(company);
    }


    //////////////////////////////////////////////
    /////////////////////////////////////////////////
    // GET: Companies/Create
    public IActionResult Create()
    {
        return View();
    }

    // POST: Companies/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("CompanyId,Name,Address,City,State,PostalCode")] Company company)
    {
        ModelState.Remove("Employees");
        if (ModelState.IsValid)
        {
            _compRepo.Add(company);
            return RedirectToAction(nameof(Index));
        }
        return View(company);
    }


    //////////////////////////////////////////////
    /////////////////////////////////////////////////
    // GET: Companies/Edit/5
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var company = _compRepo.Find(id.GetValueOrDefault());
        if (company == null)
        {
            return NotFound();
        }
        return View(company);
    }

    // POST: Companies/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, [Bind("CompanyId,Name,Address,City,State,PostalCode")] Company company)
    {
        if (id != company.CompanyId)
        {
            return NotFound();
        }
        
        ModelState.Remove("Employees");
        if (ModelState.IsValid)
        {
            _compRepo.Update(company);
            return RedirectToAction(nameof(Index));
        }

        return View(company);
    }


    //////////////////////////////////////////////
    /////////////////////////////////////////////////
    // GET: Companies/Delete/5
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null )
        {
            return NotFound();
        }

        _compRepo.Remove(id.GetValueOrDefault());

        return RedirectToAction(nameof(Index));
    }
    /*
    // POST: Companies/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        if (_context.Companies == null)
        {
            return Problem("Entity set 'ApplicationDbContext.Companies'  is null.");
        }
        var company = await _context.Companies.FindAsync(id);
        if (company != null)
        {
            _context.Companies.Remove(company);
        }

        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private bool CompanyExists(int id)
    {
        return (_context.Companies?.Any(e => e.CompanyId == id)).GetValueOrDefault();
    }
    */
}
