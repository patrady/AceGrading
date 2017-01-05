using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;

namespace AceGrading
{
    public class MultipleChoice : Question, QuestionInterface, INotifyPropertyChanged
    {
        public MultipleChoice() { }
        public MultipleChoice(Test _ParentTest)
        {
            this.ParentTest = _ParentTest; //these need to be added to other Question children
            this.TestSection = this.ParentTest.RequiredSection; //these need to be added to other Question children
            Answer = null;
            IncrementOptions = new IncrementMultChoiceOptions_Command(this);
            DecrementOptions = new DecrementMultChoiceOptions_Command(this);
            OptionalAnswers = new ObservableCollection<MultipleChoiceAnswer_UI>();
            AnswersToPickFrom = new ObservableCollection<MultipleChoiceAnswer_UI>();
            InitializeAnswerToPickFrom();
            InitializeOptionalAnswers();
            NumberOptions = this.OptionalAnswers.Count;
        }

        //Class Attributes
        public string Answer
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
        public ObservableCollection<MultipleChoiceAnswer_UI> OptionalAnswers { get; set; }
        public ObservableCollection<MultipleChoiceAnswer_UI> AnswersToPickFrom { get; set; }
        string QuestionInterface.Question_Type()
        {
            return "Multiple Choice";
        }
        public int NumberOptions
        {
            get { return _NumberOptions; }
            set
            {
                if (value != _NumberOptions)
                {
                    _NumberOptions = value;
                    OnPropertyChanged("NumberOptions");
                }
            }
        }

        //Commands
        public IncrementMultChoiceOptions_Command IncrementOptions { get; set; }
        public DecrementMultChoiceOptions_Command DecrementOptions { get; set; }

        //Public Methods
        public void AddToOptionalList()
        {
            int Index = 0;

            //find the index of the first optional answer
            for (int i = 0; i < this.AnswersToPickFrom.Count; i++)
                if (AnswersToPickFrom[i].isStartingOption)
                { Index = i; break; }

            //get the index of the answer to be added answer
            Index = (Index + this.NumberOptions) % this.AnswersToPickFrom.Count;

            //Add to the Optional List
            this.OptionalAnswers.Add(this.AnswersToPickFrom[Index]);
            this.NumberOptions++;
        }
        public void RemoveFromOptionalList()
        {
            this.OptionalAnswers.RemoveAt(this.OptionalAnswers.Count - 1);
            this.NumberOptions--;
        }
        public void ChangeStartingOptionalAnswer(MultipleChoiceAnswer_UI newStart)
        {
            //Get the index of the new starting option
            int index = this.AnswersToPickFrom.IndexOf(newStart);

            //Remove all of the previously added answers
            this.OptionalAnswers.Clear();

            //Add the answers with the new starting option, accomidating for an Z->A change and for the respective number of options selected
            for (int i = 0; i < this.NumberOptions; i++)
                this.OptionalAnswers.Add(this.AnswersToPickFrom[(index + i) % AnswersToPickFrom.Count]);
        }
        public void EnsureOnlyOneStartingOption(MultipleChoiceAnswer_UI correctAnswer)
        {
            //There can only be one starting option, so linear scan through all elements 
            // and deselect any element that was the starting option
            foreach (MultipleChoiceAnswer_UI answer in this.AnswersToPickFrom)
                if (answer.Letter != correctAnswer.Letter && answer.isStartingOption)
                    answer.isStartingOption = false;
        }
        public void EnsureOnlyOneCorrectAnswer(MultipleChoiceAnswer_UI correctAnswer)
        {
            //There can only be one correct answer, so linear scan through all elements 
            // and deselect any element that was previously the correct answer
            foreach (MultipleChoiceAnswer_UI answer in this.AnswersToPickFrom)
                if (answer.Letter != correctAnswer.Letter && answer.isAnswer)
                    answer.isAnswer = false;
        }

        //Private Methods
        private void InitializeAnswerToPickFrom()
        {
            AnswersToPickFrom.Add(new MultipleChoiceAnswer_UI(this) { Letter = 'A', isAnswer = false, isStartingOption = true});
            AnswersToPickFrom.Add(new MultipleChoiceAnswer_UI(this) { Letter = 'B', isAnswer = false, isStartingOption = false});
            AnswersToPickFrom.Add(new MultipleChoiceAnswer_UI(this) { Letter = 'C', isAnswer = false, isStartingOption = false });
            AnswersToPickFrom.Add(new MultipleChoiceAnswer_UI(this) { Letter = 'D', isAnswer = false, isStartingOption = false });
            AnswersToPickFrom.Add(new MultipleChoiceAnswer_UI(this) { Letter = 'E', isAnswer = false, isStartingOption = false });
            AnswersToPickFrom.Add(new MultipleChoiceAnswer_UI(this) { Letter = 'F', isAnswer = false, isStartingOption = false });
            AnswersToPickFrom.Add(new MultipleChoiceAnswer_UI(this) { Letter = 'G', isAnswer = false, isStartingOption = false });
            AnswersToPickFrom.Add(new MultipleChoiceAnswer_UI(this) { Letter = 'H', isAnswer = false, isStartingOption = false });
            AnswersToPickFrom.Add(new MultipleChoiceAnswer_UI(this) { Letter = 'I', isAnswer = false, isStartingOption = false });
            AnswersToPickFrom.Add(new MultipleChoiceAnswer_UI(this) { Letter = 'J', isAnswer = false, isStartingOption = false });
            AnswersToPickFrom.Add(new MultipleChoiceAnswer_UI(this) { Letter = 'K', isAnswer = false, isStartingOption = false });
            AnswersToPickFrom.Add(new MultipleChoiceAnswer_UI(this) { Letter = 'L', isAnswer = false, isStartingOption = false });
            AnswersToPickFrom.Add(new MultipleChoiceAnswer_UI(this) { Letter = 'M', isAnswer = false, isStartingOption = false });
            AnswersToPickFrom.Add(new MultipleChoiceAnswer_UI(this) { Letter = 'N', isAnswer = false, isStartingOption = false });
            AnswersToPickFrom.Add(new MultipleChoiceAnswer_UI(this) { Letter = 'O', isAnswer = false, isStartingOption = false });
            AnswersToPickFrom.Add(new MultipleChoiceAnswer_UI(this) { Letter = 'P', isAnswer = false, isStartingOption = false });
            AnswersToPickFrom.Add(new MultipleChoiceAnswer_UI(this) { Letter = 'Q', isAnswer = false, isStartingOption = false });
            AnswersToPickFrom.Add(new MultipleChoiceAnswer_UI(this) { Letter = 'R', isAnswer = false, isStartingOption = false });
            AnswersToPickFrom.Add(new MultipleChoiceAnswer_UI(this) { Letter = 'S', isAnswer = false, isStartingOption = false });
            AnswersToPickFrom.Add(new MultipleChoiceAnswer_UI(this) { Letter = 'T', isAnswer = false, isStartingOption = false });
            AnswersToPickFrom.Add(new MultipleChoiceAnswer_UI(this) { Letter = 'U', isAnswer = false, isStartingOption = false });
            AnswersToPickFrom.Add(new MultipleChoiceAnswer_UI(this) { Letter = 'V', isAnswer = false, isStartingOption = false });
            AnswersToPickFrom.Add(new MultipleChoiceAnswer_UI(this) { Letter = 'W', isAnswer = false, isStartingOption = false });
            AnswersToPickFrom.Add(new MultipleChoiceAnswer_UI(this) { Letter = 'X', isAnswer = false, isStartingOption = false });
            AnswersToPickFrom.Add(new MultipleChoiceAnswer_UI(this) { Letter = 'Y', isAnswer = false, isStartingOption = false });
            AnswersToPickFrom.Add(new MultipleChoiceAnswer_UI(this) { Letter = 'Z', isAnswer = false, isStartingOption = false });
        }
        private void InitializeOptionalAnswers()
        {
            //Add A->D as the default optional answers
            for (int i = 0; i < 4; i++)
                this.OptionalAnswers.Add(this.AnswersToPickFrom[i]);
        }

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
        private string _Answer;
        private int _NumberOptions;
    }

    public class MultipleChoiceAnswer_UI : INotifyPropertyChanged
    {
        public MultipleChoiceAnswer_UI(MultipleChoice _parentQuestion)
        {
            parentQuestion = _parentQuestion;
        }

        //Public Attributes
        public MultipleChoice parentQuestion { get; set; }
        public char Letter { get; set; }
        public bool isStartingOption
        {
            get { return _isStartingOption; }
            set
            {
                if (value != _isStartingOption)
                {
                    _isStartingOption = value;
                    OnPropertyChanged("isStartingOption");
                    if (_isStartingOption)
                    {
                        parentQuestion.EnsureOnlyOneStartingOption(this);
                        parentQuestion.ChangeStartingOptionalAnswer(this);
                    }
                }
            }
        }
        public bool isAnswer
        {
            get { return _isAnswer; }
            set
            {
                if (value != _isAnswer)
                {
                    _isAnswer = value;
                    OnPropertyChanged("isAnswer");

                    if (_isAnswer)
                        parentQuestion.EnsureOnlyOneCorrectAnswer(this);
                }
            }
        }

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

        //Private variables
        private bool _isAnswer, _isStartingOption;
    }

    public class IncrementMultChoiceOptions_Command : ICommand
    {
        MultipleChoice question;
        public event EventHandler CanExecuteChanged;

        public IncrementMultChoiceOptions_Command(MultipleChoice _Question)
        {
            question = _Question;
        }

        public bool CanExecute(object parameter)
        {
            if (question.NumberOptions > 25)
                return false;
            return true;
        }

        public void Execute(object parameter)
        {
            //Adds a the next letter to the optional list
            this.question.AddToOptionalList();
        }
    }

    public class DecrementMultChoiceOptions_Command : ICommand
    {
        MultipleChoice question;
        public event EventHandler CanExecuteChanged;

        public DecrementMultChoiceOptions_Command(MultipleChoice _Question)
        {
            question = _Question;
        }

        public bool CanExecute(object parameter)
        {
            if (question.NumberOptions < 2)
                return false;
            return true;
        }

        public void Execute(object parameter)
        {
            //Remove the last letter in the correct options dockpanel
            this.question.RemoveFromOptionalList();
        }
    }
}
