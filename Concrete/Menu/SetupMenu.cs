using System.Text;
using task.Interfaces.Logger;
using task.Interfaces.Setup;
using task.models;
using task.Interfaces;
using static System.Console;
using task.Interfaces.ApplicationMenu;

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
                            _display.Show("Only absolute paths accepted\n");
                            if (_setup.SetSourcePath())
                                SuccessAction("SourcePath updated successfully!");
                            else FailedAction("Failed to SourcePath file path!");
                            break;
                        case (int)SetupMenuEnum.ReplicaPath:
                            _display.Show("Only absolute paths accepted\n");
                            if (_setup.SetReplicaPath())
                                SuccessAction("ReplicaPath updated successfully!");
                            else FailedAction("Failed to ReplicaPath file path!");
                            break;
                        case (int)SetupMenuEnum.LogFilePath:
                            _display.Show("Only absolute paths accepted\n");
                            if (_setup.SetLoggerPath())
                                SuccessAction("LoggerPath updated successfully!");
                            else FailedAction("Failed to update LoggerPath path!");
                            break;
                        case (int)SetupMenuEnum.Interval:
                            if (_setup.SetInterval())
                                SuccessAction("Interval updated successfully!");
                            else FailedAction("Failed to update the interval!");
                            break;
                        case (int)SetupMenuEnum.MainMenu:
                            _menuMediator.ShowMainMenu();
                            Clear();
                            _setupMenuVisible = false;
                            break;
                        case (int)SetupMenuEnum.Exit:
                            _logger.LogAction("\nProgram terminated!\n Synchronization unavailable!");
                            Environment.Exit(0);
                            break;
                    }
                }
                else _display.Show("Invalid input");
            }
        }

        private void SuccessAction(string message)
        {
            _display.Show(OpenSetupMenu());
            _logger.LogAction(message);
        }

        private void FailedAction(string message)
        {
            _display.Show(message);
            _logger.LogAction(message);
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