using System;
using System.Windows.Documents;

namespace AceGrading
{
    public class Essay : Question, QuestionInterface
    {
        public Essay(Test _ParentTest)
        {
            this.ParentTest = _ParentTest;
            this.TestSection = this.ParentTest.RequiredSection;
            Answer = null;
        }

        //Public Attributes
        public string Answer
        {
            get; set;
        }

        //Private Methods
        string QuestionInterface.Question_Type()
        {
            return "Essay";
        }
    }
}
