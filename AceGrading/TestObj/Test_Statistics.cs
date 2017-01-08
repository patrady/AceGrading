using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace AceGrading
{
    public class Test_Statistics : INotifyPropertyChanged
    {
        //Constructors
        public Test_Statistics()
        {
            Random rand = new Random();
            //Used for debugging
            this.Students = new List<Student>()
            {
                new Student() {TestAnswers = new List<Student_Answer>() {new Student_Answer() { Points_Received = rand.Next(1, 100)} } },
                new Student() {TestAnswers = new List<Student_Answer>() {new Student_Answer() { Points_Received = rand.Next(1, 100) } } },
                new Student() {TestAnswers = new List<Student_Answer>() {new Student_Answer() { Points_Received = rand.Next(1, 100) } } },
                new Student() {TestAnswers = new List<Student_Answer>() {new Student_Answer() { Points_Received = rand.Next(1, 100) } } }
            };
            this.CalculateScores();
        }
        public Test_Statistics(int Num_Questions, List<Student> _Students)
        {
            Question_Correctly_Answered = new int[Num_Questions];
            Question_Attempted = new int[Num_Questions];
            Most_Missed_Problem = new List<int>();
            Students = _Students;
            this.Highest_Test_Score = double.NegativeInfinity;
            this.Lowest_Test_Score = double.PositiveInfinity;
            this.Average_Test_Score = 0;
            this.NumberOfScores = 0;

            Zero_Out_Arrays();
        }

        //Public Attributes
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

        //Private Attributes
        private int NumberOfScores { get; set; }

        //Public Methods
        public void CalculateScores()
        {
            this.CalculateHighestScore();
            this.CalculateAverageScore();
            this.CalculateLowestScore();
        }
        /// <summary>
        /// Includes a score in the statistics.
        /// </summary>
        /// <param name="score">New score to be included.</param>
        public void AddScore(double score)
        {
            //Check if score is higher than the highet
            if (score > this.Highest_Test_Score)
                this.Highest_Test_Score = score;

            //Check if the score is lower than the lowest
            if (score < this.Lowest_Test_Score)
                this.Lowest_Test_Score = score;

            //Change the average to include the new score
            if (this.NumberOfScores == 0)
                this.Average_Test_Score = score;
            else
                this.Average_Test_Score = ((this.Average_Test_Score) * (this.NumberOfScores) + score) / (this.NumberOfScores + 1);
            this.NumberOfScores++;
        }
        /// Includes a score in the statistics by first calculating the score the student made.
        /// </summary>
        /// <param name="student">The student to have their score calculated.</param>
        /// <returns>Returns the score the student received.</returns>
        public double AddScore(Student student)
        {
            double tempScore = 0;
            tempScore = 0;
            tempScore += student.Bonus_Points;
            tempScore += student.TestAnswers.Sum(x => x.Points_Received);

            //Add the score into the statistics
            this.AddScore(student.TestScore);

            return tempScore;
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
                foreach (Student_Answer answer in student.TestAnswers)
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
                foreach (Student_Answer answer in student.TestAnswers)
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
                foreach (Student_Answer answer in student.TestAnswers)
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
