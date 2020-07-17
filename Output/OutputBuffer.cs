using System;

namespace TextEditor
{
    class OutputBuffer
    {
        protected int height;
        protected int width;
        protected CursorPosition rootPosition;
        protected ColorConfiguration colorConfiguration;

        public void SetDimensions(int height, int width)
        {
            this.height = height;
            this.width = width;
        }

        public void SetPosition(CursorPosition position)
        {
            this.rootPosition = position;
        }

        public void SetColorConfiguration(ColorConfiguration configuration)
        {
            colorConfiguration = configuration;
        }

        public void Paint()
        {
            colorConfiguration.Restore();

            for (int i = 0; i < height; i++)
            {
                rootPosition.Restore(0, i);
                Console.Write(GenerateContent(i));
            }
        }

        protected virtual string GenerateContent(int lineIndex)
        {
            return "";
        }
    }
}