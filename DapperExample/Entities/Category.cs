using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DapperExample.Entities
{
    [Dapper.Contrib.Extensions.Table("Categories")]
    public class Category
    {
       
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
