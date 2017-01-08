using Microsoft.Win32;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Windows.Input;

namespace AceGrading
{
    public class DeleteStudent_Command : ICommand
    {
        public Class thisClass { get; set; }
        public event EventHandler CanExecuteChanged;

        public DeleteStudent_Command(Class _Class)
        {
            this.thisClass = _Class;
        }

        public bool CanExecute(object parameter) { return true; }

        public void Execute(object parameter)
        {
            thisClass.Delete_Student(thisClass.SelectedStudent);
        }
    }

    public class AddStudent_Command : ICommand
    {
        public Class thisClass;
        public event EventHandler CanExecuteChanged;

        public AddStudent_Command(Class _Class)
        {
            this.thisClass = _Class;
        }

        public bool CanExecute(object parameter) { return true; }

        public void Execute(object parameter)
        {
            if (this.thisClass.NewStudent.StudentName != this.thisClass.NewStudent.DefaultName)
            {
                thisClass.Add_Student(thisClass.NewStudent);
            }

            //Reset to normal
            thisClass.NewStudent.StudentName = "";
        }
    }

    public class DeleteClass_Command : ICommand
    {
        public MainViewModel allClasses;
        public event EventHandler CanExecuteChanged;

        public DeleteClass_Command(MainViewModel _VM)
        {
            allClasses = _VM;
        }

        public bool CanExecute(object parameter) { return true; }

        public void Execute(object parameter)
        {
            if (allClasses.IsClassInList(allClasses.SelectedClass))
            {
                allClasses.Delete_Class(allClasses.SelectedClass);
                allClasses.ChangeSelectedClass();
            }
                
        }
    }

    public class AddClass_Command : ICommand
    {
        MainViewModel MainVM;
        public event EventHandler CanExecuteChanged;

        public AddClass_Command(MainViewModel _MainVM)
        {
            MainVM = _MainVM;
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            if (!MainVM.IsClassInList(MainVM.SelectedClass))
                MainVM.Add_Class(MainVM.SelectedClass);
        }
    }

    public class PreAddClass_Command : ICommand
    {
        MainViewModel MainVM;
        public event EventHandler CanExecuteChanged;

        public PreAddClass_Command(MainViewModel _MainVM)
        {
            MainVM = _MainVM;
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            this.MainVM.NewClass = new Class();
            this.MainVM.SelectedClass = this.MainVM.NewClass;
        }
    }

    public class Browse_Command : ICommand
    {
        Test test;
        public event EventHandler CanExecuteChanged;

        public Browse_Command(Test _Test)
        {
            test = _Test;
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            OpenFileDialog browse = new OpenFileDialog();
            browse.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            browse.ShowDialog();
            try
            {
                if (browse.FileName == "")
                    return;
                test.Upload_File_Name = browse.FileName;
                Stream openFile = browse.OpenFile();
            }
            catch { }
        }
    }

    public class OpenFile_Command : ICommand
    {
        Test test;
        public event EventHandler CanExecuteChanged;

        public OpenFile_Command(Test _Test)
        {
            test = _Test;
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            try
            {
                Process.Start(test.Upload_File_Name);
            }
            catch { }
        }
    }

    public class PreAddTest_Command : ICommand
    {
        Class parentClass;
        public event EventHandler CanExecuteChanged;

        public PreAddTest_Command(Class _Class)
        {
            parentClass = _Class;
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            this.parentClass.NewTest = new Test();
            this.parentClass.SelectedTest = this.parentClass.NewTest;
        }
    }

    public class AddTest_Command : ICommand
    {
        Class parentClass;
        public event EventHandler CanExecuteChanged;

        public AddTest_Command(Class _Class)
        {
            parentClass = _Class;
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            if (!parentClass.IsTestInList(parentClass.SelectedTest))
                parentClass.Add_Test(parentClass.SelectedTest);
        }
    }

    public class DeleteTest_Command : ICommand
    {
        Class parentClass;
        public event EventHandler CanExecuteChanged;

        public DeleteTest_Command(Class _Class)
        {
            parentClass = _Class;
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            if (parentClass.IsTestInList(parentClass.SelectedTest))
            {
                parentClass.Delete_Test(parentClass.SelectedTest);
                parentClass.ChangeSelectedTest();
            }
                
        }
    }

}
