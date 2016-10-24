using System.Windows.Controls;

namespace Tsunami.Classes
{
    public static class Switcher
    {
        public static MainWindow pageSwitcher;

        public static void Switch(UserControl newPage)
        {
            ListBoxItem lbi;
            switch (newPage.ToString())
            {
                case "Tsunami.Pages.Add":
                    lbi = (ListBoxItem)pageSwitcher.menuListBox.Items[0];
                    break;
                case "Tsunami.Pages.List":
                    lbi = (ListBoxItem)pageSwitcher.menuListBox.Items[1];
                    break;
                case "Tsunami.Pages.Player":
                    lbi = (ListBoxItem)pageSwitcher.menuListBox.Items[2];
                    break;
                case "Tsunami.Pages.SettingsPage":
                    lbi = (ListBoxItem)pageSwitcher.menuListBox.Items[3];
                    break;
                default:
                    throw new System.Exception("Requested "+newPage.ToString()+" to Switcher but not included in pages!");
                    //break;
            }
            lbi.IsSelected = true;
            pageSwitcher.Navigate(newPage);
        }
    }
}
