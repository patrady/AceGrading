using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;

namespace AceGrading
{
    public class Test : INotifyPropertyChanged
    {
        public Test()
        {
            Answers = new ObservableCollection<Question>();
            Sections = new ObservableCollection<Section>() { new Section(this) { Section_Number = 0 } };
            WifiUsers = new ObservableCollection<Student>();
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
            UpdateTestEndTime = new UpdateTestEndTime_Command(this);
            StartTest = new StartTest_Command(this);
            LayoutPresets = new QuestionLayoutPresets_ContainerUI();
            Statistics = new Test_Statistics();
            TestName = "Test Name";
            TestStatus = Test_Status.CollectingData;
            Upload_File = null;
            Individual_Values = false;
            HasWifiUsers = false;
            HasSections = false;
            Point_Worth = 0;
            Is_Graded = false;
            IsWifiDetectionEnabled = true;
            Has_Student_Answers = false;
            Upload_File_Name = null;
            Server_ID = null;
            EndTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, DateTime.Now.Hour, DateTime.Now.Minute, 0);
            NewEndTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, DateTime.Now.Hour, DateTime.Now.Minute, 0);
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
        public ObservableCollection<Student> WifiUsers { get; set; }
        public Class ParentClass { get; set; }
        public Test_Status TestStatus
        {
            get { return _TestStatus; }
            set
            {
                if (value != _TestStatus)
                {
                    _TestStatus = value;
                    OnPropertyChanged("TestStatus");
                }
            }
        }
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

                    if (Individual_Values)
                        UpdatePointsRemaining();
                    else UpdatePointsPerQuestion();
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

                    //Update the calculation based off of this boolean
                    if (_IndividualPoints)
                        this.UpdatePointsRemaining();
                    else this.UpdatePointsPerQuestion();
                }
            }
        }
        public bool HasWifiUsers
        {
            get { return _HasWifiUsers; }
            set
            {
                if (value != _HasWifiUsers)
                {
                    _HasWifiUsers = value;
                    OnPropertyChanged("HasWifiUsers");
                }
            }
        }
        public bool IsWifiDetectionEnabled
        {
            get { return _IsWifiDetectionEnabled; }
            set
            {
                if (value != _IsWifiDetectionEnabled)
                {
                    _IsWifiDetectionEnabled = value;
                    OnPropertyChanged("IsWifiDetectionEnabled");
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
        public double PointsPerQuestion
        {
            get { return Math.Round(_PointsPerQuestion, 2); }
            set
            {
                if (value != _PointsPerQuestion)
                {
                    _PointsPerQuestion = value;
                    OnPropertyChanged("PointsPerQuestion");
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
        public DateTime NewEndTime
        {
            get { return _NewEndTime; }
            set
            {
                if (value != _NewEndTime)
                {
                    _NewEndTime = value;
                    OnPropertyChanged("NewEndTime");
                }
            }
        }
        public TimeSpan TimeRemaining
        {
            get { return _TimeRemaining; }
            set
            {
                if (value != _TimeRemaining)
                {
                    _TimeRemaining = value;
                    OnPropertyChanged("TimeRemaining");
                }
            }
        }
        public QuestionLayoutPresets_ContainerUI LayoutPresets { get; set; }

        //Public Methods
        public void AddQuestion(Question newQuestion)
        {
            newQuestion.Number = this.Answers.Count + 1;
            newQuestion.SetParentTest(this);
            newQuestion.TestSection = this.RequiredSection;
            this.Answers.Add(newQuestion);
            this.UpdatePointsPerQuestion();
        }
        public void AddQuestion(Question newQuestion, int Index)
        {
            newQuestion.Number = Index + 1;
            newQuestion.SetParentTest(this);
            newQuestion.TestSection = this.RequiredSection;
            this.Answers.Insert(Index, newQuestion);
            this.UpdatePointsPerQuestion();
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
            for (int i = 0; i < deleteSection.Questions.Count;)
                deleteSection.Questions[i].TestSection = this.RequiredSection;

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
                prevSection.RemoveQuestion(question);

            //Add the question to the new section
            if (postSection != null)
                postSection.AddQuestion(question);
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
            if (!this.HasSections)
            {
                foreach (Question question in this.Answers)
                    this.PointsRemaining -= question.Point_Value;
            }
            else
            {
                //Since this test has sections, take the highest question point values and add those up 
                List<Question> TempQuestions;
                double sectionScore = 0;
                double runningScore = 0;
                foreach (Section section in this.Sections)
                {
                    //Copy the section questions to a new list and sort them by point value worth
                    TempQuestions = new List<Question>(section.Questions);
                    TempQuestions.Sort((x, y) => x.Point_Value.CompareTo(y.Point_Value));

                    //Take the top 'n' number of point values and sum them where 'n' = section's # of required questions
                    for (int i = section.Total_Questions - section.Required_Questions; i < section.Total_Questions; i++)
                        sectionScore += TempQuestions[i].Point_Value;
                    runningScore += sectionScore;
                    sectionScore = 0;
                }
                this.PointsRemaining -= runningScore;   
            }
            
        }
        /// <summary>
        /// Updates the points remaining to be used in the test when a specific question's point value is changed.
        /// </summary>
        /// <param name="PreviousPointWorth">Old Point Value.</param>
        /// <param name="NewPointWorth">New Point Value.</param>
        public void UpdatePointsRemaining(double PreviousPointWorth, double NewPointWorth)
        {
            if (!this.HasSections)
                this.PointsRemaining += (PreviousPointWorth - NewPointWorth);
            else
                UpdatePointsRemaining();
        }
        public void StartTimeCountdown()
        {
            this.TestStatus = Test_Status.Started;
            this.TimeRemaining = this.EndTime.Subtract(DateTime.Now);
            TimeCountdown = new DispatcherTimer(new TimeSpan(0, 0, 1), DispatcherPriority.Normal, 
                                                delegate
                                                {
                                                    if (TimeRemaining <= TimeSpan.Zero.Add(TimeSpan.FromSeconds(1)))
                                                    {
                                                        this.StopTimeCountdown();
                                                    }
                                                    TimeRemaining = TimeRemaining.Add(TimeSpan.FromSeconds(-1));

                                                    //When debugging, simulate some student data
                                                    if (TimeRemaining.Seconds % 5 == 0)
                                                        this.DebugMode();
                                                }, Application.Current.Dispatcher);

            TimeCountdown.Start();
        }
        public void StopTimeCountdown()
        {
            if (TimeCountdown != null)
                if (TimeCountdown.IsEnabled)
                {
                    TimeCountdown.Stop();
                    this.TestStatus = Test_Status.CollectingData;
                    this.TimeRemaining = TimeSpan.Zero;
                }       
        }
        /// <summary>
        /// Update the Points Per Question attribute.
        /// </summary>
        public void UpdatePointsPerQuestion()
        {
            int numRequiredQuestions = 0;

            //Take each test section and total up the numer of required question
            foreach (Section section in this.Sections)
                numRequiredQuestions += section.Required_Questions;

            //Do not divide by zero
            if (numRequiredQuestions == 0)
                this.PointsPerQuestion = 0;
            else
                this.PointsPerQuestion = this.Point_Worth / numRequiredQuestions;
        }
        public void UpdateEndTime(DateTime newEndTime)
        {
            //Stop the countdown if there is one
            this.StopTimeCountdown();

            //Update the test end time
            this.EndTime = newEndTime;

            //restart the countdown
            this.StartTimeCountdown();
        }

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
        public UpdateTestEndTime_Command UpdateTestEndTime { get; set; }
        public StartTest_Command StartTest { get; set; }

        //Private Methods
        private void DebugMode()
        {
            Random rand = new Random();
            foreach (Student student in this.ParentClass.Students)
            {
                if (student.Status == Online_Status.Finished)
                    continue;

                //Update the student Progress
                student.TestProgress += rand.Next(0, 10);
                if (student.TestProgress > this.Point_Worth)
                {
                    student.TestProgress = this.Point_Worth;
                    student.Status = Online_Status.Finished;
                }
                    
                //Update the Online Status
                if (student.TestProgress == 0)
                    student.Status = Online_Status.Offline;
                else if (student.TestProgress > 0 && student.TestProgress < this.Point_Worth)
                    student.Status = Online_Status.Online;
                else
                    student.Status = Online_Status.Finished;

                //Simulate a random disconnection
                if (rand.Next(0, 5) == 4)
                    student.Status = Online_Status.Offline;

                //Update the student wifi
                switch (rand.Next(1, 3))
                {
                    case 1: student.WifiUsage = Wifi_Status.AbstainingWifi; break;
                    case 2: student.WifiUsage = Wifi_Status.UsingWifi; break;
                    default: student.WifiUsage = Wifi_Status.AbstainingWifi; break;
                }
            }

            //Check the wifi usage of all students
            CheckIfStudentsUsingWifi();
        }
        private void CheckIfStudentsUsingWifi()
        {
            foreach (Student student in this.ParentClass.Students)
            {
                if (student.WifiDetected)
                {
                    if (!this.WifiUsers.Contains(student))
                        this.WifiUsers.Add(student);
                }   
                else
                {
                    if (this.WifiUsers.Contains(student))
                        this.WifiUsers.Remove(student);
                }
            }

            //Update the boolean that indicates if there are users using the internet
            if (this.WifiUsers.Count > 0)
                this.HasWifiUsers = true;
            else
                this.HasWifiUsers = false;
        }

        //INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        //Private Variables
        private string _Initials, _Test_Name, _UploadFileName;
        private bool _IndividualPoints, _HasSections, _IsWifiDetectionEnabled, _HasWifiUsers;
        private double _PointsRemaining, _PointWorth, _PointsPerQuestion;
        private Test_Status _TestStatus;
        private Initials TestInitials;
        private Test_Statistics _TestStats;
        private DateTime _EndTime, _NewEndTime;
        private TimeSpan _TimeRemaining;
        private DispatcherTimer TimeCountdown;
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
            if (test.SelectedQuestion == null)
                this.test.AddQuestion(new MultipleChoice(test));
            //Else add the question at a specific index that is one after the selected question
            else
            {
                this.test.AddQuestion(new MultipleChoice(test), test.SelectedQuestion.Number - 1);
                this.test.IncrementQuestionNumbers(test.SelectedQuestion.Number);
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
            if (test.SelectedQuestion == null)
                this.test.AddQuestion(new Matching(test));
            //Else add the question at a specific index that is one after the selected question
            else
            {
                this.test.AddQuestion(new Matching(test), test.SelectedQuestion.Number - 1);
                this.test.IncrementQuestionNumbers(test.SelectedQuestion.Number);
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
            if (test.SelectedQuestion == null)
                this.test.AddQuestion(new TrueFalse(test));
            //Else add the question at a specific index that is one after the selected question
            else
            {
                this.test.AddQuestion(new TrueFalse(test), test.SelectedQuestion.Number - 1);
                this.test.IncrementQuestionNumbers(test.SelectedQuestion.Number);
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
            if (test.SelectedQuestion == null)
                this.test.AddQuestion(new Essay(test));
            //Else add the question at a specific index that is one after the selected question
            else
            {
                this.test.AddQuestion(new Essay(test), test.SelectedQuestion.Number - 1);
                this.test.IncrementQuestionNumbers(test.SelectedQuestion.Number);
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
            if (test.SelectedQuestion == null)
                this.test.AddQuestion(new ShortAnswer(test));
            //Else add the question at a specific index that is one after the selected question
            else
            {
                this.test.AddQuestion(new ShortAnswer(test), test.SelectedQuestion.Number - 1);
                this.test.IncrementQuestionNumbers(test.SelectedQuestion.Number);
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

    public class UpdateTestEndTime_Command : ICommand
    {
        Test test;
        public event EventHandler CanExecuteChanged;

        public UpdateTestEndTime_Command(Test _Test) { test = _Test; }
        public bool CanExecute(object parameter) { return true; }

        public void Execute(object parameter)
        {
            //Update the time only if the new end time is greater than the previous end time
            if (this.test.NewEndTime != null)
                if (this.test.NewEndTime > DateTime.Now)
                    this.test.UpdateEndTime(this.test.NewEndTime);
        }
    }

    public class StartTest_Command : ICommand
    {
        Test test;
        public event EventHandler CanExecuteChanged;

        public StartTest_Command(Test _Test) { test = _Test; }
        public bool CanExecute(object parameter) { return true; }

        public void Execute(object parameter)
        {
            this.test.NewEndTime = this.test.EndTime;
            this.test.StartTimeCountdown();
        }
    }

    public class QuestionLayoutPresets_ContainerUI : INotifyPropertyChanged
    {
        public QuestionLayoutPresets_ContainerUI()
        {
            MultChoicePresets = new ObservableCollection<QuestionLayoutPreset_UI>()
            {
                new QuestionLayoutPreset_UI() { Letters = new List<char> {'A', 'B', 'C' } },
                new QuestionLayoutPreset_UI() { Letters = new List<char> {'A', 'B', 'C', 'D' } },
                new QuestionLayoutPreset_UI() { Letters = new List<char> {'A', 'B', 'C', 'D', 'E' } },
                new QuestionLayoutPreset_UI() { Letters = new List<char> {'I', 'J', 'K' } },
                new QuestionLayoutPreset_UI() { Letters = new List<char> {'I', 'J', 'K', 'L' } },
                new QuestionLayoutPreset_UI() { Letters = new List<char> {'I', 'J', 'K', 'L', 'M' } },
            };
        }

        //Public Attributes
        public ObservableCollection<QuestionLayoutPreset_UI> MultChoicePresets { get; set; }
        public QuestionLayoutPreset_UI SelectedPreset
        {
            get { return _SelectedPreset; }
            set
            {
                if (value != _SelectedPreset)
                {
                    _SelectedPreset = value;
                    OnPropertyChanged("SelectedPreset");
                }
            }
        }

        //Public Methods
        public void AddOrUpdatePreset(MultipleChoice MultChoice)
        {
            int index;
            if ((index = CheckIfPresentExists(MultChoice)) == -1)
            {
                //Add the new preset to the list
                List<char> tempLetters = new List<char>();
                for (int i = 0; i < MultChoice.NumberOptions; i++)
                    tempLetters.Add(MultChoice.OptionalAnswers[i].Letter);
                this.LocalMultChoicePresets.Add(new QuestionLayoutPreset_UI() { Letters = tempLetters });

                //Reorganize the Presets
                OrganizePresets(this.LocalMultChoicePresets);
            }
            else
            {
                //Increment the number of times this preset has been used
                this.LocalMultChoicePresets[index].IncrementNumUses();
            }
        }
        public void RemovePreset(MultipleChoice MultChoice)
        {
            int index;
            if ((index = CheckIfPresentExists(MultChoice)) > -1)
            {
                //Decrement and Reorganize the Presets
                this.LocalMultChoicePresets[index].DecrementNumUses();
                this.OrganizePresets(this.LocalMultChoicePresets);
            }
        }

        //Private Methods
        private void OrganizePresets(ObservableCollection<QuestionLayoutPreset_UI> Preset)
        {
            Preset = new ObservableCollection<QuestionLayoutPreset_UI>(Preset.OrderByDescending(x => x.NumberOfUses));
        }
        private void UpdateDisplayedMultChoicePresets()
        {
            //Display Up to set number of presets, so if less presets exist then show all
            if (this.LocalMultChoicePresets.Count <= maxDisplayed)
                this.MultChoicePresets = this.LocalMultChoicePresets;
            else
            {
                //Since there are more than the set number of presets, only had up to that predefined number
                this.MultChoicePresets.Clear();
                for (int i = 0; i < maxDisplayed; i++)
                    this.MultChoicePresets.Add(this.LocalMultChoicePresets[i]);
            }
        }
        private int CheckIfPresentExists(MultipleChoice MultChoiceQuestion)
        {            
            for (int i = 0; i < this.LocalMultChoicePresets.Count; i++)
            {
                //Check if they have the same number of letters
                if (this.LocalMultChoicePresets[i].NumberOfLetters == MultChoiceQuestion.NumberOptions)
                {
                    //Check if they have the first starting letter, if they do then they are identical
                    if (this.LocalMultChoicePresets[i].Letters[0] == MultChoiceQuestion.OptionalAnswers[0].Letter)
                        return i;
                }
            }
            return -1;
        }

        //Private Variables
        private QuestionLayoutPreset_UI _SelectedPreset;
        private ObservableCollection<QuestionLayoutPreset_UI> LocalMultChoicePresets { get; set; }
        static int maxDisplayed = 10;

        //INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    public class QuestionLayoutPreset_UI
    {
        public QuestionLayoutPreset_UI()
        {
            this.Letters = new List<char>();
            this.NumberOfUses = 0;
        }

        //Public Attributes
        public List<char> Letters { get; set; }
        public int NumberOfUses { get; set; }
        public int NumberOfLetters { get { return this.Letters.Count; } }

        //Public Methods
        public void IncrementNumUses()
        {
            this.NumberOfUses++;
        }
        public void DecrementNumUses()
        {
            if (this.NumberOfUses > 0)
                this.NumberOfUses--;
        }
    }
}
