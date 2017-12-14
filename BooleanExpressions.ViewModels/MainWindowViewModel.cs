using BooleanExpressions.Models;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace BooleanExpressions.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        public MainWindowViewModel()
        {
            InitCommands();

            Atoms = new ObservableCollection<Atom>();
            SelectedAtom = null;
            SetEnable(true, true, false, false, true, false);
            LBracketCount = RBracketCount = 0;

            BitRow1 = BitRow2 = BitwiseResult = "";
        }

        public void ShowResult()
        {
            Result = _expression.GetResult();
        }
        public void ShowTruthTable()
        {
            TruthTable = "";
            bool[,] table = _expression.GetTruthTable();
            List<Atom> atoms = _expression.GetAtoms();

            foreach (var atom in atoms)
            {
                TruthTable += atom.Name + "\t";
            }

            TruthTable += "Результат";

            for (int i = 0; i < table.GetUpperBound(0) - table.GetLowerBound(0) + 1; i++)
            {
                TruthTable += "\n";

                for (int j = 0; j < table.GetUpperBound(1) - table.GetLowerBound(1) + 1; j++)
                {
                    TruthTable += table[i, j] + "\t";
                }
            }
        }

        public Expression Expression
        {
            get
            {
                return _expression;
            }
            private set
            {
                _expression = value;
                OnPropertyChanged("Expression");
            }
        }
        public ObservableCollection<Atom> Atoms
        {
            get
            {
                return _atoms;
            }
            set
            {
                _atoms = value;
                OnPropertyChanged("Atoms");
            }
        }
        public Atom SelectedAtom
        {
            get
            {
                return _selectedAtom;
            }
            set
            {
                _selectedAtom = value;
                OnPropertyChanged("SelectedAtom");
            }
        }
        public bool Result
        {
            get { return _result; }
            set
            {
                _result = value;
                OnPropertyChanged("Result");
            }
        }
        public string TruthTable
        {
            get { return _truthTable; }
            set
            {
                _truthTable = value;
                OnPropertyChanged("TruthTable");
            }
        }

        public string BitRow1
        {
            get { return _bitRow1; }
            set
            {
                if (!value.Any(s => s != '0' && s != '1'))
                    _bitRow1 = value;

                OnPropertyChanged("BitRow1");
            }
        }
        public string BitRow2
        {
            get { return _bitRow2; }
            set
            {
                if (!value.Any(s => s != '0' && s != '1'))
                    _bitRow2 = value;

                OnPropertyChanged("BitRow2");
            }
        }
        public string BitwiseResult
        {
            get { return _bitwiseResult; }
            set
            {
                _bitwiseResult = value;
                OnPropertyChanged("BitwiseResult");
            }
        }

        public void SetEnable(bool atom, bool not, bool operand, bool remove, bool lBracket, bool rBracket)
        {
            if (rBracket == true)
            {
                rBracket = LBracketCount > RBracketCount;
            }

            AtomEnabled = atom;
            NotEnabled = not;
            OperatorEnabled = operand;
            RemoveEnabled = remove;
            LBracketEnabled = lBracket;
            RBracketEnabled = rBracket;

            savedEnables.Push(atom);
            savedEnables.Push(not);
            savedEnables.Push(operand);
            savedEnables.Push(remove);
            savedEnables.Push(lBracket);
            savedEnables.Push(rBracket);

            ResultEnabled = IsCorrectExpression();
        }
        public void RestoreEnable()
        {
            if (savedEnables.Count > 5)
            {
                savedEnables.Pop();
                savedEnables.Pop();
                savedEnables.Pop();
                savedEnables.Pop();
                savedEnables.Pop();
                savedEnables.Pop();

                RBracketEnabled = savedEnables.Pop();
                LBracketEnabled = savedEnables.Pop();
                RemoveEnabled = savedEnables.Pop();
                OperatorEnabled = savedEnables.Pop();
                NotEnabled = savedEnables.Pop();
                AtomEnabled = savedEnables.Pop();

                savedEnables.Push(AtomEnabled);
                savedEnables.Push(NotEnabled);
                savedEnables.Push(OperatorEnabled);
                savedEnables.Push(RemoveEnabled);
                savedEnables.Push(LBracketEnabled);
                savedEnables.Push(RBracketEnabled);

                ResultEnabled = IsCorrectExpression();
            }
        }
        public void ClearSavedEnable()
        {
            LBracketCount = RBracketCount = 0;
            savedEnables.Clear();
            ResultEnabled = IsCorrectExpression();
        }

        public bool AtomEnabled
        {
            get { return _atomEnabled; }
            set
            {
                _atomEnabled = value;
                OnPropertyChanged("AtomEnabled");
            }
        }
        public bool NotEnabled
        {
            get { return _notEnabled; }
            set
            {
                _notEnabled = value;
                OnPropertyChanged("NotEnabled");
            }
        }
        public bool OperatorEnabled
        {
            get { return _operatorEnabled; }
            set
            {
                _operatorEnabled = value;
                OnPropertyChanged("OperatorEnabled");
            }
        }
        public bool ClearEnabled
        {
            get { return _clearEnabled; }
            set
            {
                _clearEnabled = value;
                OnPropertyChanged("ClearEnabled");
            }
        }
        public bool RemoveEnabled
        {
            get { return _removeEnabled; }
            set
            {
                _removeEnabled = value;
                OnPropertyChanged("RemoveEnabled");
            }
        }
        public bool LBracketEnabled
        {
            get { return _lBracketEnabled; }
            set
            {
                _lBracketEnabled = value;
                OnPropertyChanged("LBracketEnabled");
            }
        }
        public bool RBracketEnabled
        {
            get { return _rBracketEnabled; }
            set
            {
                _rBracketEnabled = value;
                OnPropertyChanged("RBracketEnabled");
            }
        }
        public bool ResultEnabled
        {
            get { return _resultEnabled; }
            set
            {
                _resultEnabled = value;
                OnPropertyChanged("ResultEnabled");
            }
        }

        public int LBracketCount { get; set; }
        public int RBracketCount { get; set; }

        private void InitCommands()
        {
            AtomCommand = new AtomCommand(this);
            NotCommand = new NotCommand(this);
            AndCommand = new AndCommand(this);
            OrCommand = new OrCommand(this);
            ImpCommand = new ImpCommand(this);
            EqualsCommand = new EqualsCommand(this);
            LBracketCommand = new LBracketCommand(this);
            RBracketCommand = new RBracketCommand(this);
            RemoveLastCommand = new RemoveLastCommand(this);
            ClearCommand = new ClearCommand(this);
            ResultCommand = new ResultCommand(this);

            BitwiseAndCommand = new BitwiseAndCommand(this);
            BitwiseOrCommand = new BitwiseOrCommand(this);
            BitwiseXorCommand = new BitwiseXorCommand(this);
        }
        private bool IsCorrectExpression()
        {
            List<Symbol> symbolList = Expression.GetSymbolListCopy();
            Symbol lastSymbol = symbolList.LastOrDefault();

            if (LBracketCount != RBracketCount || symbolList.Count < 1 || 
                (lastSymbol.SymbolType == SymbolType.Operation &&
                 lastSymbol.OperationType != OperationType.RBracket))
            {
                return false;
            }

            return true;
        }

        private ObservableCollection<Atom> _atoms;
        private Atom _selectedAtom;
        private Expression _expression = new Expression();
        private bool _result;
        private string _truthTable;

        private string _bitRow1;
        private string _bitRow2;
        private string _bitwiseResult;

        private Stack<bool> savedEnables = new Stack<bool>();

        private bool _atomEnabled;
        private bool _operatorEnabled;
        private bool _clearEnabled;
        private bool _removeEnabled;
        private bool _lBracketEnabled;
        private bool _rBracketEnabled;
        private bool _notEnabled;
        private bool _resultEnabled;

        #region Commands
        public AtomCommand AtomCommand { get; private set; }
        public NotCommand NotCommand { get; private set; }
        public AndCommand AndCommand { get; private set; }
        public OrCommand OrCommand { get; private set; }
        public ImpCommand ImpCommand { get; private set; }
        public EqualsCommand EqualsCommand { get; private set; }
        public LBracketCommand LBracketCommand { get; private set; }
        public RBracketCommand RBracketCommand { get; private set; }
        public RemoveLastCommand RemoveLastCommand { get; private set; }
        public ClearCommand ClearCommand { get; private set; }
        public ResultCommand ResultCommand { get; private set; }

        public BitwiseAndCommand BitwiseAndCommand { get; private set; }
        public BitwiseOrCommand BitwiseOrCommand { get; private set; }
        public BitwiseXorCommand BitwiseXorCommand { get; private set; }
        #endregion
    }
}