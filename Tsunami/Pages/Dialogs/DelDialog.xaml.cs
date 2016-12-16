using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Tsunami.Pages.Dialogs
{
    /// <summary>
    /// Interaction logic for DelDialog.xaml
    /// </summary>
    public partial class DelDialog : UserControl
    {
        private bool _deleteTorrent = false;
        private bool _deleteFile = false;

        public DelDialog()
        {
            InitializeComponent();
        }

        public bool DeleteTorrent { get { return _deleteTorrent; } set { if (_deleteTorrent != value) _deleteTorrent = value; } }
        public bool DeleteFile { get { return _deleteFile; } set { if (_deleteFile != value) _deleteFile = value; } }

        private void DelTorrentAndFile_Click(object sender, RoutedEventArgs e)
        {
            DeleteTorrent = true;
            DeleteFile = true;
        }

        private void DelTorrentOnly_Click(object sender, RoutedEventArgs e)
        {
            DeleteTorrent = true;
            DeleteFile = false;
        }
    }
}
