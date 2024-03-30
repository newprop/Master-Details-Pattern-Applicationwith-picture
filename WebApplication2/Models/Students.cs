using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace WebApplication2.Models
{
    public class Students
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }


        [Required]
        [StringLength(50)]
        public string Name { get; set; }

        [DataType(DataType.MultilineText)]
        [StringLength(250)]
        public string Address { get; set; }

        public string? ContactNo { get; set; }
        public string? StudentImage { get; set; }
        [NotMapped]
        public IFormFile? ImageUpload { get; set; }


        public int Class { get; set; }


        public bool Regular { get; set; }







        public virtual IList<StudentExamMarks> StudentMarks { get; set; }

        public Students()
        {
            StudentMarks = new List<StudentExamMarks>();

        }
    }
}
