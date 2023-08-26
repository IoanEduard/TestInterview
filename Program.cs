
using Concrete.Logger;
using Concrete.Setup;
using Concrete.Synchronizer;
using task.Concrete;
using task.Concrete.Menu;
using task.models;

var display = new Display();
var synchronizer = new Synchronizer();
var logger = new Logger();
var setup = new Setup();
var settings = new Settings();

var menuMediator = new MenuMediator(display, synchronizer, logger, setup, settings);

menuMediator.ShowMainMenu();




