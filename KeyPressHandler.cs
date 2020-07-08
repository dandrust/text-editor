using System;

namespace TextEditor
{
    class KeyPressHandler
    {
        protected Editor editor;
        public KeyPressHandler(Editor editor) {
            this.editor = editor;
        }

        public void listen()
        {
            while (HandleKeyPress( Console.ReadKey(true) )) {}
            
        }

        protected bool HandleKeyPress(ConsoleKeyInfo keyPress)
        {
            if (EscapeKey(keyPress)) return false;

            if (ModifiedByAlt(keyPress)) {
                HandleAltInput(keyPress);
            } else if (ModifiedByControl(keyPress)) {
                HandleControlInput(keyPress);
            } else if (ArrowKey(keyPress)) {
                HandleArrowKey(keyPress);
            } else if (NullKey(keyPress)) {
                //
            } else if (BackspaceKey(keyPress)) {
                HandleBackpace();
            } else if (EnterKey(keyPress)) {
                HandleEnter();
            } else {
                HandleInsert(keyPress.KeyChar);
            }
            
            return true;
        }

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

        protected void HandleAltInput(ConsoleKeyInfo keyPress) {}
        protected void HandleControlInput(ConsoleKeyInfo keyPress) {}
        protected void HandleArrowKey(ConsoleKeyInfo keyPress)
        {
            if (keyPress.Key == ConsoleKey.LeftArrow) {
                editor.GetEditorBuffer().MoveCursorLeft();
            } else if (keyPress.Key == ConsoleKey.RightArrow) {
                editor.GetEditorBuffer().MoveCursorRight();
            } else if (keyPress.Key == ConsoleKey.UpArrow) {
                editor.GetEditorBuffer().MoveCursorUp();
            } else if (keyPress.Key == ConsoleKey.DownArrow) {
                editor.GetEditorBuffer().MoveCursorDown();
            }
        }
        protected void HandleBackpace() {}
        protected void HandleEnter() {}
        protected void HandleInsert(char character)
        {
            int left = Console.CursorLeft;
            int currentLine = editor.GetEditorBuffer().GetCurrentLineIndex();
            int currentColumnPosition = editor.GetEditorBuffer().GetCurrentColumnIndex();

            // Get string after cursor
            string remainingSubstring = editor.GetTextBuffer().GetLine(currentLine).Substring(currentColumnPosition);

            // Insert character
            editor.GetTextBuffer().Insert(currentLine, currentColumnPosition, character);

            // Append new character + string after insert to display
            Console.Write(character);
            Console.Write(remainingSubstring);

            // Advance curson position by one
            Console.CursorLeft = left + 1;
        }
    }
}