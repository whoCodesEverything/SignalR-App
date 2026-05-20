using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatApp.Core.Entities
{
    public  class BaseEntity : IEntity
    {
        public Guid ID { get; set; }

        //public bool? IsSuccess { get; set; }

       // public DateTime? CreatedDate { get; set; }
    }
}
