using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HospitalProject.Models
{
    public class Doctor
    {
        [Key]
        public int DoctorId { get; set; }

        [Required(ErrorMessage = "Name is required")]
        [StringLength(20,MinimumLength =3, ErrorMessage = "Name must be between 3 and 20 characters")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Age is required")]
        [Range(25, 70, ErrorMessage = "Age must be between 25 and 70")]
        public int Age { get; set; }

        [Required(ErrorMessage = "Email is required")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Phone number is required")]
        [StringLength(11, MinimumLength = 11, ErrorMessage = "Phone number must be 11 digits")]
        public string PhoneNumber { get; set; }

        [Required(ErrorMessage = "Salary is required")]
        [Range(5000, 100000, ErrorMessage = "Salary must be between 5,000 and 100,000")]
        public float Salary { get; set; }

        [ForeignKey("Department")]
        public int DepartmentId { get; set; }
        public Department? Department { get; set; }

        public List<Patient>? Patients { get; set; } = new();

        public List<Treatment>? Treatments { get; set; } = new();
    }
}
