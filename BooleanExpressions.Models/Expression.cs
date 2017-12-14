using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace BooleanExpressions.Models
{
    public class Expression : INotifyPropertyChanged
    {
        public Expression()
        {
            _symbolList = new List<Symbol>();
            _postfixList = new List<Symbol>();
        }

        public void AddAtom(Atom a)
        {
            _symbolList.Add(new Symbol(a));
            UpdateProperties();
        }
        public void AddOperation(OperationType o)
        {
            _symbolList.Add(new Symbol(o));
            UpdateProperties();
        }
        public void RemoveLast()
        {
            if(_symbolList.Count > 0)
            {
                _symbolList.RemoveAt(_symbolList.Count - 1);
                UpdateProperties();
            }
        }
        public void Clear()
        {
            _symbolList.Clear();
            UpdateProperties();
        }
        public List<Atom> GetAtoms()
        {
            List<Atom> atoms = new List<Atom>();

            foreach (var s in _symbolList)
            {
                if (s.IsAtom() && !atoms.Exists(item => item.Name == s.Atom.Name))
                {
                    atoms.Add(s.Atom);
                }
            }

            return atoms;
        }
        public List<Symbol> GetSymbolListCopy()
        {
            List<Symbol> symbols = new List<Symbol>();

            foreach (var s in _symbolList)
            {
                Symbol newSymbol;

                if (s.IsAtom())
                {
                    newSymbol = new Symbol(new Atom()
                    {
                        Name = s.Atom.Name,
                        Value = s.Atom.Value,
                        Description = s.Atom.Description
                    });
                }
                else
                {
                    newSymbol = new Symbol(s.OperationType);
                }

                symbols.Add(newSymbol);
            }

            return symbols;
        }
        public bool GetResult()
        {
            _postfixList = GetPostfix();
            string str = GetStringFromSymbolList(_postfixList);
            Stack<bool> calculationStack = new Stack<bool>();

            foreach (var s in _postfixList)
            {
                if (s.IsAtom())
                {
                    calculationStack.Push(s.Atom.Value);
                }
                else if (calculationStack.Count > 0)
                {
                    if (s.OperationType == OperationType.Not)
                    {
                        bool left = !calculationStack.Pop();
                        calculationStack.Push(left);
                    }
                    else if (s.OperationType == OperationType.And)
                    {
                        bool right = calculationStack.Pop();
                        bool left = calculationStack.Pop();
                        calculationStack.Push(left && right);
                    }
                    else if (s.OperationType == OperationType.Or)
                    {
                        bool right = calculationStack.Pop();
                        bool left = calculationStack.Pop();
                        calculationStack.Push(left || right);
                    }
                    else if (s.OperationType == OperationType.Equals)
                    {
                        bool right = calculationStack.Pop();
                        bool left = calculationStack.Pop();
                        calculationStack.Push(left == right);
                    }
                    else if (s.OperationType == OperationType.Imp)
                    {
                        bool right = calculationStack.Pop();
                        bool left = !calculationStack.Pop();
                        calculationStack.Push(left || right);
                    }
                }
            }

            bool result = calculationStack.Pop();
            return result;
        }
        public bool[,] GetTruthTable()
        {
            List<Atom> atoms = GetAtoms();

            int columnCount = atoms.Count + 1;
            int rowCount = (int)Math.Pow(2, atoms.Count);
            bool[,] table = new bool[rowCount, columnCount];

            int divisor = 1;
                        
            for (int j = 0; j < columnCount - 1; j++)
            {
                divisor *= 2;
                int counter = 1;
                bool cellValue = true;

                for (int i = 0; i < rowCount; i++)
                {
                    if (counter > rowCount / divisor)
                    {
                        counter = 1;
                        cellValue = !cellValue;
                    }

                    table[i, j] = cellValue;
                    counter++;
                }
            }

            bool[] savedValues = new bool[atoms.Count];

            for (int i = 0; i < atoms.Count; i++)
                savedValues[i] = atoms[i].Value;

            for (int i = 0; i < rowCount; i++)
            {
                for (int j = 0; j < atoms.Count; j++)
                {
                    atoms[j].Value = table[i,j];
                }

                table[i, columnCount - 1] = GetResult();
            }

            for (int i = 0; i < atoms.Count; i++)
                atoms[i].Value = savedValues[i];

            return table;
        }
        public string GetStringFromSymbolList(List<Symbol> list)
        {
            string exp = String.Empty;

            foreach (var i in list)
            {
                if (i.IsAtom())
                {
                    exp += i.Atom.Name + " ";
                }
                else
                {
                    switch (i.OperationType)
                    {
                        case OperationType.Not:
                            exp += "\u00AC ";
                            break;
                        case OperationType.And:
                            exp += "\u2227 ";
                            break;
                        case OperationType.Or:
                            exp += "\u2228 ";
                            break;
                        case OperationType.Imp:
                            exp += "\u2192 ";
                            break;
                        case OperationType.Equals:
                            exp += "== ";
                            break;
                        case OperationType.LBracket:
                            exp += "( ";
                            break;
                        case OperationType.RBracket:
                            exp += ") ";
                            break;
                    }
                }
            }

            return exp;
        }
        public override string ToString()
        {
            string exp = String.Empty;

            foreach(var i in _symbolList)
            {
                if (i.IsAtom())
                {
                    exp += i.Atom.Name + " ";
                }
                else
                {
                    switch (i.OperationType)
                    {
                        case OperationType.Not:
                            exp += "\u00AC ";
                            break;
                        case OperationType.And:
                            exp += "\u2227 ";
                            break;
                        case OperationType.Or:
                            exp += "\u2228 ";
                            break;
                        case OperationType.Imp:
                            exp += "\u2192 ";
                            break;
                        case OperationType.Equals:
                            exp += "== ";
                            break;
                        case OperationType.LBracket:
                            exp += "( ";
                            break;
                        case OperationType.RBracket:
                            exp += ") ";
                            break;
                    }
                }
            }

            return exp;
        }

        public string ExpressionText
        {
            get { return _expressionText; }
            private set
            {
                _expressionText = value;
                OnPropertyChanged("ExpressionText");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private List<Symbol> GetPostfix()
        {
            _postfixList = new List<Symbol>();
            Stack<Symbol> operatorStack = new Stack<Symbol>();

            foreach (var s in _symbolList)
            {
                ApplyRules(s, operatorStack);
            }

            while (operatorStack.Count > 0)
            {
                _postfixList.Add(operatorStack.Pop());
            }

            return _postfixList;
        }
        private void ApplyRules(Symbol s, Stack<Symbol> operatorStack)
        {
            if (s.IsAtom())
            {
                _postfixList.Add(s);
            }
            else
            {
                if (s.OperationType == OperationType.LBracket)
                {
                    operatorStack.Push(s);
                }
                else if (s.OperationType == OperationType.RBracket)
                {
                    Symbol current;

                    do
                    {
                        current = operatorStack.Pop();

                        if (current.OperationType != OperationType.LBracket)
                        {
                            _postfixList.Add(current);
                        }
                    }
                    while (current.OperationType != OperationType.LBracket);
                }
                else if (operatorStack.Count == 0 || operatorStack.Peek().OperationType == OperationType.LBracket)
                {
                    operatorStack.Push(s);
                }
                else if (operatorStack.Peek().OperationType == s.OperationType)
                {
                    _postfixList.Add(operatorStack.Pop());
                    ApplyRules(s, operatorStack);
                }
                else if (s.OperationType == OperationType.Not)
                {
                    operatorStack.Push(s);
                }
                else if (operatorStack.Peek().OperationType != OperationType.Not &&
                         (s.OperationType == OperationType.And))
                {
                    operatorStack.Push(s);
                }
                else if (operatorStack.Peek().OperationType != OperationType.Not &&
                         operatorStack.Peek().OperationType != OperationType.And &&
                         (s.OperationType == OperationType.Or))
                {
                    operatorStack.Push(s);
                }
                else if (operatorStack.Peek().OperationType != OperationType.Not &&
                         operatorStack.Peek().OperationType != OperationType.And &&
                         operatorStack.Peek().OperationType != OperationType.Or &&
                         (s.OperationType == OperationType.Imp))
                {
                    operatorStack.Push(s);
                }
                else
                {
                    _postfixList.Add(operatorStack.Pop());
                    ApplyRules(s, operatorStack);
                }
            }
        }
        private void UpdateProperties()
        {
            ExpressionText = ToString();
        }

        private List<Symbol> _symbolList;
        private List<Symbol> _postfixList;
        private string _expressionText;
    }
}