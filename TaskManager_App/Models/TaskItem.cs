using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TaskManager_App.Models
{
    public class TaskItem
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Title { get; set; }

        [MaxLength(500)]
        public string Description { get; set; }

        public DateTime? DueDate { get; set; }

        [Required]
        [RegularExpression("Low|Medium|High", ErrorMessage = "Priority must be Low, Medium, or High")]
        public string Priority { get; set; }

        [Required]
        [RegularExpression("Pending|In Progress|Completed", ErrorMessage = "Status must be Pending, In Progress, or Completed")]
        public string Status { get; set; }

        [Required]
        [RegularExpression("Work|Personal|Study", ErrorMessage = "Category must be Work, Personal, or Study")]
        public string Category { get; set; }
    }
}