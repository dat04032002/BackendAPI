using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Model
{
    public class UserTest
    {
        public int UserTestId { get; set; }
        public int UserID { get; set; }
        public int TestID { get; set; }
        public float PointMultipleChoiceTest { get; set; }
        public float PointMultipleCodeTest { get; set; }
        public DateTime TestDate { get; set; }

        public int UserTestDataID { get; set; }
    }
}
