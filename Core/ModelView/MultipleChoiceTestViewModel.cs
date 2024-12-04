using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.ModelView
{
    public class MultipleChoiceTestViewModel
    {
        public int MultipleChoiceTestId { get; set; }
        public string Question { get; set; }
        public string PlanA { get; set; }
        public string PlanB { get; set; }
        public string PlanC { get; set; }
        public string PlanD { get; set; }
        public string CorrectAnswer { get; set; }
       
        public int TestId { get; set; }
        public string TestName { get; set; }
    }
}
