
using Concrete.Logger;
using Concrete.Setup;
using Concrete.Synchronizer;
using task.Concrete;
using task.Concrete.Menu;
using task.models;

var settings = new Settings();
var display = new Display();
var logger = new Logger(settings);
var synchronizer = new Synchronizer(settings, logger, display);
var setup = new Setup(settings, display, logger);

var menuMediator = new MenuMediator(display, synchronizer, logger, setup, settings);

menuMediator.ShowMainMenu();




