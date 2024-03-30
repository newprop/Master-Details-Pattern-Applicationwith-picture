using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace WebApplication2.Models
{
    
    public class StudentExamMarks
    {
        [Key]
        public int ExamMarksId { get; set; }
        public int SubjectId { get; set; }
        public int ObtainedNumber { get; set; }
        [Column(TypeName = "datetime2")]

        [DataType(DataType.Date)]
        [DisplayName("Start Date")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]

        public DateTime StartDate { get; set; }



        [ForeignKey("Students")]
        public int? StudentId { get; set; }

        public Subject? Subject { get; set; }
        public Students? Students { get; set; }

    }
}