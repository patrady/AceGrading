using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;

namespace AceGrading
{
    public class ClassStructure : INotifyPropertyChanged
    {
        public ClassStructure() { }

        public ClassStructure(Class _ParentClass)
        {
            this.ParentClass = _ParentClass;
            this.NumColumns = FindMaxColumnNum();
            this.NumRows = FindMaxRowNum();
            this.IncrementColumns = new IncrementClassStructColumns_Command(this);
            this.IncrementRows = new IncrementClassStructRows_Command(this);
            this.DecrementColumns = new DecrementClassStructColumns_Command(this);
            this.DecrementRows = new DecrementClassStructRows_Command(this);
            this.Desks = new ObservableCollection<object>();
            this.DisjointStudents = new ObservableCollection<Student>();

            //Run the initialization methods
            InitializeDesks();
        }

        //Public Attributes
        public int NumRows
        {
            get { return _NumRows; }
            set
            {
                if (value != _NumRows)
                {
                    _NumRows = value;
                    OnPropertyChanged("NumRows");
                }
            }
        }
        public int NumColumns
        {
            get { return _NumColumns; }
            set
            {
                if (value != _NumColumns)
                {
                    _NumColumns = value;
                    OnPropertyChanged("NumColumns");
                }
            }
        }
        public bool HasDisjointStudents
        {
            get { return _HasDisjointStudents; }
            set
            {
                if (value != _HasDisjointStudents)
                {
                    _HasDisjointStudents = value;
                    OnPropertyChanged("HasDisjointStudents");
                }
            }
        }
        public Class ParentClass { get; set; }
        public ObservableCollection<Student> DisjointStudents { get; set; }
        public ObservableCollection<object> Desks { get; set; }

        //Public Methods
        public void IncrementNumRows()
        {
            //Increment the number of rows
            this.NumRows++;

            //Add student placeholders in all of the new empty desks
            for (int i = 0; i < this.NumColumns; i++)
                this.AddStudentPlaceholderAtIndex(this.NumRows - 1, i);
        }
        public void IncrementNumColumns()
        {
            //Increment the number of columns
            this.NumColumns++;

            //Add student placeholders in all of the new empty desks
            for (int i = 0; i < this.NumRows; i++)
                this.AddStudentPlaceholderAtIndex(i, this.NumColumns - 1);
        }
        public void DecrementNumRows()
        {
            //Do not remove the row unless there is more than one
            if (this.NumRows <= 1)
                return;

            //Remove all of the students and student placeholders from the last row
            for (int i = this.NumColumns - 1; i >= 0; i--)
                this.DeleteStudentAtIndex(this.NumRows - 1, i);

            this.NumRows--;
        }
        public void DecrementNumColumns()
        {
            //Do not remove the row unless there is more than one
            if (this.NumColumns <= 1)
                return;

            //Remove all of the students and student placeholders from the last row
            for (int i = 0; i < this.NumRows; i++)
                this.DeleteStudentAtIndex(i, this.NumColumns - 1 - i);

            this.NumColumns--;
        }
        /// <summary>
        /// Add a student to the classroom.
        /// </summary>
        /// <param name="student">The student to be added.</param>
        /// <param name="row">Zero-based row index of the student to be added.</param>
        /// <param name="column">Zero-based column index of the student to be added.</param>
        public void AddStudentAtIndex(Student student, int row, int column)
        {
            //Check if the student should be added to the end of the list or in between previous elements
            int index = row * this.NumColumns + column;

            if (index > this.Desks.Count - 1)
                return;

            //Update the properties of the student
            student.IsInClassroom = true;
            student.RowIndex = row;
            student.ColumnIndex = column;

            //Remove the student from the disjoint students list
            this.DisjointStudents.Remove(student);
            if (this.DisjointStudents.Count == 0) this.HasDisjointStudents = false;

            if (index > this.Desks.Count - 1)
            {                
                this.Desks.Add(student);
            }   
            else
            {
                //Delete the placeholder that is currently there
                this.Desks.RemoveAt(index);

                //Add the new student to that index
                this.Desks.Insert(index, student);
            }
                
        }
        /// <summary>
        /// Add a student placeholder to the classroom.
        /// </summary>
        /// <param name="row">Zero-based row index of the placeholder.</param>
        /// <param name="column">Zero-based column index of the placeholder.</param>
        public void AddStudentPlaceholderAtIndex(int row, int column)
        {
            //Check if the placeholder should be added to the end of the list or in between previous elements
            int index = row * this.NumColumns + column;
            if (index > this.Desks.Count - 1)
                this.Desks.Add(new StudentPlaceholder(this, row, column));
            else
                this.Desks.Insert(index, new StudentPlaceholder(this, row, column));
        }
        /// <summary>
        /// Delete a student or student placeholder from the classroom
        /// </summary>
        /// <param name="row">Zero-based row index of the student.</param>
        /// <param name="column">Zero-based column index of the student.</param>
        public void DeleteStudentAtIndex(int row, int column)
        {
            int index = row * this.NumColumns + column;
            if (index > -1 && index < this.Desks.Count)
            {
                //Check to see if the element at this index is a student, if so then add it to the disjoint students list
                if (this.Desks[index] is Student)
                {
                    this.DisjointStudents.Add(this.Desks[index] as Student);
                    this.HasDisjointStudents = true;
                }
                    

                this.Desks.RemoveAt(index);
            }    
        }
        /// <summary>
        /// Refreshes the list of students that are not in the classroom. Useful to call whenever a student is newly added to the roster.
        /// </summary>
        public void RefreshDisjointStudents()
        {
            foreach (Student student in this.ParentClass.Students)
                if (!student.IsInClassroom)
                    if (!this.DisjointStudents.Contains(student))
                    {
                        this.DisjointStudents.Add(student);
                        this.HasDisjointStudents = true;
                    }
                        
        }

        //Public Commands
        public IncrementClassStructColumns_Command IncrementColumns { get; set; }
        public IncrementClassStructRows_Command IncrementRows { get; set; }
        public DecrementClassStructColumns_Command DecrementColumns { get; set; }
        public DecrementClassStructRows_Command DecrementRows { get; set; }

        //Private Methods
        private int FindMaxRowNum()
        {
            int maxRow = -1;
            for (int i = 0; i < this.ParentClass.Students.Count; i++)
                if (this.ParentClass.Students[i].RowIndex > maxRow)
                    maxRow = this.ParentClass.Students[i].RowIndex;

            if (maxRow < 0)
                return 1;
            return maxRow;
        }
        private int FindMaxColumnNum()
        {
            int maxCol = -1;
            for (int i = 0; i < this.ParentClass.Students.Count; i++)
                if (this.ParentClass.Students[i].ColumnIndex > maxCol)
                    maxCol = this.ParentClass.Students[i].ColumnIndex;

            if (maxCol < 0)
                return 1;
            return maxCol;
        }
        private void InitializeDesks()
        {
            //Fill the entire classroom with placeholders
            for (int i = 0; i < this.NumRows; i++)
                for (int j = 0; j < this.NumColumns; j++)
                    this.AddStudentPlaceholderAtIndex(i, j);

            //Load the students that have previously saved data into the classroom
            foreach (Student student in this.ParentClass.Students)
            {
                if (student.RowIndex > -1 && student.ColumnIndex > -1)
                {
                    //Since they have been placed in the classroom before, change the boolean
                    this.AddStudentAtIndex(student, student.RowIndex, student.ColumnIndex);
                    student.IsInClassroom = true;
                } 
                else
                {
                    //Since they have not been placed in the classroom before, tell the user they need to be placed
                    student.IsInClassroom = false;
                    this.DisjointStudents.Add(student);
                } 
            }
        }
      
        //Private Variables
        private int _NumRows, _NumColumns;
        private bool _HasDisjointStudents;

        //INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    public class IncrementClassStructRows_Command : ICommand
    {
        ClassStructure layout;
        public event EventHandler CanExecuteChanged;
        public IncrementClassStructRows_Command(ClassStructure _Layout) { layout = _Layout; }
        public bool CanExecute(object parameter) { return true; }
        public void Execute(object parameter) { this.layout.IncrementNumRows(); }
    }

    public class IncrementClassStructColumns_Command : ICommand
    {
        ClassStructure layout;
        public event EventHandler CanExecuteChanged;
        public IncrementClassStructColumns_Command(ClassStructure _Layout) { layout = _Layout; }
        public bool CanExecute(object parameter) { return true; }
        public void Execute(object parameter) { this.layout.IncrementNumColumns(); }
    }

    public class DecrementClassStructRows_Command : ICommand
    {
        ClassStructure layout;
        public event EventHandler CanExecuteChanged;
        public DecrementClassStructRows_Command(ClassStructure _Layout) { layout = _Layout; }
        public bool CanExecute(object parameter) { return true; }
        public void Execute(object parameter) { this.layout.DecrementNumRows(); }
    }

    public class DecrementClassStructColumns_Command : ICommand
    {
        ClassStructure layout;
        public event EventHandler CanExecuteChanged;
        public DecrementClassStructColumns_Command(ClassStructure _Layout) { layout = _Layout; }
        public bool CanExecute(object parameter) { return true; }
        public void Execute(object parameter) { this.layout.DecrementNumColumns(); }
    }

    public class StudentPlaceholder : INotifyPropertyChanged
    {
        //Constructors
        public StudentPlaceholder() { }
        public StudentPlaceholder(ClassStructure _ClassStruct, int row, int column)
        {
            this.ParentClassStructure = _ClassStruct;
            this.RowIndex = row;
            this.ColumnIndex = column;
            this.AddStudent = new AddStudentToClassroom_Command(this);
        }

        //Public Methods
        public void AddStudentToClassroom(Student student)
        {
            this.ParentClassStructure.AddStudentAtIndex(student, this.RowIndex, this.ColumnIndex);
        }

        //Public Attributes
        public ClassStructure ParentClassStructure
        {
            get { return _ParentClassStructure; }
            set
            {
                if (value != _ParentClassStructure)
                {
                    _ParentClassStructure = value;
                    OnPropertyChanged("ParentClassStructure");
                }
            }
        }
        public ObservableCollection<Student> DisjointStudents
            { get { return ParentClassStructure.DisjointStudents; } }
        public int RowIndex
        {
            get { return _RowIndex; }
            set
            {
                if (value != _RowIndex)
                {
                    _RowIndex = value;
                    OnPropertyChanged("RowIndex");
                }
            }
        }
        public int ColumnIndex
        {
            get { return _ColumnIndex; }
            set
            {
                if (value != _ColumnIndex)
                {
                    _ColumnIndex = value;
                    OnPropertyChanged("ColumnIndex");
                }
            }
        }
        public Student SelectedDisjointStudent
        {
            get { return _SelectedDisjointStudent; }
            set
            {
                if (value != _SelectedDisjointStudent)
                {
                    _SelectedDisjointStudent = value;
                    OnPropertyChanged("SelectedStudent");
                }
            }
        }

        //Public Commands
        public AddStudentToClassroom_Command AddStudent { get; set; }

        //Private Variables
        private ClassStructure _ParentClassStructure;
        private int _RowIndex, _ColumnIndex;
        private Student _SelectedDisjointStudent;

        //INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    public class AddStudentToClassroom_Command : ICommand
    {
        StudentPlaceholder studentPlaceholder;
        public event EventHandler CanExecuteChanged;
        public AddStudentToClassroom_Command(StudentPlaceholder _studentPlaceholder) { studentPlaceholder = _studentPlaceholder; }
        public bool CanExecute(object parameter) { return true; }
        public void Execute(object parameter)
        {
            if (studentPlaceholder.SelectedDisjointStudent != null)
                studentPlaceholder.AddStudentToClassroom(studentPlaceholder.SelectedDisjointStudent);
        }
    }
}
