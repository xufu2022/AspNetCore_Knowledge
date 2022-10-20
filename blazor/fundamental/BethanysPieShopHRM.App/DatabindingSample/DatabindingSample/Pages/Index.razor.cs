using BethanysPieShopHRM.Shared.Domain;

namespace DatabindingSample.Pages
{
    public partial class Index
    {
        public Employee Employee { get; set; } = default!;
        protected override Task OnInitializedAsync()
        {
            Employee = new Employee
            {
                FirstName = "Bethany",
                LastName = "Smith",
            };

            return base.OnInitializedAsync();
        }

        public void Button_Click()
        {
            Employee.FirstName = "Gill";

        }

        public void Test()
        {

        }
    }
}
