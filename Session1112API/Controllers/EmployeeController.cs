using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Session1112API.Models;
using Session1112API.Services;
using System.Data;

namespace Session1112API.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	[Authorize(Roles = "admin")]
	public class EmployeeController : ControllerBase
	{
		private readonly IEmployeeService employeeService;

		public EmployeeController(IEmployeeService _employeeService)
		{
			employeeService = _employeeService;
		}

		// HTTPget
		// 192.168.1.200/Demo/api/Employee

		// HTTPPost
		// 192.168.1.200/Demo/api/Employee/xyz

		[HttpPost]
		[Route("xyz")]
		public void Insert(EmployeeDTO employeeDTO)
		{
			//employeeService.Insert(employeeDTO);
		}


		[HttpPost]
		[Route("abc")]
		public void Update(EmployeeDTO employeeDTO)
		{
			//employeeService.Insert(employeeDTO);
		}

		[HttpGet]
		public List<EmployeeDTO> Load(int Id)
		{
			List<EmployeeDTO> employees = new List<EmployeeDTO>();
			employees.Add(new EmployeeDTO()
			{
				Id = 1,
				Name = "Ahmad"
			});

			return employees;
		}
	}
}
