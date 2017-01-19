using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace AceGrading
{
    public class TrueFalse : Question, INotifyPropertyChanged
    {
        public TrueFalse(Test _ParentTest)
        {
            this.ParentTest = _ParentTest;
            this.TestSection = this.ParentTest.RequiredSection;
            this.SetAnswer(false);
            this.AnswerIsFalse = false;
            this.AnswerIsTrue = false;
            Answer_If_False = null;
        }

        public TrueFalse()
        {
            this.TestSection = this.ParentTest.RequiredSection;
            this.SetAnswer(false);
            this.AnswerIsFalse = false;
            this.AnswerIsTrue = false;
            Answer_If_False = null;
        }

        //Public Attributes
        public bool Answer
        {
            get { return _Answer; }
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
        public bool AnswerIsTrue
        {
            get { return _AnswerIsTrue; }
            set
            {
                if (value != _AnswerIsTrue)
                {
                    _AnswerIsTrue = value;
                    if (_AnswerIsTrue)
                    {
                        SetAnswer(true);
                        this.AnswerIsFalse = false;
                    }
                    OnPropertyChanged("AnswerIsTrue");
                }
            }
        }
        public bool AnswerIsFalse
        {
            get { return _AnswerIsFalse; }
            set
            {
                if (value != _AnswerIsFalse)
                {
                    _AnswerIsFalse = value;
                    if (_AnswerIsFalse)
                    {
                        SetAnswer(false);
                        this.AnswerIsTrue = false;
                    }
                    OnPropertyChanged("AnswerIsFalse");
                }
            }

        }

        //Public Methods
        /// <summary>
        /// Set the answer of the True/False Question.
        /// </summary>
        /// <param name="Value">The value of the answer.</param>
        public void SetAnswer(bool Value)
        {
            _Answer = Value;
            if (_Answer)
                AnswerIsTrue = true;
            else AnswerIsFalse = true;

        }

        //Private Variables
        private bool _Answer, _AnswerIsTrue, _AnswerIsFalse;
        private string _Answer_If_False;
    }
}
