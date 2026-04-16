using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LangApp.Core.Models
{
    public class ReviewHistory
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public Guid WordId { get; set; }
        public Word Word { get; set; }
        public Guid UserId {  get; set; }
        public User User { get; set; }
        [Column(TypeName = "timestamp")]
        public DateTime LastReviewedDate { get; set; }
        [Required]
        public int LastPerformace { get; set; }
        public double EasinessFactor { get; set; }
        public int IntervalDays { get; set; }
    }
}
