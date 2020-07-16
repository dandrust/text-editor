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
        public EditorMode Mode { get; set; } = EditorMode.Edit;
        public enum EditorMode
        {
            Command,
            Edit,
            Highlight
        }
        protected CommandBuffer commandBuffer = null;

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

        public void ToggleMode()
        {
            if (InEditMode())
            {
                EnterCommandMode();
            }
            else
            {
                LeaveCommandMode();
            }
        }

        public void EnterCommandMode()
        {
            Mode = EditorMode.Command;
            commandBuffer = new CommandBuffer;
        }

        public void LeaveCommandMode()
        {
            Mode = EditorMode.Edit;
            commandBuffer = null;
        }

        public void EnterHighlightMode()
        {
            Mode = EditorMode.Highlight;
        }

        public bool InCommandMode()
        {
            return Mode == EditorMode.Command;
        }

        public bool InEditMode()
        {
            return Mode == EditorMode.Edit;
        }

        public bool InHighlightMode()
        {
            return Mode = EditorMode.Highlight;
        }

        public void DispatchCommand() {
            string command = commandBuffer.ToString();

            switch (command)
            {
                case "quit":
                case "q":
                    Exit();
                    break;
                case "undo":
                case "u":
                case "z":
                    //
                    break;
                case "redo":
                case "r":
                    //
                    break;
                case "save":
                case "s":
                    //
                    break;
                case "copy":
                case "c":
                    //
                    break;
                case "paste":
                case "p":
                    //
                    break;
                case "cut":
                case "x":
                    //
                    break;
                case "highlight":
                case "h":
                    EnterHighlightMode();
                    break;
                default:
                    // Send a timeoutable message to status bar "Invalid Command"
                    break;
            }
        }
    }
}