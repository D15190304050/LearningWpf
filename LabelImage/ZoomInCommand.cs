using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace LabelImage
{
    public class ZoomInCommand : ICommand
    {
        private const int MaxZoomIn = 3;
        private int currentZoomIn;

        public ZoomInCommand()
        {
            currentZoomIn = 1;
        }

        public bool CanExecute(object parameter)
        {
            return currentZoomIn >= MaxZoomIn;
        }

        public void Execute(object parameter)
        {
            throw new NotImplementedException();
        }

        public event EventHandler CanExecuteChanged;
    }
}
