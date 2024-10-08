using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Model
{
    public class Test
    {
        public int TestId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int Time { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int DanhMucChiTietId { get; set; }
        public DanhMucChiTiet DanhMucChiTiet { get; set; }

        public ICollection<MultipleChoiceTest> MultipleChoiceTests { get; set; }

    }
}
