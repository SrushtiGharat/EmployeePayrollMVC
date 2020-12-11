using EmpManagementIdentityProMVC.Models;
using EmpManagementIdentityProMVC.Models.Common;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EmpManagementIdentityProMVC.Controllers
{
    [Authorize]
    public class EmployeeController : Controller
    {
        
        public ApplicationDbContext db = new ApplicationDbContext();
        // GET: Employee
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult RegisterEmployee(RegisterEmpRequestModel employee)
        {
            bool result = false;
            if (ModelState.IsValid)
            {
                result = this.RegisterEmployeeService(employee);
            }
            ModelState.Clear();

            if (result == true)
            {
                return RedirectToAction("EmployeeList");
            }
            return View("Index");
        }

        public ActionResult EmployeeList()
        {
            List<EmployeeDetailModel> list = GetAllEmployee();
            return View(list);
        }

        public ActionResult Delete(int id)
        {
            bool result = DeleteEmployee(id);
            if (result == true)
            {
                List<EmployeeDetailModel> list = GetAllEmployee();
                return View("EmployeeList", list);
            }
            else
            {
                return View("Error");
            }
        }

        public ActionResult Edit(EmployeeDetailModel item)
        {
            RegisterEmpRequestModel emp = new RegisterEmpRequestModel();
            emp.EmpId = item.EmpId;
            emp.Name = item.Name;
            emp.Gender = item.Gender;
            emp.Department = item.Department;
            emp.SalaryId = item.SalaryId.ToString();
            emp.StartDate = item.StartDate;
            emp.Description = item.Description;
            
            return View(emp);
        }
        public ActionResult EditEmployee(RegisterEmpRequestModel employee)
        {
            bool result = EditEmployeeService(employee);
            if (result == true)
            {
                List<EmployeeDetailModel> list = GetAllEmployee();
                return View("EmployeeList", list);
            }
            else
            {
                return View("Error");
            }
        }
        public bool RegisterEmployeeService(RegisterEmpRequestModel employee)
        {
            try
            {
                Employee validEmployee = db.Employees.Where(x => x.Name == employee.Name && x.Gender == employee.Gender).FirstOrDefault();
                if (validEmployee == null)
                {
                    int departmentId = db.Departments.Where(x => x.DeptName == employee.Department).Select(x => x.DeptId).FirstOrDefault();
                    Employee newEmployee = new Employee()
                    {
                        Name = employee.Name,
                        Gender = employee.Gender,
                        DepartmentId = departmentId,
                        SalaryId = Convert.ToInt32(employee.SalaryId),
                        StartDate = employee.StartDate,
                        Description = employee.Description
                    };
                    Employee returnData = db.Employees.Add(newEmployee);
                }
                int result = db.SaveChanges();
                if (result > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public List<EmployeeDetailModel> GetAllEmployee()
        {
            try
            {
                List<EmployeeDetailModel> list = (from e in db.Employees
                                                  join d in db.Departments on e.DepartmentId equals d.DeptId
                                                  join s in db.Salaries on e.SalaryId equals s.SalaryId
                                                  select new EmployeeDetailModel
                                                  {
                                                      EmpId = e.EmpId,
                                                      Name = e.Name,
                                                      Gender = e.Gender,
                                                      DepartmentId = d.DeptId,
                                                      Department = d.DeptName,
                                                      SalaryId = s.SalaryId,
                                                      Amount = s.Amount,
                                                      StartDate = e.StartDate,
                                                      Description = e.Description
                                                  }).ToList<EmployeeDetailModel>();

                return list;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public bool DeleteEmployee(int id)
        {
            try
            {
                Employee employee = db.Employees.Find(id);
                if (employee == null)
                {
                    return false;
                }
                db.Employees.Remove(employee);
                int result = db.SaveChanges();
                if (result > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        public bool EditEmployeeService(RegisterEmpRequestModel employee)
        {
            try
            {
                int departmentId = db.Departments.Where(x => x.DeptName == employee.Department).Select(x => x.DeptId).FirstOrDefault();
               
                Employee emp = db.Employees.Find(employee.EmpId);
                emp.Name = employee.Name;
                emp.SalaryId = Convert.ToInt32(employee.SalaryId);
                emp.StartDate = employee.StartDate;
                emp.Description = employee.Description;
                emp.Gender = employee.Gender;
                emp.DepartmentId = departmentId;

                //db.Entry(emp).State = EntityState.Modified;
                
                int result = db.SaveChanges();
                if (result > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }
    }
}
//int id,string name,string gender,string department,int salaryId,DateTime startDate,string desc