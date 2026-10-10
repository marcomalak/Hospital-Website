using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HospitalProject.Models
{
    public enum Gender
    {
        Male,
        Female
    }
    public class Patient
    {
        [Key]
        public int ID { get; set; }

        [Required(ErrorMessage = "Name is required")]
        [StringLength(20, MinimumLength = 3, ErrorMessage = "Name must be between 3 and 20 characters")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Age is required")]
        [Range(0, 120, ErrorMessage = "Age must be between 0 and 120")]
        public int Age { get; set; }

        [Required(ErrorMessage = "Gender is required")]
        public Gender Gender { get; set; }

        public string Disease { get; set; } = "No Disease Found";

        public List<Doctor>? Doctors { get; set; } = new List<Doctor>();

        public List<Treatment>? Treatments { get; set; } = new List<Treatment>();
    }
}
