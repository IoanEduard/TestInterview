
using task.Concrete.Logger;
using task.Concrete.Setup;
using task.Concrete.Synchronizer;
using task.Concrete;
using task.Concrete.Menu;
using task.models;

var settings = new Settings();
var display = new Display();
var logger = new Logger(settings);
var setup = new Setup(settings, display, logger);

var synchronizer = new Synchronizer(settings, logger, display);
var menuMediator = new MenuMediator(display, synchronizer, logger, setup, settings);

Task synchronizationTask = synchronizer.ExecuteScheduledSynchronizationAsync();

menuMediator.ShowMainMenu();
await synchronizationTask;



