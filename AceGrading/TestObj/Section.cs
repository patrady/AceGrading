using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows.Input;

namespace AceGrading
{
    public class Section : INotifyPropertyChanged
    {
        public Section(Test _ParentTest)
        {
            ParentTest = _ParentTest;
            Questions = new ObservableCollection<Question>();
            Section_Number = 0;
            Required_Questions = 0;
            IncrementRequired = new IncrementSectionRequiredQuestions_Command(this);
            DecrementRequired = new DecrementSectionRequiredQuestions_Command(this);
        }

        public Section()
        {
            ParentTest = ParentTest;
            Questions = new ObservableCollection<Question>();
            Section_Number = 0;
            Required_Questions = 0;
            IncrementRequired = new IncrementSectionRequiredQuestions_Command(this);
            DecrementRequired = new DecrementSectionRequiredQuestions_Command(this);
        }

        //Public Attributes
        public ObservableCollection<Question> Questions { get; set; }
        public int Section_Number
        {
            get { return _Section_Number; }
            set
            {
                if (value != _Section_Number)
                {
                    _Section_Number = value;
                    OnPropertyChanged("Name");
                }
            }
        }
        public int Total_Questions { get { return Questions.Count; } }
        public object Database_ID { get; set; }
        public string Name
        {
            get
            {
                if (this.Section_Number == 0)
                    return "Required Section";
                else return "Section #" + this.Section_Number;
            }
        }
        public int Required_Questions
        {
            get
            {
                if (Section_Number.Equals(0))
                    return this.Total_Questions;
                return required_questions;
            }
            set
            {
                if (value != required_questions)
                {
                    required_questions = value;
                    OnPropertyChanged("Required_Questions");
                }
            }
        }
        public Test ParentTest { get; set; }
        public bool IsRequiredSection { get { if (this.Section_Number == 0) return true;  return false; } }

        //Commands
        public IncrementSectionRequiredQuestions_Command IncrementRequired { get; set; }
        public DecrementSectionRequiredQuestions_Command DecrementRequired { get; set; }

        //Public Methods
        public void AddQuestion(Question question)
        {
            this.Questions.Add(question);
            if (this.IsRequiredSection)
                this.Required_Questions++;
        }
        public void RemoveQuestion(Question question)
        {
            this.Questions.Remove(question);
            if (this.IsRequiredSection)
                this.Required_Questions--;
        }
        public void IncrementRequiredQuestions()
        {
            this.Required_Questions++;
        }
        public void DecrementRequiredQuestions()
        {
            this.Required_Questions--;
        }

        //Private variables
        private int required_questions, _Section_Number;

        //INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    public class IncrementSectionRequiredQuestions_Command : ICommand
    {
        Section section;
        public event EventHandler CanExecuteChanged;
        public IncrementSectionRequiredQuestions_Command(Section _Section) { section = _Section; }
        public bool CanExecute(object parameter) { return true; }

        public void Execute(object parameter)
        {
            this.section.IncrementRequiredQuestions();
        }
    }

    public class DecrementSectionRequiredQuestions_Command : ICommand
    {
        Section section;
        public event EventHandler CanExecuteChanged;
        public DecrementSectionRequiredQuestions_Command(Section _Section) { section = _Section; }
        public bool CanExecute(object parameter) { return true; }
        public void Execute(object parameter)
        {
            if (this.section.Required_Questions > 0)
                this.section.DecrementRequiredQuestions();
        }
    }


}
