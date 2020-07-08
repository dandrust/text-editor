using System;

namespace TextEditor
{
    class ColorConfiguration
    {
        protected ConsoleColor background;
        protected ConsoleColor foreground;

        public ColorConfiguration(ConsoleColor background, ConsoleColor foreground)
        {
            this.background = background;
            this.foreground = foreground;
        }

        public void Restore()
        {
            Console.BackgroundColor = background;
            Console.ForegroundColor = foreground;
        }
    }
}