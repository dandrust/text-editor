using System;

namespace TextEditor
{
    class StatusBuffer : IUpdatable
    {
        protected int height;
        protected int width;
        protected CursorPosition position;
        protected string content = "";
        protected ColorConfiguration colorConfiguration;
        

        // Fluent Interface
        public StatusBuffer AtPosition(CursorPosition position)
        {
            this.position = position;
            return this;
        }

        public StatusBuffer WithColorConfiguration(ColorConfiguration configuration)
        {
            colorConfiguration = configuration;
            return this;
        }

        public StatusBuffer WithContent(string content)
        {
            this.content = content;
            return this;
        }

        public void paint()
        {
            // Remember console configuration
            CursorPosition originalCursorPosition = new CursorPosition(
                Console.CursorLeft,
                Console.CursorTop
            );
            ColorConfiguration originalColorConfiguration = new ColorConfiguration(
                Console.BackgroundColor,
                Console.ForegroundColor
            );


            position.Restore();
            colorConfiguration.Restore();

            Console.Write(GenerateContent());

            // Reset stored configuration
            originalColorConfiguration.Restore();
            originalCursorPosition.Restore();
        }

        public void UpdateContent(string content)
        {
            this.content = content;
            paint();
        }

        public void Update(string text)
        {
            UpdateContent(text);
        }

        protected string GenerateContent()
        {
            return content.PadRight(this.width);
        }
    }
}