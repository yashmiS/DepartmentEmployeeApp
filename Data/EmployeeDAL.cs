using DepartmentEmployeeApp.Models;
using System.Collections.Generic;
using System.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace DepartmentEmployeeApp.Data
{
    public class EmployeeDAL
    {
        private readonly DbHelper _db;

        public EmployeeDAL(IConfiguration config)
        {
            _db = new DbHelper(config);
        }

        public List<Employee> GetAll()
        {
            var list = new List<Employee>();

            using (var conn = _db.GetConnection())
            {
                var cmd = new SqlCommand(@"
                    SELECT e.*, d.DepartmentName 
                    FROM Employees e 
                    JOIN Departments d ON e.DepartmentId = d.DepartmentId", conn);
                conn.Open();
                var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    list.Add(new Employee
                    {
                        EmployeeId = (int)reader["EmployeeId"],
                        FirstName = reader["FirstName"].ToString(),
                        LastName = reader["LastName"].ToString(),
                        Email = reader["Email"].ToString(),
                        DateOfBirth = (System.DateTime)reader["DateOfBirth"],
                        Salary = (decimal)reader["Salary"],
                        DepartmentId = (int)reader["DepartmentId"],
                        DepartmentName = reader["DepartmentName"].ToString()
                    });
                }
            }

            return list;
        }

        public Employee GetById(int id)
        {
            Employee emp = null;

            using (var conn = _db.GetConnection())
            {
                var cmd = new SqlCommand("SELECT * FROM Employees WHERE EmployeeId = @id", conn);
                cmd.Parameters.AddWithValue("@id", id);
                conn.Open();
                var reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    emp = new Employee
                    {
                        EmployeeId = (int)reader["EmployeeId"],
                        FirstName = reader["FirstName"].ToString(),
                        LastName = reader["LastName"].ToString(),
                        Email = reader["Email"].ToString(),
                        DateOfBirth = (System.DateTime)reader["DateOfBirth"],
                        Salary = (decimal)reader["Salary"],
                        DepartmentId = (int)reader["DepartmentId"]
                    };
                }
            }

            return emp;
        }

       public void Insert(Employee emp)
        {
        try
        {
            using (var conn = _db.GetConnection())
            {

            var cmd = new SqlCommand(@"
                INSERT INTO Employees 
                (FirstName, LastName, Email, DateOfBirth, Salary, DepartmentId)
                VALUES 
                (@fn, @ln, @em, @dob, @sal, @deptId)", conn);

            cmd.Parameters.AddWithValue("@fn", emp.FirstName);
            cmd.Parameters.AddWithValue("@ln", emp.LastName);
            cmd.Parameters.AddWithValue("@em", emp.Email);
            cmd.Parameters.AddWithValue("@dob", emp.DateOfBirth);
            cmd.Parameters.AddWithValue("@sal", emp.Salary);
            cmd.Parameters.AddWithValue("@deptId", emp.DepartmentId);

            conn.Open();
            int result = cmd.ExecuteNonQuery();

            SqlCommand checkDb = new SqlCommand("SELECT DB_NAME()", conn);
            var dbName = checkDb.ExecuteScalar();

            if (result == 0)
                throw new Exception("Insert failed: No rows affected.");
        }
        }
        catch (Exception ex)
        {
            throw new Exception("Insert failed: " + ex.Message);
        }
        }



        public void Update(Employee emp)
        {
    
            using (var conn = _db.GetConnection())
            {
                var cmd = new SqlCommand(@"
                UPDATE Employees 
                SET FirstName = @fn, LastName = @ln, Email = @em, DateOfBirth = @dob, 
                Salary = @sal, DepartmentId = @deptId
                WHERE EmployeeId = @id", conn);

            cmd.Parameters.AddWithValue("@fn", emp.FirstName);
            cmd.Parameters.AddWithValue("@ln", emp.LastName);
            cmd.Parameters.AddWithValue("@em", emp.Email);
            cmd.Parameters.AddWithValue("@dob", emp.DateOfBirth);
            cmd.Parameters.AddWithValue("@sal", emp.Salary);
            cmd.Parameters.AddWithValue("@deptId", emp.DepartmentId);
            cmd.Parameters.AddWithValue("@id", emp.EmployeeId);

         conn.Open();
         int rows = cmd.ExecuteNonQuery();
        }
        }

        public void Delete(int id)
        {
            using (var conn = _db.GetConnection())
            {
                var cmd = new SqlCommand("DELETE FROM Employees WHERE EmployeeId = @id", conn);
                cmd.Parameters.AddWithValue("@id", id);
                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }
    }
}
