using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.ModelView
{
    public class CodeTestViewModel
    {
        public int CodeTestId { get; set; }
        public string Question { get; set; }
        public string FunctionDescription { get; set; }

        public string SampleInput { get; set; }
        public string SampleOutput { get; set; }
        public string Example { get; set; }
        public string ExampleReturn { get; set; }

        public string Stdin1 { get; set; }
        public string Stdin1Return { get; set; }
        public string Stdin2 { get; set; }
        public string Stdin2Return { get; set; }
        public string Explanation { get; set; }
       
        public string CodeSnippetsDescription { get; set; }
        public string LANGUAGEVERSIONS { get; set; }

        public int TestId { get; set; }
        public string TestName { get; set; }
    }
}
