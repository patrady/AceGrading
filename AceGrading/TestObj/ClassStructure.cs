using System;
using System.ComponentModel;
using System.Windows.Input;

namespace AceGrading
{
    public class ClassStructure : INotifyPropertyChanged
    {
        public ClassStructure()
        {
            this.NumColumns = 1;
            this.NumRows = 1;
            this.IncrementColumns = new IncrementClassStructColumns_Command(this);
            this.IncrementRows = new IncrementClassStructRows_Command(this);
            this.DecrementColumns = new DecrementClassStructColumns_Command(this);
            this.DecrementRows = new DecrementClassStructRows_Command(this);
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

        //Public Methods
        public void IncrementNumRows() { this.NumRows++; }
        public void IncrementNumColumns() { this.NumColumns++; }
        public void DecrementNumRows() { if (this.NumRows > 1) this.NumRows--; }
        public void DecrementNumColumns() { if (this.NumColumns > 1) this.NumColumns--; }

        //Public Commands
        public IncrementClassStructColumns_Command IncrementColumns { get; set; }
        public IncrementClassStructRows_Command IncrementRows { get; set; }
        public DecrementClassStructColumns_Command DecrementColumns { get; set; }
        public DecrementClassStructRows_Command DecrementRows { get; set; }

        //Private Variables
        private int _NumRows, _NumColumns;

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
}
