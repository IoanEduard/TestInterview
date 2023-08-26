using System.Text;
using Interfaces.ApplicationMenu;
using Interfaces.Logger;
using Interfaces.Synchronizer;
using task.Interfaces;

namespace Concrete.Menu.ApplicationMenu
{
    public class MainMenu
    {
        private bool _mainMenuVisible = true;
        private ISynchronizer _sync;
        private ILogger _logger;
        private IDisplay _display;
        private IMenuMediator _menuMediator;
        public MainMenu(ISynchronizer synchronizer, ILogger logger, IDisplay display, IMenuMediator menuMediator)
        {
            _sync = synchronizer;
            _logger = logger;
            _display = display;
            _menuMediator = menuMediator;
        }

        public void DisplayMenu()
        {
            _display.Show(OpenMenu());

            while (_mainMenuVisible)
            {
                if (int.TryParse(Console.ReadLine(), out int option))
                {
                    switch (option)
                    {
                        case (int)MenuEnum.Setup:
                            _menuMediator.ShowSetupMenu();
                            _mainMenuVisible = false;
                            Hide();
                            break;
                        case (int)MenuEnum.SyncNow:
                            _sync.SyncNow();
                            break;
                        case (int)MenuEnum.DisplayLogs:
                            _logger.DisplayLogs();
                            break;
                        case (int)MenuEnum.Exit:
                            _mainMenuVisible = false;
                            Hide();
                            break;
                    }
                }
                else _display.Show("Invalid input");
            }
        }

        public void Hide() => Console.Clear();

        private string OpenMenu()
        {
            var sr = new StringBuilder();
            sr.Append("MENU - Select a number between 0 and 3 \t\t\t\n");

            sr.Append("0. Setup\n");
            sr.Append("1. Sync now\n");
            sr.Append("2. Display logs\n");
            sr.Append("3. Exit\n");

            return sr.ToString();
        }
    }
}

// so in the menu I just display current settings. 
// hard written now, then from a json file.

// in a setup file I will do the changes.