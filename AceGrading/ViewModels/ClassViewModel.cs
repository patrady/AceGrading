using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;

namespace AceGrading
{
    public class MainViewModel : INotifyPropertyChanged
    {
        public MainViewModel()
        {
            this.Classes = new ObservableCollection<Class>()
            {
                new Class()
                {
                    Class_Name = "Theology IV",
                    Students = new ObservableCollection<Student>()
                    {
                        new Student() {Name = "Robert Brady" },
                        new Student() {Name = "Julie Brady" },
                        new Student() {Name = "Kristen Duke" },
                        new Student() {Name = "Joe Cloud" },
                        new Student() {Name = "Nick Nocholi" },
                    },
                    Tests = new ObservableCollection<Test>()
                    {
                        new Test() { TestName = "Test #1", Upload_File_Name = @"C:\Users\rober\Desktop\pingpong.png", Point_Worth=100, Statistics = new Test_Statistics(), Is_Graded = true, HighestScore = 99},
                        new Test() { TestName = "Test #2", Upload_File_Name = @"C:\Users\rober\Desktop\Doc1.docx", Point_Worth = 200, Statistics = new Test_Statistics(), Is_Graded = true, HighestScore = 98}
                    },
                    HasTests = true
                },
                new Class()
                {
                    Class_Name = "History / Geography",
                    Students = new ObservableCollection<Student>()
                    {
                        new Student() {Name = "Joseph Herring" },
                        new Student() {Name = "Alberto Rudeo" },
                        new Student() {Name = "Laura Cook" },
                        new Student() {Name = "PJ Biyani" },
                        new Student() {Name = "Kartik Gupta" },
                    },
                    Tests = new ObservableCollection<Test>()
                    {
                        new Test() { TestName = "Test #3", Statistics = new Test_Statistics(), Is_Graded = true, HighestScore = 97},
                        new Test() { TestName = "Test #4", Statistics = new Test_Statistics(), Is_Graded = true, HighestScore = 96 }
                    },
                    HasTests = true
                }
            };

            this.HasClasses = true;
            this.DeleteClass = new DeleteClass_Command(this);
            this.AddClass = new AddClass_Command(this);
            this.PreAddClass = new PreAddClass_Command(this);
            NewClass = new Class();
            SelectedClass = this.Classes.ElementAt(0);
        }

        //Attributes
        public ObservableCollection<Class> Classes { get; set; }
        public Class NewClass { get; set; }
        public Class SelectedClass
        {
            get { return _SelectedClass; }
            set
            {
                if (value != _SelectedClass)
                {
                    _SelectedClass = value;
                    OnPropertyChanged("SelectedClass");
                }
            }
        }
        public bool HasClasses
        {
            get { return _HasClasses; }
            set
            {
                if (value != _HasClasses)
                {
                    _HasClasses = value;
                    OnPropertyChanged("HasClasses");
                }
            }
        }

        //Methods
        public ReturnValidation Add_Class(Class _class)
        {
            //Check if empty
            if (_class.Class_Name == null || _class.Class_Name == "")
                return new ReturnValidation(_IsOk: false, _Header: "Add Class", _Body: "A class must receive a valid name.");

            //Check if the test name is already taken
            foreach (Class tempClass in this.Classes)
                if (tempClass.Class_Name.ToLower() == _class.Class_Name.ToLower())
                    return new ReturnValidation(_IsOk: false, _Header: "Add Class", _Body: "A class by this name already exists, please choose another name.");

            //Add the test since no other student has the name
            this.Classes.Add(_class);
            this.HasClasses = true;
            return new ReturnValidation(_IsOk: true);
        }
        public ReturnValidation ReName_Class(Class _class, string NewName)
        {
            //Check if empty
            if (_class.Class_Name == null || _class.Class_Name == "")
                return new ReturnValidation(_IsOk: false, _Header: "Rename Class", _Body: "A class must receive a valid name.");

            //Check if the student name matches any other student names
            foreach (Class tempClass in this.Classes)
                if (NewName.ToLower() == tempClass.Class_Name.ToLower())
                    if (tempClass != _class)
                        return new ReturnValidation(_IsOk: false, _Header: "Rename Class", _Body: "A class by this name already exists.");

            _class.Class_Name = NewName;
            return new ReturnValidation(_IsOk: true);
        }
        public void Delete_Class(Class _class)
        {
            this.Classes.Remove(_class);

            if (this.Classes.Count == 0)
                this.HasClasses = false;
        }
        public void ChangeSelectedClass()
        {
            if (this.HasClasses)
                this.SelectedClass = this.Classes[0];
        }
        public bool IsClassInList(Class _class) 
        {
            return this.Classes.Contains(_class);
        }

        //Commands
        public PreAddClass_Command PreAddClass { get; set; }
        public AddClass_Command AddClass { get; set; }
        public DeleteClass_Command DeleteClass { get; set; }

        //Private Variables
        private Class _SelectedClass;
        private bool _HasClasses;

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

    public class Student : INotifyPropertyChanged
    {
        public Student()
        {
            StudentInitials = new Initials();
            this.Name = this.DefaultName;

            Student_Test_Answers = new List<Student_Answer>();
            Bonus_Points = 0;
            StartTest_Column = -1;
            StartTest_Row = -1;
            Database_ID = -1;
            LoginKey = 1234;
        }

        //Variables
        public string Name
        {
            get { return _Name; }
            set
            {
                if (value != _Name)
                {
                    _Name = value;
                    _Initials = StudentInitials.MakeInitials(_Name);
                    OnPropertyChanged("Name");
                    OnPropertyChanged("Initials");
                }
            }
        }
        public string DefaultName { get { return "Student Name"; } }
        public string Initials { get { return _Initials; } }
        public double Test_Score
        {
            get
            {
                double score = 0;

                //Tallies up the student's test score
                foreach (Student_Answer answer in Student_Test_Answers)
                    if (answer.Has_Been_Graded)
                        score += answer.Points_Received;

                return score + Bonus_Points;
            }
        }
        public object Database_ID { get; set; }
        public double Bonus_Points { get; set; }
        public int StartTest_Row { get; set; }
        public int StartTest_Column { get; set; }
        public List<Student_Answer> Student_Test_Answers { get; set; }
        private Initials StudentInitials;
        public int LoginKey { get; set; }

        //Methods

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
        private string _Name, _Initials;
    }

    public class ReturnValidation
    {
        /// <summary>
        /// Returns true or false indicating whether the objective was executed.
        /// </summary>
        /// <param name="_IsOk">True or False</param>
        public ReturnValidation(bool _IsOk)
        {
            this.IsOk = _IsOk;
            this.Header = null;
            this.Body = null;
        }
        /// <summary>
        /// Returns true or false indicating whether the objective was executed while explaining the outcome.
        /// </summary>
        /// <param name="_IsOk">True or False</param>
        /// <param name="_Header">Header of the message.</param>
        /// <param name="_Body">Body of the message.</param>
        public ReturnValidation(bool _IsOk, string _Header, string _Body)
        {
            this.IsOk = _IsOk;
            this.Header = _Header;
            this.Body = _Body;
        }

        //Private Attributes
        private bool IsOk { get; set; }
        private string Header { get; set; }
        private string Body { get; set; }

        //Public Methods
        /// <summary>
        /// Returns if the objective was executed or not.
        /// </summary>
        /// <returns></returns>
        public bool GetIsOk() { return this.IsOk; }
        /// <summary>
        /// Gets the Header of the execution outcome.
        /// </summary>
        /// <returns>Returns null if empty.</returns>
        public string GetErrorHeader() { return this.Header; }
        /// <summary>
        /// Gets the Body of the execution outcome.
        /// </summary>
        /// <returns>Returns null if empty.</returns>
        public string GetErrorBody() { return this.Body; }
    }

    public class Class : INotifyPropertyChanged
    {
        public Class()
        {
            Students = new ObservableCollection<Student>();
            Tests = new ObservableCollection<Test>();
            ClassInitials = new Initials();
            NewStudent = new Student();
            NewTest = new Test();
            this.HasTests = false;
            this.Class_Name = this.DefaultName;
            this.DeleteStudent = new DeleteStudent_Command(this);
            this.AddStudent = new AddStudent_Command(this);
            this.PreAddTest = new PreAddTest_Command(this);
            this.AddTest = new AddTest_Command(this);
            this.DeleteTest = new DeleteTest_Command(this);
        }

        //Variables
        public ObservableCollection<Student> Students { get; set; }
        public ObservableCollection<Test> Tests { get; set; }
        public string Class_Name
        {
            get { return _Class_Name; }
            set
            {
                _Class_Name = value;
                _Class_Intials = ClassInitials.MakeInitials(_Class_Name);
                OnPropertyChanged("Class_Name");
                OnPropertyChanged("Class_Initials");
            }
        }
        public string DefaultName { get { return "Class Name"; } }
        public int TestCount { get { return this.Tests.Count; } }
        public int StudentCount { get { return this.Students.Count; } }
        public string Class_Initials { get { return _Class_Intials; } }
        public Student NewStudent { get; set; }
        public Test NewTest { get; set; }
        public bool HasTests
        {
            get { return _HasTests; }
            set
            {
                if (value != _HasTests)
                {
                    _HasTests = value;
                    OnPropertyChanged("HasTests");
                }
            }
        }
        private Initials ClassInitials;
        public Student SelectedStudent
        {
            get { return _SelectedStudent; }
            set
            {
                _SelectedStudent = value;
                OnPropertyChanged("SelectedStudent");
            }
        }
        public Test SelectedTest
        {
            get { return _SelectedTest; }
            set
            {
                _SelectedTest = value;
                OnPropertyChanged("SelectedTest");
            }
        }

        //Methods
        public ReturnValidation Add_Student(Student student)
        {
            //Check if empty
            if (student.Name == null || student.Name == "")
                return new ReturnValidation(_IsOk: false, _Header: "Add Student", _Body: "A student must receive a valid name.");

            //Check if the student name matches any other student names
            foreach (Student tempStudent in this.Students)
                if (tempStudent.Name.ToLower() == student.Name.ToLower())
                    return new ReturnValidation(_IsOk: false, _Header: "Add Student", _Body: "A student by this name already exists, please choose another name.");

            //Add the student since no other student has the name
            this.Students.Add(new Student() { Name = student.Name });
            return new ReturnValidation(_IsOk: true);
        }
        public ReturnValidation Add_Test(Test test)
        {
            //Check if empty
            if (test.TestName == null || test.TestName == "")
                return new ReturnValidation(_IsOk: false, _Header: "Add Test", _Body: "A test must receive a valid name.");

            //Check if the test name is already taken
            foreach (Test temptest in this.Tests)
                if (temptest.TestName.ToLower() == test.TestName.ToLower())
                    return new ReturnValidation(_IsOk: false, _Header: "Add Test", _Body: "A test by this name already exists, please choose another name.");

            //Add the test since no other student has the name
            this.Tests.Add(test);
            this.HasTests = true;
            return new ReturnValidation(_IsOk: true);
        }
        public ReturnValidation ReName_Student(Student student, string NewName)
        {
            //Check if empty
            if (student.Name == null || student.Name == "")
                return new ReturnValidation(_IsOk: false, _Header: "Rename Student", _Body: "A student must receive a valid name.");

            //Check if the student name matches any other student names
            foreach (Student tempStudent in this.Students)
                if (NewName.ToLower() == tempStudent.Name.ToLower())
                    if (tempStudent != student)
                        return new ReturnValidation(_IsOk: false, _Header: "Rename Student", _Body: "A student by this name already exists.");

            student.Name = NewName;
            return new ReturnValidation(_IsOk: true);
        }
        public ReturnValidation ReName_Test(Test test, string NewName)
        {
            //Check if empty
            if (test.TestName == null || test.TestName == "")
                return new ReturnValidation(_IsOk: false, _Header: "Add Test", _Body: "A test must receive a valid name.");
            
            //Check if the test name matches any other tests
            foreach (Test tempTest in this.Tests)
                if (NewName.ToLower() == tempTest.TestName.ToLower())
                    if (tempTest != test)
                        return new ReturnValidation(_IsOk: false, _Header: "Rename Test", _Body: "A test by this name already exists.");

            test.TestName = NewName;
            return new ReturnValidation(_IsOk: true);
        }
        public void Delete_Student(Student stud)
        {
            this.Students.Remove(stud);
        }
        public void Delete_Test(Test test)
        {
            this.Tests.Remove(test);
            if (this.Tests.Count == 0)
                this.HasTests = false;
        }
        public bool IsStudentInList(Student student)
        {
            return this.Students.Contains(student);
        }
        public bool IsTestInList(Test test)
        {
            return this.Tests.Contains(test);
        }
        /// <summary>
        /// Changes the selected test to be the test at element zero if there is at least one test.
        /// </summary>
        public void ChangeSelectedTest()
        {
            if (this.HasTests)
                this.SelectedTest = this.Tests[0];
        }
        /// <summary>
        /// Changes the class' selected test.
        /// </summary>
        /// <param name="test">The test to change the selected test to.</param>
        public void ChangeSelectedTest(Test test)
        {
            if (this.IsTestInList(test))
                this.SelectedTest = test;
        }

        //Commands
        public DeleteStudent_Command DeleteStudent { get; set; }
        public AddStudent_Command AddStudent { get; set; }
        public PreAddTest_Command PreAddTest { get; set; }
        public AddTest_Command AddTest { get; set; }
        public DeleteTest_Command DeleteTest { get; set; }

        //Private Variables
        private string _Class_Intials, _Class_Name;
        private Test _SelectedTest;
        private Student _SelectedStudent;
        private bool _HasTests;

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
