using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Model
{
    public class DanhMuc
    {
        public  int DanhMucId { get; set; }
        public  string Name { get; set; }
        public ICollection<DanhMucChiTiet> ChiTiets { get; set;}
    }
}
