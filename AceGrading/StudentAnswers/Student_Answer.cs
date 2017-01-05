namespace AceGrading
{
    public class Student_Answer
    {
        public int Question_Number { get; set; }
        public object Server_ID { get; set; }
        public bool Has_Been_Answered { get; set; }
        public double Points_Received { get; set; }
        public string Comments { get; set; }
        public bool Has_Been_Graded { get; set; }
        public bool Teacher_Has_Graded { get; set; }
        public string Grading_Remarks { get; set; }

        public Student_Answer()
        {
            Question_Number = -1;
            Has_Been_Answered = false;
            Points_Received = 0;
            Comments = null;
            Has_Been_Graded = false;
            Teacher_Has_Graded = false;
            Grading_Remarks = null;
        }
    }
}
