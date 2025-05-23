using System.Data;
using System.Data.SqlClient;
using DepartmentEmployeeApp.Models;
using Microsoft.Extensions.Configuration;

namespace DepartmentEmployeeApp.Data
{
    public class EmployeeDAL
    {
        private readonly string _connectionString;

        public EmployeeDAL(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public List<Employee> GetAll()
        {
            var list = new List<Employee>();
            using var con = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand("SELECT e.*, d.DepartmentName FROM Employees e JOIN Departments d ON e.DepartmentId = d.DepartmentId", con);
            con.Open();
            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                list.Add(new Employee
                {
                    EmployeeId = (int)reader["EmployeeId"],
                    FirstName = reader["FirstName"].ToString(),
                    LastName = reader["LastName"].ToString(),
                    Email = reader["Email"].ToString(),
                    DateOfBirth = Convert.ToDateTime(reader["DateOfBirth"]),
                    Salary = Convert.ToDecimal(reader["Salary"]),
                    DepartmentId = (int)reader["DepartmentId"],
                    DepartmentName = reader["DepartmentName"].ToString()
                });
            }
            return list;
        }

        public Employee GetById(int id)
        {
            using var con = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand("SELECT * FROM Employees WHERE EmployeeId = @id", con);
            cmd.Parameters.AddWithValue("@id", id);
            con.Open();
            using var reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                return new Employee
                {
                    EmployeeId = (int)reader["EmployeeId"],
                    FirstName = reader["FirstName"].ToString(),
                    LastName = reader["LastName"].ToString(),
                    Email = reader["Email"].ToString(),
                    DateOfBirth = Convert.ToDateTime(reader["DateOfBirth"]),
                    Salary = Convert.ToDecimal(reader["Salary"]),
                    DepartmentId = (int)reader["DepartmentId"]
                };
            }
            return null;
        }

        public Employee GetByEmail(string email)
        {
            using var con = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand("SELECT * FROM Employees WHERE Email = @Email", con);
            cmd.Parameters.AddWithValue("@Email", email);
            con.Open();
            using var reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                return new Employee
                {
                    EmployeeId = (int)reader["EmployeeId"],
                    FirstName = reader["FirstName"].ToString(),
                    LastName = reader["LastName"].ToString(),
                    Email = reader["Email"].ToString(),
                    DateOfBirth = Convert.ToDateTime(reader["DateOfBirth"]),
                    Salary = Convert.ToDecimal(reader["Salary"]),
                    DepartmentId = (int)reader["DepartmentId"]
                };
            }
            return null;
        }

        public void Insert(Employee emp)
        {
            using var con = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand("INSERT INTO Employees (FirstName, LastName, Email, DateOfBirth, Salary, DepartmentId) VALUES (@FirstName, @LastName, @Email, @DateOfBirth, @Salary, @DepartmentId)", con);
            cmd.Parameters.AddWithValue("@FirstName", emp.FirstName);
            cmd.Parameters.AddWithValue("@LastName", emp.LastName);
            cmd.Parameters.AddWithValue("@Email", emp.Email);
            cmd.Parameters.AddWithValue("@DateOfBirth", emp.DateOfBirth);
            cmd.Parameters.AddWithValue("@Salary", emp.Salary);
            cmd.Parameters.AddWithValue("@DepartmentId", emp.DepartmentId);
            con.Open();
            if (emp.DateOfBirth < new DateTime(1753, 1, 1))
            {
                throw new Exception("Date of birth is not valid for SQL Server.");
            }
            cmd.ExecuteNonQuery();
        }

        public void Update(Employee emp)
        {
            using var con = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand("UPDATE Employees SET FirstName=@FirstName, LastName=@LastName, Email=@Email, DateOfBirth=@DateOfBirth, Salary=@Salary, DepartmentId=@DepartmentId WHERE EmployeeId=@EmployeeId", con);
            cmd.Parameters.AddWithValue("@FirstName", emp.FirstName);
            cmd.Parameters.AddWithValue("@LastName", emp.LastName);
            cmd.Parameters.AddWithValue("@Email", emp.Email);
            cmd.Parameters.AddWithValue("@DateOfBirth", emp.DateOfBirth);
            cmd.Parameters.AddWithValue("@Salary", emp.Salary);
            cmd.Parameters.AddWithValue("@DepartmentId", emp.DepartmentId);
            cmd.Parameters.AddWithValue("@EmployeeId", emp.EmployeeId);
            con.Open();
            cmd.ExecuteNonQuery();
        }

        public void Delete(int id)
        {
            using var con = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand("DELETE FROM Employees WHERE EmployeeId=@id", con);
            cmd.Parameters.AddWithValue("@id", id);
            con.Open();
            cmd.ExecuteNonQuery();
        }
    }
}
