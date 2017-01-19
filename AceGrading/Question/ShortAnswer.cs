using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace AceGrading
{
    public class ShortAnswer : Question, INotifyPropertyChanged
    {
        public ShortAnswer(Test _ParentTest)
        {
            this.ParentTest = _ParentTest;
            this.TestSection = this.ParentTest.RequiredSection;
            StrictInterpretation = false;
        }

        //Public Attributes
        public string Answer
        {
            get { return _Answer; }
            set
            {
                if (value != _Answer)
                {
                    _Answer = value;
                    OnPropertyChanged("Answer");
                    OnPropertyChanged("WordsRemaining");
                }
            }
        }
        public bool StrictInterpretation
        {
            get { return _StrictInterpretation; }
            set
            {
                if (value != _StrictInterpretation)
                {
                    _StrictInterpretation = value;
                    OnPropertyChanged("StrictInterpretation");
                }
            }
        }
        public int WordsRemaining
        {
            get { return GetWordsRemaining(); }
        }
        public int MaxAnswerLength
        {
            get { return _MaxAnswerLength; }
            set
            {
                if (value != _MaxAnswerLength)
                {
                    _MaxAnswerLength = value;
                    OnPropertyChanged("MaxAnswerLength");
                }
            }
        }

        //Private variables
        private bool _StrictInterpretation;
        private string _Answer;
        private int _MaxAnswerLength;
        
        //Private methods
        private int GetWordsRemaining()
        {
            int maxWords = 20;

            if (Answer == null)
                return maxWords;

            if (Answer.Trim() == "")
                return maxWords;

            int numWords = Answer.TrimStart().Split(' ').Length;

            if (maxWords - numWords == -1)
                MaxAnswerLength = this.Answer.Length;
            else
                MaxAnswerLength = int.MaxValue;

            return maxWords - numWords + 1;
        }
    }
}
