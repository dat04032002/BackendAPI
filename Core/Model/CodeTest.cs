using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Model
{
    public class CodeTest
    {
        [Key]
        public int CodeTestId { get; set; }
        public string Question { get; set; }
        public string CorrectAnswer { get; set; }
        public string CodeSnippetsDescription { get; set; }
        public string LANGUAGEVERSIONS { get; set; }

        public int TestId { get; set; }
        public Test Test { get; set; }
    }
}
