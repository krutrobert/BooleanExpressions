namespace BooleanExpressions.Models
{
    public class Symbol
    {
        public Symbol(OperationType operationType)
        {
            SymbolType = SymbolType.Operation;
            OperationType = operationType;
        }
        public Symbol(Atom atom)
        {
            SymbolType = SymbolType.Atom;
            Atom = atom;
        }

        public bool IsAtom()
        {
            return SymbolType == SymbolType.Atom;
        }

        public SymbolType SymbolType { get; private set; }
        public OperationType OperationType { get; private set; }
        public Atom Atom { get; private set; }
    }
}