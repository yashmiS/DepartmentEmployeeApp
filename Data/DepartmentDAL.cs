using DepartmentEmployeeApp.Models;
using System.Collections.Generic;
using System.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace DepartmentEmployeeApp.Data
{
    public class DepartmentDAL
    {
        private readonly DbHelper _db;

        public DepartmentDAL(IConfiguration config)
        {
            _db = new DbHelper(config);
        }

        public List<Department> GetAll()
        {
            var list = new List<Department>();

            using (var conn = _db.GetConnection())
            {
                var cmd = new SqlCommand("SELECT * FROM Departments", conn);
                conn.Open();
                var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    list.Add(new Department
                    {
                        DepartmentId = (int)reader["DepartmentId"],
                        DepartmentCode = reader["DepartmentCode"].ToString(),
                        DepartmentName = reader["DepartmentName"].ToString()
                    });
                }
            }

            return list;
        }

        public Department GetById(int id)
        {
            Department dept = null;

            using (var conn = _db.GetConnection())
            {
                var cmd = new SqlCommand("SELECT * FROM Departments WHERE DepartmentId = @id", conn);
                cmd.Parameters.AddWithValue("@id", id);
                conn.Open();
                var reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    dept = new Department
                    {
                        DepartmentId = (int)reader["DepartmentId"],
                        DepartmentCode = reader["DepartmentCode"].ToString(),
                        DepartmentName = reader["DepartmentName"].ToString()
                    };
                }
            }

            return dept;
        }

        public void Insert(Department dept)
        {
            using (var conn = _db.GetConnection())
            {
                var cmd = new SqlCommand("INSERT INTO Departments (DepartmentCode, DepartmentName) VALUES (@code, @name)", conn);
                cmd.Parameters.AddWithValue("@code", dept.DepartmentCode);
                cmd.Parameters.AddWithValue("@name", dept.DepartmentName);
                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }

        public void Update(Department dept)
        {
            using (var conn = _db.GetConnection())
            {
                var cmd = new SqlCommand("UPDATE Departments SET DepartmentCode = @code, DepartmentName = @name WHERE DepartmentId = @id", conn);
                cmd.Parameters.AddWithValue("@code", dept.DepartmentCode);
                cmd.Parameters.AddWithValue("@name", dept.DepartmentName);
                cmd.Parameters.AddWithValue("@id", dept.DepartmentId);
                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }

        public void Delete(int id)
        {
            using (var conn = _db.GetConnection())
            {
                var cmd = new SqlCommand("DELETE FROM Departments WHERE DepartmentId = @id", conn);
                cmd.Parameters.AddWithValue("@id", id);
                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }

        public bool HasEmployees(int departmentId)
        {
            using (var conn = _db.GetConnection())
            {
                var cmd = new SqlCommand("SELECT COUNT(*) FROM Employees WHERE DepartmentId = @deptId", conn);
                cmd.Parameters.AddWithValue("@deptId", departmentId);
                conn.Open();
                int count = (int)cmd.ExecuteScalar();
                return count > 0;
            }
        }
        public Department GetByCode(string code)
        {
            using (var conn = _db.GetConnection())
            {
                var cmd = new SqlCommand("SELECT * FROM Departments WHERE DepartmentCode = @code", conn);
                cmd.Parameters.AddWithValue("@code", code);
                conn.Open();
                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return new Department
                        {
                            DepartmentId = Convert.ToInt32(reader["DepartmentId"]),
                            DepartmentName = reader["DepartmentName"].ToString(),
                            DepartmentCode = reader["DepartmentCode"].ToString()
                        };
                    }
                }
            }
            return null;
        }

    }
}
