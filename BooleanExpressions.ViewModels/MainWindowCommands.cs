using BooleanExpressions.Models;
using System;
using System.Linq;
using System.Windows.Input;
using System.Numerics;

namespace BooleanExpressions.ViewModels
{
    public class AtomCommand : ICommand
    {
        public AtomCommand(MainWindowViewModel viewModel)
        {
            this.ViewModel = viewModel;
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }
        public void Execute(object parameter)
        {
            if(ViewModel.SelectedAtom != null)
            {
                ViewModel.Expression.AddAtom(ViewModel.SelectedAtom);
                ViewModel.SetEnable(false, false, true, true, false, true);
            }
        }

        public MainWindowViewModel ViewModel { get; set; }
        public event EventHandler CanExecuteChanged;
    }
    public class NotCommand : ICommand
    {
        public NotCommand(MainWindowViewModel viewModel)
        {
            this.ViewModel = viewModel;
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }
        public void Execute(object parameter)
        {
            ViewModel.Expression.AddOperation(OperationType.Not);
            ViewModel.SetEnable(true, true, false, true, true, false);
        }

        public MainWindowViewModel ViewModel { get; set; }
        public event EventHandler CanExecuteChanged;
    }
    public class AndCommand : ICommand
    {
        public AndCommand(MainWindowViewModel viewModel)
        {
            this.ViewModel = viewModel;
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }
        public void Execute(object parameter)
        {
            ViewModel.Expression.AddOperation(OperationType.And);
            ViewModel.SetEnable(true, true, false, true, true, false);
        }

        public MainWindowViewModel ViewModel { get; set; }
        public event EventHandler CanExecuteChanged;
    }
    public class OrCommand : ICommand
    {
        public OrCommand(MainWindowViewModel viewModel)
        {
            this.ViewModel = viewModel;
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }
        public void Execute(object parameter)
        {
            ViewModel.Expression.AddOperation(OperationType.Or);
            ViewModel.SetEnable(true, true, false, true, true, false);
        }

        public MainWindowViewModel ViewModel { get; set; }
        public event EventHandler CanExecuteChanged;
    }
    public class ImpCommand : ICommand
    {
        public ImpCommand(MainWindowViewModel viewModel)
        {
            this.ViewModel = viewModel;
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }
        public void Execute(object parameter)
        {
            ViewModel.Expression.AddOperation(OperationType.Imp);
            ViewModel.SetEnable(true, true, false, true, true, false);
        }

        public MainWindowViewModel ViewModel { get; set; }
        public event EventHandler CanExecuteChanged;
    }
    public class EqualsCommand : ICommand
    {
        public EqualsCommand(MainWindowViewModel viewModel)
        {
            this.ViewModel = viewModel;
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }
        public void Execute(object parameter)
        {
            ViewModel.Expression.AddOperation(OperationType.Equals);
            ViewModel.SetEnable(true, true, false, true, true, false);
        }

        public MainWindowViewModel ViewModel { get; set; }
        public event EventHandler CanExecuteChanged;
    }
    public class LBracketCommand : ICommand
    {
        public LBracketCommand(MainWindowViewModel viewModel)
        {
            this.ViewModel = viewModel;
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }
        public void Execute(object parameter)
        {
            ViewModel.Expression.AddOperation(OperationType.LBracket);
            ViewModel.LBracketCount++;
            ViewModel.SetEnable(true, true, false, true, true, false);
        }

        public MainWindowViewModel ViewModel { get; set; }
        public event EventHandler CanExecuteChanged;
    }
    public class RBracketCommand : ICommand
    {
        public RBracketCommand(MainWindowViewModel viewModel)
        {
            this.ViewModel = viewModel;
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }
        public void Execute(object parameter)
        {
            ViewModel.Expression.AddOperation(OperationType.RBracket);
            ViewModel.RBracketCount++;
            ViewModel.SetEnable(false, false, true, true, false, true);
        }

        public MainWindowViewModel ViewModel { get; set; }
        public event EventHandler CanExecuteChanged;
    }
    public class RemoveLastCommand : ICommand
    {
        public RemoveLastCommand(MainWindowViewModel viewModel)
        {
            this.ViewModel = viewModel;
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }
        public void Execute(object parameter)
        {
            if (ViewModel.Expression.ExpressionText.EndsWith("( "))
            {
                ViewModel.LBracketCount--;
            }
            else if (ViewModel.Expression.ExpressionText.EndsWith(") "))
            {
                ViewModel.RBracketCount--;
            }

            ViewModel.Expression.RemoveLast();
            ViewModel.RestoreEnable();
        }

        public MainWindowViewModel ViewModel { get; set; }
        public event EventHandler CanExecuteChanged;
    }
    public class ClearCommand : ICommand
    {
        public ClearCommand(MainWindowViewModel viewModel)
        {
            this.ViewModel = viewModel;
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }
        public void Execute(object parameter)
        {
            ViewModel.Expression.Clear();
            ViewModel.ClearSavedEnable();
            ViewModel.SetEnable(true, true, false, false, true, false);
        }

        public MainWindowViewModel ViewModel { get; set; }
        public event EventHandler CanExecuteChanged;
    }
    public class ResultCommand : ICommand
    {
        public ResultCommand(MainWindowViewModel viewModel)
        {
            this.ViewModel = viewModel;
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }
        public void Execute(object parameter)
        {
            ViewModel.ShowResult();
            ViewModel.ShowTruthTable();
        }

        public MainWindowViewModel ViewModel { get; set; }
        public event EventHandler CanExecuteChanged;
    }

    public class BitwiseAndCommand : ICommand
    {
        public BitwiseAndCommand(MainWindowViewModel viewModel)
        {
            this.ViewModel = viewModel;
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }
        public void Execute(object parameter)
        {
            if (ViewModel.BitRow1 == "")
            {
                ViewModel.BitRow1 = "0";
            }
            if (ViewModel.BitRow2 == "")
            {
                ViewModel.BitRow2 = "0";
            }

            int num1 = Convert.ToInt32(ViewModel.BitRow1, 2);
            int num2 = Convert.ToInt32(ViewModel.BitRow2, 2);

            ViewModel.BitwiseResult = Convert.ToString(num1 & num2, 2);
        }

        public MainWindowViewModel ViewModel { get; set; }
        public event EventHandler CanExecuteChanged;
    }
    public class BitwiseOrCommand : ICommand
    {
        public BitwiseOrCommand(MainWindowViewModel viewModel)
        {
            this.ViewModel = viewModel;
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }
        public void Execute(object parameter)
        {
            if (ViewModel.BitRow1 == "")
            {
                ViewModel.BitRow1 = "0";
            }
            if (ViewModel.BitRow2 == "")
            {
                ViewModel.BitRow2 = "0";
            }

            int num1 = Convert.ToInt32(ViewModel.BitRow1, 2);
            int num2 = Convert.ToInt32(ViewModel.BitRow2, 2);

            ViewModel.BitwiseResult = Convert.ToString(num1 | num2, 2);
        }

        public MainWindowViewModel ViewModel { get; set; }
        public event EventHandler CanExecuteChanged;
    }
    public class BitwiseXorCommand : ICommand
    {
        public BitwiseXorCommand(MainWindowViewModel viewModel)
        {
            this.ViewModel = viewModel;
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }
        public void Execute(object parameter)
        {
            if (ViewModel.BitRow1 == "")
            {
                ViewModel.BitRow1 = "0";
            }
            if (ViewModel.BitRow2 == "")
            {
                ViewModel.BitRow2 = "0";
            }

            int num1 = Convert.ToInt32(ViewModel.BitRow1, 2);
            int num2 = Convert.ToInt32(ViewModel.BitRow2, 2);

            ViewModel.BitwiseResult = Convert.ToString(num1 ^ num2, 2);
        }

        public MainWindowViewModel ViewModel { get; set; }
        public event EventHandler CanExecuteChanged;
    }
}