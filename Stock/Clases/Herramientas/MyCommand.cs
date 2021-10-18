using System;
using System.Windows.Input;

namespace Stock.Clases.Herramientas
{
    public class MyCommand : ICommand
    {
        public event EventHandler CanExecuteChanged;

        private Action action = null;
        private Func<bool> canExecute = null;

        public MyCommand(Action action)
        {
            this.action = action;
        }

        public MyCommand(Action action, Func<bool> canexecute) : this(action)
        {
            this.canExecute = canexecute;
        }

        public bool CanExecute(object parameter)
        {            
            return canExecute();
        }

        public void Execute(object parameter)
        {
            action();
        }

        public void RaisCanExecuteChange()
        {
            CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        }
    }
}