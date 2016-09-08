using System;
using MahApps.Metro.Controls.Dialogs;
using System.Windows.Controls;
using System.Windows;

namespace Tsunami.Gui.Wpf
{
    class CustomMessage
    {
        CustomDialog message = null;
        Grid messageGrid = null;
        Button bnt = null;
        TextBlock txt = null;

        public CustomMessage(string msg)
        {
            message = new CustomDialog();
            messageGrid = new Grid();
            messageGrid.Width = 700;
            messageGrid.Height = 150;
            bnt = new Button();
            txt = new TextBlock();

            txt.Text = msg;
            bnt.Content = "OK";
            bnt.Width = 100;
            txt.FontSize = 20;

            bnt.HorizontalAlignment = HorizontalAlignment.Center;
            bnt.VerticalAlignment = VerticalAlignment.Center;
            txt.HorizontalAlignment = HorizontalAlignment.Center;
            txt.VerticalAlignment = VerticalAlignment.Top;

            messageGrid.Children.Insert(0, txt);
            messageGrid.Children.Insert(1, bnt);
            message.Content = messageGrid;
            message.ShowDialogExternally();
            bnt.Click += new RoutedEventHandler(OnOkClicked);
        }

        private async void OnOkClicked(object sender, EventArgs e)
        {
            await message.RequestCloseAsync();
            bnt = null;
            txt = null;
            messageGrid = null;
            message = null;
        }
    }
}
