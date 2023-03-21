using Microsoft.AspNetCore.Authorization;
using Session1112API.data;
using Session1112API.Models;

namespace Session1112API.Services
{
	
	public class EmployeeService:IEmployeeService
	{
		private readonly HRContext context;

		public EmployeeService(HRContext _context)
		{
			context = _context;
		}
		public void Insert(EmployeeDTO employeeDTO)
		{
			Employee employee = new Employee()
			{
				Name=employeeDTO.Name
			};

			context.employees.Add(employee);
			context.SaveChanges();


		}
	}
}
