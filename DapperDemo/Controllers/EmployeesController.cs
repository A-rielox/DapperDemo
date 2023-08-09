using DapperDemo.Models;
using DapperDemo.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace DapperDemo.Controllers;

public class EmployeesController : Controller
{
    private readonly ICompanyRepository _compRepo;
    private readonly IEmployeeRepository _empRepo;

    [BindProperty]
    public Employee Employee { get; set; }

    public EmployeesController(ICompanyRepository compRepo, IEmployeeRepository empRepo)
    {
        _compRepo = compRepo;
        _empRepo = empRepo;
    }



    //////////////////////////////////////////////
    /////////////////////////////////////////////////
    public async Task<IActionResult> Index()
    {
        return View(_empRepo.GetAll());
    }




    //////////////////////////////////////////////
    /////////////////////////////////////////////////
    public IActionResult Create()
    {
        IEnumerable<SelectListItem> companyList = _compRepo.GetAll().Select(i => new SelectListItem
        {
            Text = i.Name,
            Value = i.CompanyId.ToString()
        });

        ViewBag.CompanyList = companyList;
        
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [ActionName("Create")]
    public async Task<IActionResult> CreatePOST()
    {
        ModelState.Remove("Company");
        if (ModelState.IsValid)
        {
            _empRepo.Add(Employee);
            return RedirectToAction(nameof(Index));
        }
        return View(Employee);
    }


    //////////////////////////////////////////////
    /////////////////////////////////////////////////
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        Employee = _empRepo.Find(id.GetValueOrDefault());

        IEnumerable<SelectListItem> companyList = _compRepo.GetAll().Select(i => new SelectListItem
        {
            Text = i.Name,
            Value = i.CompanyId.ToString()
        });

        ViewBag.CompanyList = companyList;

        if (Employee == null)
        {
            return NotFound();
        }

        return View(Employee);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id)
    {
        if (id != Employee.EmployeeId)
        {
            return NotFound();
        }

        ModelState.Remove("Company");
        if (ModelState.IsValid)
        {
            _empRepo.Update(Employee);

            return RedirectToAction(nameof(Index));
        }

        // x si NO es valido
        return View(Employee);
    }


    //////////////////////////////////////////////
    /////////////////////////////////////////////////
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        _empRepo.Remove(id.GetValueOrDefault());
        return RedirectToAction(nameof(Index));
    }
}
