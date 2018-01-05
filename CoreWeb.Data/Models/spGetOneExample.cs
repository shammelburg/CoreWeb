using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace CoreWeb.Data.Models
{
    public class spGetOneExample
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime? DOB { get; set; }
        public bool Active { get; set; }
    }
}
