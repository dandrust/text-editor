using System;

namespace TextEditor
{
    class CommandController
    {
        protected Editor editor;
        public CommandController(Editor editor)
        {
            this.editor = editor;
        }

        public bool Process(ConsoleKeyInfo keyPress)
        {
            if (keyPress.Key == ConsoleKey.Q)
            {
                return false;
            }
            else if (EscapeKey(keyPress))
            {
                editor.ToggleMode();
            };

            

            return true;
        }

        protected bool EscapeKey(ConsoleKeyInfo keyPress)
        {
            return keyPress.Key == ConsoleKey.Escape;
        }
    }
}