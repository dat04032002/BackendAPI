using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Model
{
    public class UserTestDetail
    {
        public int UserTestDetailId { get; set; }
        public int STT { get;set; }
        public int TestID { get; set; }
        public int TypeTest { get; set;}
        public string CorrectAnswer { get; set; }
    }
}
