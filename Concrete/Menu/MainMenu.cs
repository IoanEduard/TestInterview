using System.Text;
using Interfaces.ApplicationMenu;
using Interfaces.Logger;
using Interfaces.Synchronizer;
using task.Interfaces;
using static System.Console;

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
            if (!IsOutputRedirected) Clear();
            _display.Show(OpenMenu());

            while (_mainMenuVisible)
            {
                if (int.TryParse(ReadLine(), out int option))
                {
                    switch (option)
                    {
                        case (int)MenuEnum.Setup:
                            _menuMediator.ShowSetupMenu();
                            _mainMenuVisible = false;
                            Clear();
                            break;
                        case (int)MenuEnum.SyncNow:
                            _sync.Sync();
                            var message = "Replica was synchronized manually";
                            _display.Show(message);
                            _logger.LogAction(message);
                            break;
                        case (int)MenuEnum.DisplayLogs:
                            _logger.DisplayLogs();
                            break;
                        case (int)MenuEnum.Exit:
                            Environment.Exit(0);
                            Clear();
                            break;
                    }
                }
                else _display.Show("Invalid input");
            }
        }

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