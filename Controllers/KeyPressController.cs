using System;

namespace TextEditor
{
    abstract class KeyPressController
    {
        protected Editor editor;
        public KeyPressController(Editor editor)
        {
            this.editor = editor;
        }

        abstract public bool Process(ConsoleKeyInfo keyPress);

        protected bool EscapeKey(ConsoleKeyInfo keyPress)
        {
            return keyPress.Key == ConsoleKey.Escape;
        }

        protected bool ModifiedByAlt(ConsoleKeyInfo keyPress)
        {
            return (keyPress.Modifiers & ConsoleModifiers.Alt) != 0;
        }

        protected bool ModifiedByControl(ConsoleKeyInfo keyPress)
        {
            return (keyPress.Modifiers & ConsoleModifiers.Control) != 0;
        }

        protected bool ArrowKey(ConsoleKeyInfo keyPress)
        {
            return keyPress.Key == ConsoleKey.UpArrow ||
                keyPress.Key == ConsoleKey.DownArrow ||
                keyPress.Key == ConsoleKey.LeftArrow ||
                keyPress.Key == ConsoleKey.RightArrow;
        }

        protected bool NullKey(ConsoleKeyInfo keyPress)
        {
            return keyPress.KeyChar == '\u0000';
        }

        protected bool BackspaceKey(ConsoleKeyInfo keyPress)
        {
            return keyPress.Key == ConsoleKey.Backspace;
        }

        protected bool EnterKey(ConsoleKeyInfo keyPress)
        {
            return keyPress.Key == ConsoleKey.Enter;
        }
    }
}