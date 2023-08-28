
using Concrete.Logger;
using Concrete.Setup;
using Concrete.Synchronizer;
using task.Concrete;
using task.Concrete.Menu;
using task.models;

var display = new Display();
var settings = new Settings();
var synchronizer = new Synchronizer(settings);
var logger = new Logger();
var setup = new Setup(settings, display);

var menuMediator = new MenuMediator(display, synchronizer, logger, setup, settings);

menuMediator.ShowMainMenu();




