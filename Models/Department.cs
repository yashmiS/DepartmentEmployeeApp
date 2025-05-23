using System.ComponentModel.DataAnnotations;

namespace DepartmentEmployeeApp.Models
{
    public class Department
    {
        public int DepartmentId { get; set; }

        [Required]
        [StringLength(10)]
        public string DepartmentCode { get; set; }

        [Required]
        [StringLength(100)]
        public string DepartmentName { get; set; }
    }
}
