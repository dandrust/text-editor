using System;

namespace TextEditor
{
    class Editor
    {
        protected SimpleOutputBuffer titleBar;
        protected ModeStatusField statusBar;
        protected EditorBuffer editorBuffer;
        public EditorBuffer EditorBuffer { get { return editorBuffer; } }
        protected TextBuffer textBuffer;
        public TextBuffer TextBuffer { get { return textBuffer; } }
        protected EditorMode mode = EditorMode.Edit;
        public EditorMode Mode { get { return mode; } }
        public enum EditorMode
        {
            Command,
            Edit,
            Highlight
        }
        protected CommandBuffer commandBuffer = null;

        public event EventHandler<EditorModeEventArgs> ModeChangedEvent;

        public Editor()
        {
            Console.Clear();

            ColorConfiguration editorColorConfiguration = GetEditorColorConfiguration();
            ColorConfiguration systemColorConfiguration = GetSystemColorConfiguration();

            titleBar = new SimpleOutputBuffer();
            titleBar.SetDimensions(1, Console.BufferWidth);
            titleBar.SetPosition(new CursorPosition(0, 0));
            titleBar.SetColorConfiguration(systemColorConfiguration);
            titleBar.SetContent("Welcome to Text Editor");
            titleBar.Paint();

            // statusBar = new FieldOutputBuffer();
            statusBar = new ModeStatusField();
            statusBar.SetDimensions(1, Console.BufferWidth);
            statusBar.SetPosition(new CursorPosition(0, Console.BufferHeight - 1)); // Last row
            statusBar.SetColorConfiguration(systemColorConfiguration);
            // statusBar.AddField("One");
            // statusBar.AddField("Two");
            // statusBar.AddField("Three");
            statusBar.SetContent("EDIT");
            statusBar.Paint();

            ModeChangedEvent += statusBar.HandleModeChangedEvent;

            textBuffer = new TextBuffer();

            editorBuffer = new EditorBuffer();
            editorBuffer.SetDimensions(Console.BufferHeight - 2, Console.BufferWidth);
            editorBuffer.SetPosition(new CursorPosition(0, 1));
            editorBuffer.SetColorConfiguration(editorColorConfiguration);
            editorBuffer.SetModel(textBuffer);
            editorBuffer.Start();
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
                changeMode(Editor.EditorMode.Command);
            }
            else if (InCommandMode())
            {
                changeMode(Editor.EditorMode.Edit);
            }
        }

        protected void changeMode(Editor.EditorMode mode)
        {
            this.mode = mode;
            raiseModeChangedEvent(mode);
        }

        protected void raiseModeChangedEvent(Editor.EditorMode mode)
        {
            EventHandler<EditorModeEventArgs> raiseEvent = ModeChangedEvent;
            if (raiseEvent != null) raiseEvent(this, new EditorModeEventArgs(mode));
        }

        public bool InCommandMode()
        {
            return Mode == EditorMode.Command;
        }

        public bool InEditMode()
        {
            return Mode == EditorMode.Edit;
        }

        public void DispatchCommand()
        {
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
                    // EnterHighlightMode();
                    break;
                default:
                    // Send a timeoutable message to status bar "Invalid Command"
                    break;
            }
        }
    }
}