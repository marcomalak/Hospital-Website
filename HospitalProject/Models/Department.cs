using System.ComponentModel.DataAnnotations;

namespace HospitalProject.Models
{
    public class Department
    {
        [Key]
        public int DepartmentId { get; set; }

        [Required(ErrorMessage = "Name is required")]
        [StringLength(20, MinimumLength = 3, ErrorMessage = "Name must be between 3 and 20 characters")]
        public string Name { get; set; }
        public string? Description { get; set; } = "No Description Found";

        public List<Doctor>? Doctors { get; set; } = new List<Doctor>();
    }
}
