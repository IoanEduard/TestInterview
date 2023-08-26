using System.Text;
using Interfaces.Logger;
using Interfaces.Setup;
using Interfaces;
using task.models;
using task.Interfaces;
using static System.Console;
using Interfaces.ApplicationMenu;

namespace task.Concrete.Menu
{
    public class SetupMenu
    {
        private bool _setupMenuVisible;
        private ILogger _logger;
        private ISetup _setup;
        private Settings _settings;
        private IDisplay _display;
        private IMenuMediator _menuMediator;

        public SetupMenu(ILogger logger, ISetup setup, IDisplay display, IMenuMediator menuMediator, Settings settings)
        {
            _logger = logger;
            _setup = setup;
            _settings = settings;
            _display = display;
            _menuMediator = menuMediator;
        }

        public void DisplayMenu()
        {
            _display.Show(OpenSetupMenu());

            while (_setupMenuVisible)
            {
                if (int.TryParse(ReadLine(), out int option))
                {
                    switch (option)
                    {
                        case (int)SetupMenuEnum.SourcePath:
                            _setup.SetSourcePath(""); // later
                            _logger.LogAction(); // later
                            break;
                        case (int)SetupMenuEnum.ReplicaPath:
                            _setup.SetReplicaPath("ReplicaPath"); // later
                            _logger.LogAction(); // later
                            break;
                        case (int)SetupMenuEnum.LogFilePath:
                            _setup.SetLoggerPath("LoggerPath"); // later
                            _logger.LogAction(); // later
                            break;
                        case (int)SetupMenuEnum.DisplayLogs:
                            _setup.SetInterval(5); // later
                            _logger.LogAction(); // later
                            break;
                        case (int)SetupMenuEnum.MainMenu:
                            _menuMediator.ShowMainMenu();
                            Hide();
                            _setupMenuVisible = false;
                            break;
                        case (int)SetupMenuEnum.Exit:
                            _setupMenuVisible = false;
                            break;
                    }
                }
                else _display.Show("Invalid input");
            }
        }
        public void Hide()
        {
            if (!IsOutputRedirected)
                Clear();
        }

        private string OpenSetupMenu()
        {
            _setupMenuVisible = true;
            var sr = new StringBuilder();
            sr.Append("MENU\t\t\t\n");

            sr.Append($"0. Set Source Path ({_settings.SourcePath})\n");
            sr.Append($"1. Set Replica path ({_settings.ReplicaPath})\n");
            sr.Append($"2. Set Logger Path ({_settings.ReplicaPath})\n");
            sr.Append($"3. Update interval ({_settings.Interval})\n");
            sr.Append($"4. Main Menu\n");

            return sr.ToString();
        }
    }
}