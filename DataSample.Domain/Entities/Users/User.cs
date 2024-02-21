using DataSample.Domain.Entities.Commons;
using DataSample.Domain.Entities.Fainances;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataSample.Domain.Entities.Users
{
    public class User : BaseEntity
    {
       
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public bool IsActive { get; set; }
        public List<UserInRole> UserInRoles { get; set; }
        public List<Cheque> Cheques { get; set; }      
    }
}
