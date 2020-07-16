using System;

namespace TextEditor
{
    class Editor
    {
        protected StatusBuffer titleBar;
        protected StatusBuffer statusBar;
        protected EditorBuffer editorBuffer;
        public EditorBuffer EditorBuffer { get { return editorBuffer; } }
        protected TextBuffer textBuffer;
        public TextBuffer TextBuffer { get { return textBuffer; } }

        public Editor()
        {
            Console.Clear();

            ColorConfiguration editorColorConfiguration = GetEditorColorConfiguration();
            ColorConfiguration systemColorConfiguration = GetSystemColorConfiguration();

            titleBar = new StatusBuffer(1, Console.BufferWidth);
            titleBar
                .AtPosition(new CursorPosition(0, 0))
                .WithContent("Welcome to Text Editor")
                .WithColorConfiguration(systemColorConfiguration)
                .paint();

            statusBar = new StatusBuffer(1, Console.BufferWidth);
            statusBar
                .AtPosition(new CursorPosition(0, Console.BufferHeight - 1)) // Last row
                .WithContent("STATUS: Ready")
                .WithColorConfiguration(systemColorConfiguration)
                .paint();

            textBuffer = new TextBuffer();
            textBuffer.registerSubscriber(statusBar);

            editorBuffer = new EditorBuffer(Console.BufferHeight - 2, Console.BufferWidth);
            editorBuffer
                .AtPosition(new CursorPosition(0, 1))
                .WithColorConfiguration(editorColorConfiguration)
                .WithModel(textBuffer)
                .Start();

            

            

        }

        protected ColorConfiguration GetEditorColorConfiguration()
        {
            return new ColorConfiguration(
                Console.BackgroundColor,
                Console.ForegroundColor
            );
        }

        protected ColorConfiguration GetSystemColorConfiguration()
        {
            return new ColorConfiguration(
                ConsoleColor.DarkBlue,
                ConsoleColor.White
            );
        }

        public void Exit()
        {
            Console.Clear();
            Console.WriteLine(textBuffer.ToString());
        }

        public EditorBuffer GetEditorBuffer()
        {
            return editorBuffer;
        }

        public TextBuffer GetTextBuffer()
        {
            return textBuffer;
        }
    }
}