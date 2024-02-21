using DataSample.Domain.Entities.Commons;
using DataSample.Domain.Entities.Users;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataSample.Domain.Entities.Fainances
{
    public class Cheque : BaseEntity
    {
        [Required]
        public string Name { get; set; }
        public DateTime DueDate { get; set; }

        [StringLength(16, ErrorMessage = "The SayadNo value cannot exceed 16 characters. ")]
        public string SayadNo { get; set; }

        [StringLength(26, ErrorMessage = "The ShebaNoCreditor value cannot exceed 16 characters. ")]
        public string ShebaNoCreditor { get; set; }

        [StringLength(26, ErrorMessage = "The ShebaNoDebtor value cannot exceed 16 characters. ")]
        public string ShebaNoDebtor { get; set; }

        public int UserId { get; set; }
        public User User { get; set; }

    }
}
