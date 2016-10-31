using System.Windows.Controls;

namespace Tsunami.Classes
{
    public static class Switcher
    {
        public static MainWindow PageSwitcher { get; set; }

        public static void Switch(UserControl newPage)
        {
            ListBoxItem lbi;
            switch (newPage.ToString())
            {
                case "Tsunami.Pages.Add":
                    lbi = (ListBoxItem)PageSwitcher.menuListBox.Items[0];
                    break;
                case "Tsunami.Pages.List":
                    lbi = (ListBoxItem)PageSwitcher.menuListBox.Items[1];
                    break;
                case "Tsunami.Pages.Player":
                    lbi = (ListBoxItem)PageSwitcher.menuListBox.Items[2];
                    break;
                case "Tsunami.Pages.SettingsPage":
                    lbi = (ListBoxItem)PageSwitcher.menuListBox.Items[3];
                    break;
                default:
                    throw new System.Exception("Requested "+newPage.ToString()+" to Switcher but not included in pages!");
            }
            lbi.IsSelected = true;
            PageSwitcher.Navigate(newPage);
        }
    }
}
