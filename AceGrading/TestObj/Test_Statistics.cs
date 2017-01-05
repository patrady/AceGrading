using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace AceGrading
{
    public class Test_Statistics : INotifyPropertyChanged
    {
        public Test_Statistics()
        {
            Random rand = new Random();
            //Used for debugging
            this.Students = new List<Student>()
            {
                new Student() {Student_Test_Answers = new List<Student_Answer>() {new Student_Answer() { Points_Received = rand.Next(1, 100)} } },
                new Student() {Student_Test_Answers = new List<Student_Answer>() {new Student_Answer() { Points_Received = rand.Next(1, 100) } } },
                new Student() {Student_Test_Answers = new List<Student_Answer>() {new Student_Answer() { Points_Received = rand.Next(1, 100) } } },
                new Student() {Student_Test_Answers = new List<Student_Answer>() {new Student_Answer() { Points_Received = rand.Next(1, 100) } } }
            };
            this.CalculateScores();
        }

        public Test_Statistics(int Num_Questions, List<Student> _Students)
        {
            Question_Correctly_Answered = new int[Num_Questions];
            Question_Attempted = new int[Num_Questions];
            Most_Missed_Problem = new List<int>();
            Students = _Students;

            Zero_Out_Arrays();
        }

        public int[] Question_Correctly_Answered { get; set; }

        public int[] Question_Attempted { get; set; }

        public double Highest_Test_Score
        {
            get { return _HighestScore; }
            set
            {
                _HighestScore = value;
                OnPropertyChanged("Highest_Test_Score");
            }
        }
        public double Average_Test_Score
        {
            get { return _AverageScore; }
            set
            {
                _AverageScore = value;
                OnPropertyChanged("Average_Test_Score");
            }
        }
        public double Lowest_Test_Score
        {
            get { return _LowestScore; }
            set
            {
                _LowestScore = value;
                OnPropertyChanged("Lowest_Test_Score");
            }

        }

        public List<int> Most_Missed_Problem { get; set; }

        public List<Student> Students { get; set; }

        //Public Methods
        public void CalculateScores()
        {
            this.CalculateHighestScore();
            this.CalculateAverageScore();
            this.CalculateLowestScore();
        }

        //Private Methods
        private void CalculateHighestScore()
        {
            double highest_score = double.NegativeInfinity;
            double student_score;

            foreach (Student student in Students)
            {
                student_score = 0;
                student_score += student.Bonus_Points;

                //Tallies up the student's test score
                foreach (Student_Answer answer in student.Student_Test_Answers)
                    student_score += answer.Points_Received;

                //Compares for the highest score
                if (student_score > highest_score)
                    highest_score = student_score;
            }

            this.Highest_Test_Score = highest_score;
        }
        private void CalculateAverageScore()
        {
            double highest_score = double.NegativeInfinity;
            double student_score, total_scores = 0;

            foreach (Student student in Students)
            {
                student_score = 0;
                student_score += student.Bonus_Points;

                //Tallies up the student's test score
                foreach (Student_Answer answer in student.Student_Test_Answers)
                    student_score += answer.Points_Received;

                //Compares for the highest score
                if (student_score > highest_score)
                    highest_score = student_score;

                //Used for the average score
                total_scores += student_score;
            }

            if (Students.Count.Equals(0))
                this.Average_Test_Score = 0;
            this.Average_Test_Score = (total_scores / Students.Count);
        }
        private void CalculateLowestScore()
        {
            double lowest_score = double.PositiveInfinity;
            double student_score;

            foreach (Student student in Students)
            {
                student_score = 0;
                student_score += student.Bonus_Points;

                //Tallies up the student's test score
                foreach (Student_Answer answer in student.Student_Test_Answers)
                    student_score += answer.Points_Received;

                //Compares for the highest score
                if (student_score < lowest_score)
                    lowest_score = student_score;
            }

            this.Lowest_Test_Score = lowest_score;
        }
        private void Zero_Out_Arrays()
        {
            for (int i = 0; i < Question_Correctly_Answered.Length; i++)
            {
                Question_Correctly_Answered[i] = 0;
                Question_Attempted[i] = 0;
            }
        }

        //Private variables
        private double _HighestScore, _LowestScore, _AverageScore;

        //INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        protected bool SetField<T>(ref T field, T value, string propertyName)
        {
            if (EqualityComparer<T>.Default.Equals(field, value)) return false;
            field = value;
            OnPropertyChanged(propertyName);
            return true;
        }

    }
}
