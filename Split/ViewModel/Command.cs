using System;
using System.Windows.Input;
using System.Windows.Threading;

namespace Split.ViewModel
{
    public class Command : ICommand
    {
        private Action methodToExecute = null;
        private Func<bool> methodToDetectCanExecute = null;
        private DispatcherTimer canExecuteChangedEventTimer = null;
        private Command definePDF;
        private Func<bool> p;

        public event EventHandler CanExecuteChanged;

        public Command(Action methodToExecute, Func<bool> methodToDetectCanExecute)
        {
            this.methodToExecute = methodToExecute;
            this.methodToDetectCanExecute = methodToDetectCanExecute;
            this.canExecuteChangedEventTimer = new DispatcherTimer();
            this.canExecuteChangedEventTimer.Tick += CanExecuteChangedEventTimer_Tick;
            this.canExecuteChangedEventTimer.Interval = new TimeSpan(0, 0, 0, 0, 1);
            this.canExecuteChangedEventTimer.Start();
        }

        public Command(Command definePDF, Func<bool> p)
        {
            this.definePDF = definePDF;
            this.p = p;
        }

        private void CanExecuteChangedEventTimer_Tick(object sender, EventArgs e)
        {
            if (this.CanExecuteChanged != null)
            {
                this.CanExecuteChanged(this, EventArgs.Empty);
            }
        }

        public bool CanExecute(object parameter)
        {
            if (this.methodToDetectCanExecute == null)
            {
                return true;
            }
            else
            {
                return this.methodToDetectCanExecute();
            }
        }

        public void Execute(object parameter)
        {
            this.methodToExecute();
        }
    }
}
