using Concrete.Menu.ApplicationMenu;
using Interfaces.ApplicationMenu;
using task.Interfaces;
using Interfaces.Synchronizer;
using Interfaces.Logger;
using Interfaces.Setup;
using task.models;

namespace task.Concrete.Menu
{
    public class MenuMediator : IMenuMediator
    {
        private MainMenu _mainMenu;
        private SetupMenu _setupMenu;

        public MenuMediator(IDisplay display, ISynchronizer synchronizer, ILogger logger, ISetup setup, Settings settings)
        {
            _mainMenu = new MainMenu(synchronizer, logger, display, this);
            _setupMenu = new SetupMenu(logger, setup, display, this, settings);
        }

        public void ShowMainMenu()
        {
            _mainMenu.DisplayMenu();
        }

        public void ShowSetupMenu()
        {
            _setupMenu.DisplayMenu();
        }
    }
}