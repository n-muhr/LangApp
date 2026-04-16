using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LangApp.Core.Models
{
    public class Word
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();
        [Required]
        [MaxLength(256)]
        public required string SourceText { get; set; }
        [Required]
        [MaxLength(256)]
        public required string TargetText { get; set; }
        public ICollection<ReviewHistory> ReviewHistories { get; set; } = new List<ReviewHistory>();

    }
}
