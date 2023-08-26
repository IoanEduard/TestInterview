using task.Interfaces;
using static System.Console;

namespace task.Concrete
{
    public class Display : IDisplay
    {
        public void Show<T>(T value)
        {
            WriteLine(value);
        }
    }
}