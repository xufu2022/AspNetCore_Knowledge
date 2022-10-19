using BethanysPieShopHRM.Shared.Domain;
using Microsoft.AspNetCore.Components;

namespace BethanysPieShopHRM.App.Components;

public partial class QuickViewPopup
{
    private Employee? _employee;
    protected override void OnParametersSet()
    {
        _employee = Employee;

    }

    [Parameter]
    public Employee? Employee { get; set; }


    public void Close()
    {
        _employee = null;
    }
}