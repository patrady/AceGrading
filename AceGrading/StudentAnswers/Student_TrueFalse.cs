namespace AceGrading
{
    public class Student_TrueFalse : Student_Answer
    {
        public bool Answer { get; set; }

        public string Answer_If_False { get; set; }

        public Student_TrueFalse()
        {
            Answer_If_False = null;
        }

    }
}
