using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TestMvcProjectGFL.Models
{
    public class Catalog
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int ParentId { get; set; }
    }
}