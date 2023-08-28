using System.Text;
using Interfaces.Logger;
using Interfaces.Setup;
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
             Clear();
            _display.Show(OpenSetupMenu());

            while (_setupMenuVisible)
            {
                if (int.TryParse(ReadLine(), out int option))
                {
                    switch (option)
                    {
                        case (int)SetupMenuEnum.SourcePath:
                            if (_setup.SetSourcePath())
                                SuccessAction();
                            else FailedAction("Failed to update source file path!");
                            break;
                        case (int)SetupMenuEnum.ReplicaPath:
                            if (_setup.SetReplicaPath())
                                SuccessAction();
                            else FailedAction("Failed to update replica file path!");
                            break;
                        case (int)SetupMenuEnum.LogFilePath:
                            if (_setup.SetLoggerPath())
                                SuccessAction();
                            else FailedAction("Failed to update logger file path!");
                            break;
                        case (int)SetupMenuEnum.Interval:
                            if (_setup.SetInterval())
                                SuccessAction();
                            else FailedAction("Failed to update interval");
                            break;
                        case (int)SetupMenuEnum.MainMenu:
                            _menuMediator.ShowMainMenu();
                            Clear();
                            _setupMenuVisible = false;
                            break;
                        case (int)SetupMenuEnum.Exit:
                            Environment.Exit(0);
                            break;
                    }
                }
                else _display.Show("Invalid input");
            }
        }

        private void SuccessAction()
        {
            _logger.LogAction();
            _display.Show(OpenSetupMenu());
        }

        private void FailedAction(string message)
        {
            _display.Show(message);
        }

        private string OpenSetupMenu()
        {
            Clear();
            _setupMenuVisible = true;
            var sr = new StringBuilder();
            sr.Append("MENU\t\t\t\n");

            sr.Append($"0. Set Source Path ({_settings.SourcePath})\n");
            sr.Append($"1. Set Replica path ({_settings.ReplicaPath})\n");
            sr.Append($"2. Set Logger Path ({_settings.ReplicaPath})\n");
            sr.Append($"3. Update interval ({_settings.Interval})\n");
            sr.Append($"4. Main Menu\n");
            sr.Append($"5. Exit\n");

            return sr.ToString();
        }
    }
}