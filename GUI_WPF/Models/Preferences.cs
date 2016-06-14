using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Windows;

namespace Tsunami.Gui.Wpf
{
    public class Preferences : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private bool _showAdvancedInterface;

        public bool ShowAdvancedInterface { get { return _showAdvancedInterface; } set { if (_showAdvancedInterface != value) { _showAdvancedInterface = value; CallPropertyChanged("ShowAdvancedInterface"); } } }

        public Preferences()
        {
            // TO DO : Load from SessionManager Settings
            _showAdvancedInterface = false;
        }

        private void CallPropertyChanged(string prop)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }

    }
}
