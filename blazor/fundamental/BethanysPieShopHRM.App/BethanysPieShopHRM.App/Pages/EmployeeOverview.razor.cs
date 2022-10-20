using BethanysPieShopHRM.App.Models;
using BethanysPieShopHRM.App.Services;
using BethanysPieShopHRM.Shared.Domain;
using Microsoft.AspNetCore.Components;

namespace BethanysPieShopHRM.App.Pages;

public partial class EmployeeOverview
{
    [Inject]
    public IEmployeeDataService? EmployeeDataService { get; set; }
    public List<Employee> Employees { get; set; } = default!;
    private Employee? _selectedEmployee;
    private string Title = "Employee overview";
    private string Description = "employee overview";
    protected async override Task OnInitializedAsync()
    {
        //Employees = MockDataService.Employees;
        Employees = (await EmployeeDataService.GetAllEmployees()).ToList();
    }

    public void ShowQuickViewPopup(Employee selectedEmployee)
    {
        _selectedEmployee = selectedEmployee;
    }
}