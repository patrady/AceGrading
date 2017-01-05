using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace AceGrading
{
    public class TrueFalse : Question, QuestionInterface , INotifyPropertyChanged
    {
        public TrueFalse(Test _ParentTest)
        {
            this.ParentTest = _ParentTest;
            this.TestSection = this.ParentTest.RequiredSection;
            Answer = false;
            Answer_If_False = null;
        }

        //Public Attributes
        public bool Answer
        {
            get { return _Answer; }
            set
            {
                if (value != _Answer)
                {
                    _Answer = value;
                    OnPropertyChanged("Answer");
                }
            }
        }
        public string Answer_If_False
        {
            get { return _Answer_If_False; }
            set
            {
                if (value != _Answer_If_False)
                {
                    _Answer_If_False = value;
                    OnPropertyChanged("Answer_If_False");
                }
            }
        }

        //Private Methods
        string QuestionInterface.Question_Type()
        {
            return "True / False";
        }

        //Private Variables
        private bool _Answer;
        private string _Answer_If_False;

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
