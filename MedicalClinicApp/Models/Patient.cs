using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MedicalClinicApp.Models
{
    public class Patient
    {
        [Key]
        public int Id { get; set; }
        [MaxLength(20)]
        public string FirstName { get; set; }
        [MaxLength(30)]
        public string LastName { get; set; }
        [MaxLength(11)]
        public string Pesel { get; set; }
        public Address Address { get; set; }
    }
}