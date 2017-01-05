using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;

namespace AceGrading
{
    public class Test : INotifyPropertyChanged
    {
        public Test()
        {
            Answers = new ObservableCollection<Question>();
            Sections = new ObservableCollection<Section>() { new Section(this) { Section_Number = 0 } };
            TestInitials = new Initials();
            SelectedSection = new Section(this);
            OpenFile = new OpenFile_Command(this);
            Browse = new Browse_Command(this);
            AddMultipleChoice = new AddMultipleChoiceQuestion_Command(this);
            AddMatching = new AddMatchingQuestion_Command(this);
            AddTrueFalse = new AddTrueFalseQuestion_Command(this);
            AddShortAnswer = new AddShortAnswerQuestion_Command(this);
            AddEssay = new AddEssayQuestion_Command(this);
            AddSection = new AddTestSection_Command(this);
            DeleteSection = new DeleteTestSection_Command(this);
            Statistics = new Test_Statistics();
            TestName = "Test Name";
            Upload_File = null;
            Individual_Values = false;
            HasSections = false;
            Point_Worth = 0;
            Is_Graded = false;
            Has_Student_Answers = false;
            Upload_File_Name = null;
            Server_ID = null;
        }

        //Attributes
        public string TestName
        {
            get { return _Test_Name; }
            set
            {
                _Test_Name = value;
                _Initials = TestInitials.MakeInitials(_Test_Name);
                OnPropertyChanged("TestName");
                OnPropertyChanged("Initials");
            }
        }
        public ObservableCollection<Question> Answers { get; set; }
        public string Initials { get { return _Initials; } }
        public double Point_Worth
        {
            get { return _PointWorth; }
            set
            {
                if (value != _PointWorth)
                {
                    _PointWorth = value;
                    OnPropertyChanged("Point_Worth");
                    UpdatePointsRemaining();
                }
            }
        }
        public double PointsRemaining
        {
            get { return Math.Round(_PointsRemaining, 2); }
            set
            {
                if (value != _PointsRemaining)
                {
                    _PointsRemaining = value;
                    OnPropertyChanged("PointsRemaining");
                }
            }
        }
        public double HighestScore
        {
            get { return this.Statistics.Highest_Test_Score; }
            set
            {
                this.Statistics.Highest_Test_Score = value;
                OnPropertyChanged("HighestScore");
            }
        }
        public double AverageScore
        {
            get { return this.Statistics.Average_Test_Score; }
            set
            {
                this.Statistics.Average_Test_Score = value;
                OnPropertyChanged("AverageScore");
            }
        }
        public double LowestScore
        {
            get { return this.Statistics.Lowest_Test_Score; }
            set
            {
                this.Statistics.Lowest_Test_Score = value;
                OnPropertyChanged("LowestScore");
            }
        }
        public byte[] Upload_File { get; set; }
        public string Upload_File_Name
        {
            get { return _UploadFileName; }
            set
            {
                if (value != _UploadFileName)
                {
                    _UploadFileName = value;
                    OnPropertyChanged("Upload_File_Name");
                }
            }
        }
        public bool Individual_Values
        {
            get { return _IndividualPoints; }
            set
            {
                if (value != _IndividualPoints)
                {
                    _IndividualPoints = value;
                    OnPropertyChanged("Individual_Values");
                }
            }
        }
        public bool HasSections
        {
            get { return _HasSections; }
            set
            {
                if (value != _HasSections)
                {
                    _HasSections = value;
                    OnPropertyChanged("HasSections");
                }
            }
        }
        public object Database_ID { get; set; }
        public Section RequiredSection { get { return this.Sections[0]; } }
        public Section SelectedSection { get; set; }
        public ObservableCollection<Section> Sections { get; set; }
        public Test_Statistics Statistics
        {
            get { return _TestStats; }
            set
            {
                _TestStats = value;
                OnPropertyChanged("Statistics");
            }
        }
        public bool Is_Graded { get; set; }
        public bool Has_Student_Answers { get; set; }
        public bool Has_Essays_or_ShortAnswers
        {
            get
            {
                foreach (Question question in Answers)
                    if (question is ShortAnswer || question is Essay)
                        return true;
                return false;
            }
        }
        public int Total_Questions
        {
            get
            {
                int count = 0;
                foreach (Section section in this.Sections)
                    count += section.Required_Questions;

                return count;
            }
        }
        public string DateModified { get { return DateTime.Today.ToLocalTime().ToString(); } }
        public object Server_ID { get; set; }
        public Question SelectedQuestion { get; set; }
        public DateTime EndTime
        {
            get { return _EndTime; }
            set
            {
                if (value != _EndTime)
                {
                    _EndTime = value;
                    OnPropertyChanged("EndTime");
                }
            }
        }

        //Public Methods
        public void AddQuestion(Question newQuestion)
        {
            newQuestion.Number = this.Answers.Count + 1;
            newQuestion.SetParentTest(this);
            newQuestion.TestSection = this.RequiredSection;
            this.Answers.Add(newQuestion);
        }
        public void AddQuestion(Question newQuestion, int Index)
        {
            newQuestion.Number = Index + 1;
            newQuestion.SetParentTest(this);
            newQuestion.TestSection = this.RequiredSection;
            this.Answers.Insert(Index, newQuestion);
        }
        public void AddTestSection(Section newSection)
        {
            newSection.Section_Number = this.Sections.Count;
            this.Sections.Add(newSection);
            this.HasSections = true;
        }
        public void DeleteTestSection(Section deleteSection)
        {
            //Set all of the questions that were under this section to the Required Section
            for (int i = 0; i < deleteSection.Questions.Count; i++)
                this.Answers[deleteSection.Questions[i].Number - 1].TestSection = this.RequiredSection;

            //Delete the section and update if it has any other sections than the required
            this.Sections.Remove(deleteSection);
            if (this.Sections.Count <= 1)
                this.HasSections = false;
        }
        /// <summary>
        /// Changes the section of the question.
        /// </summary>
        /// <param name="question">The question to have its section changed.</param>
        /// <param name="prevSection">The previos section.</param>
        /// <param name="postSection">The new section.</param>
        public void SwitchQuestionTestSection(Question question, Section prevSection, Section postSection)
        {
            //Remove the question from the previous section
            if (prevSection != null)
                prevSection.Questions.Remove(question);

            //Add the question to the new section
            if (postSection != null)
                postSection.Questions.Add(question);

            //If the section involves the 'required section', update the number of required questions
            if (prevSection == this.RequiredSection)
                prevSection.DecrementRequiredQuestions();
            else if (postSection == this.RequiredSection)
                postSection.IncrementRequiredQuestions();
        }
        /// <summary>
        /// Increment all question numbers by one starting at a specific index.
        /// </summary>
        /// <param name="index">Index to start incrementing at.</param>
        public void IncrementQuestionNumbers(int index)
        {
            for (int i = index; i < this.Answers.Count; i++)
                this.Answers[i].Number++;
        }
        /// <summary>
        /// Recalculates a fresh value of the remaining points to be used in the test.
        /// </summary>
        public void UpdatePointsRemaining()
        {
            //Recalculate the Points Remaining
            this.PointsRemaining = this.Point_Worth;
            foreach (Question question in this.Answers)
                this.PointsRemaining -= question.Point_Value;
        }
        /// <summary>
        /// Updates the points remaining to be used in the test when a specific question's point value is changed.
        /// </summary>
        /// <param name="PreviousPointWorth">Old Point Value.</param>
        /// <param name="NewPointWorth">New Point Value.</param>
        public void UpdatePointsRemaining(double PreviousPointWorth, double NewPointWorth)
        {
            this.PointsRemaining += (PreviousPointWorth - NewPointWorth);
        }
        public void TimeIncrementHour() { this.EndTime.AddHours(1.0); }
        public void TimeDecrementHours() { this.EndTime.AddHours(-1.0); }

        //Commands
        public Browse_Command Browse { get; set; }
        public OpenFile_Command OpenFile { get; set; }
        public AddMultipleChoiceQuestion_Command AddMultipleChoice { get; set; }
        public AddMatchingQuestion_Command AddMatching { get; set; }
        public AddTrueFalseQuestion_Command AddTrueFalse { get; set; }
        public AddShortAnswerQuestion_Command AddShortAnswer { get; set; }
        public AddEssayQuestion_Command AddEssay { get; set; }
        public AddTestSection_Command AddSection { get; set; }
        public DeleteTestSection_Command DeleteSection { get; set; }

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

        //Private Variables
        private string _Initials, _Test_Name, _UploadFileName;
        private bool _IndividualPoints, _HasSections;
        private double _PointsRemaining, _PointWorth;
        private Initials TestInitials;
        private Test_Statistics _TestStats;
        private DateTime _EndTime;
    }

    public class AddMultipleChoiceQuestion_Command : ICommand
    {
        Test test;
        public event EventHandler CanExecuteChanged;
        public AddMultipleChoiceQuestion_Command(Test _Test) { test = _Test; }
        public bool CanExecute(object parameter) { return true; }

        public void Execute(object parameter)
        {
            //If a question is to be added to the end of the list of answers, then just add it normally
            if (test.SelectedQuestion == null || test.SelectedQuestion.Number == test.Answers.Count)
                this.test.AddQuestion(new MultipleChoice(test));
            //Else add the question at a specific index that is one after the selected question
            else
            {
                this.test.AddQuestion(new MultipleChoice(test), test.SelectedQuestion.Number);
                this.test.IncrementQuestionNumbers(test.SelectedQuestion.Number + 1);
            }
        }
    }

    public class AddMatchingQuestion_Command : ICommand
    {
        Test test;
        public event EventHandler CanExecuteChanged;
        public AddMatchingQuestion_Command(Test _Test) { test = _Test; }
        public bool CanExecute(object parameter) { return true; }

        public void Execute(object parameter)
        {
            //If a question is to be added to the end of the list of answers, then just add it normally
            if (test.SelectedQuestion == null || test.SelectedQuestion.Number == test.Answers.Count)
                this.test.AddQuestion(new Matching(test));
            //Else add the question at a specific index that is one after the selected question
            else
            {
                this.test.AddQuestion(new Matching(test), test.SelectedQuestion.Number);
                this.test.IncrementQuestionNumbers(test.SelectedQuestion.Number + 1);
            }
        }
    }

    public class AddTrueFalseQuestion_Command : ICommand
    {
        Test test;
        public event EventHandler CanExecuteChanged;
        public AddTrueFalseQuestion_Command(Test _Test) { test = _Test; }
        public bool CanExecute(object parameter) { return true; }

        public void Execute(object parameter)
        {
            //If a question is to be added to the end of the list of answers, then just add it normally
            if (test.SelectedQuestion == null || test.SelectedQuestion.Number == test.Answers.Count)
                this.test.AddQuestion(new TrueFalse(test));
            //Else add the question at a specific index that is one after the selected question
            else
            {
                this.test.AddQuestion(new TrueFalse(test), test.SelectedQuestion.Number);
                this.test.IncrementQuestionNumbers(test.SelectedQuestion.Number + 1);
            }
        }
    }

    public class AddEssayQuestion_Command : ICommand
    {
        Test test;
        public event EventHandler CanExecuteChanged;
        public AddEssayQuestion_Command(Test _Test) { test = _Test; }
        public bool CanExecute(object parameter) { return true; }

        public void Execute(object parameter)
        {
            //If a question is to be added to the end of the list of answers, then just add it normally
            if (test.SelectedQuestion == null || test.SelectedQuestion.Number == test.Answers.Count)
                this.test.AddQuestion(new Essay(test));
            //Else add the question at a specific index that is one after the selected question
            else
            {
                this.test.AddQuestion(new Essay(test), test.SelectedQuestion.Number);
                this.test.IncrementQuestionNumbers(test.SelectedQuestion.Number + 1);
            }
        }
    }

    public class AddShortAnswerQuestion_Command : ICommand
    {
        Test test;
        public event EventHandler CanExecuteChanged;
        public AddShortAnswerQuestion_Command(Test _Test) { test = _Test; }
        public bool CanExecute(object parameter) { return true; }

        public void Execute(object parameter)
        {
            //If a question is to be added to the end of the list of answers, then just add it normally
            if (test.SelectedQuestion == null || test.SelectedQuestion.Number == test.Answers.Count)
                this.test.AddQuestion(new ShortAnswer(test));
            //Else add the question at a specific index that is one after the selected question
            else
            {
                this.test.AddQuestion(new ShortAnswer(test), test.SelectedQuestion.Number);
                this.test.IncrementQuestionNumbers(test.SelectedQuestion.Number + 1);
            }
        }
    }

    public class AddTestSection_Command : ICommand
    {
        Test test;
        public event EventHandler CanExecuteChanged;
        public AddTestSection_Command(Test _Test) { test = _Test; }
        public bool CanExecute(object parameter) { return true; }

        public void Execute(object parameter)
        {
            this.test.AddTestSection(new Section(test));
        }
    }

    public class DeleteTestSection_Command : ICommand
    {
        Test test;
        public event EventHandler CanExecuteChanged;

        public DeleteTestSection_Command(Test _Test)
        {
            test = _Test;
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            if (test.SelectedSection != test.RequiredSection)
            {
                test.DeleteTestSection(test.SelectedSection);
            }

        }
    }
}
