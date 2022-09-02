using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Model {
    public class Distrito {
        [Key]
        public int idDistrito { get; set; }
        [Required]
        public string nombre { get; set; }
    }
}
