using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Notes.Web.Models.ReferenceVMs
{
    public class NewReference
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string Tags { get; set; }
        public IFormFile File { get; set; }
    }
}
