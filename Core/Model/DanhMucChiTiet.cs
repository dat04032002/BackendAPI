using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Model
{
    public class DanhMucChiTiet
    {
        public int DanhMucChiTietId { get; set; }
        
        public string Name { get; set; }
        public int DanhMucId { get; set; }
        public DanhMuc DanhMuc { get; set; }
        public ICollection<Test> Tests { get; set; }
    }
}
