using System;

namespace TextEditor
{
    class KeyPressHandler
    {
        protected Editor editor;
        protected CommandController commandController;
        public KeyPressHandler(Editor editor, CommandController commandController)
        {
            this.editor = editor;
            this.commandController = commandController;
        }

        public void listen()
        {
            while (HandleKeyPress(Console.ReadKey(true))) { }

        }

        protected bool HandleKeyPress(ConsoleKeyInfo keyPress)
        {
            if (editor.InCommandMode()) {
                return commandController.Process(keyPress);
            }
            if (EscapeKey(keyPress)) {
                editor.ToggleMode();
            }

            if (ModifiedByAlt(keyPress))
            {
                HandleAltInput(keyPress);
            }
            else if (ModifiedByControl(keyPress))
            {
                HandleControlInput(keyPress);
            }
            else if (ArrowKey(keyPress))
            {
                HandleArrowKey(keyPress);
            }
            else if (NullKey(keyPress))
            {
                //
            }
            else if (BackspaceKey(keyPress))
            {
                HandleBackpace();
            }
            else if (EnterKey(keyPress))
            {
                HandleEnter();
            }
            else
            {
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

        protected void HandleAltInput(ConsoleKeyInfo keyPress) { }
        protected void HandleControlInput(ConsoleKeyInfo keyPress) { }
        protected void HandleArrowKey(ConsoleKeyInfo keyPress)
        {
            if (keyPress.Key == ConsoleKey.LeftArrow)
            {
                editor.EditorBuffer.MoveCursorLeft();
            }
            else if (keyPress.Key == ConsoleKey.RightArrow)
            {
                editor.EditorBuffer.MoveCursorRight();
            }
            else if (keyPress.Key == ConsoleKey.UpArrow)
            {
                editor.EditorBuffer.MoveCursorUp();
            }
            else if (keyPress.Key == ConsoleKey.DownArrow)
            {
                editor.EditorBuffer.MoveCursorDown();
            }
        }
        protected void HandleBackpace()
        {
            // Remember the column position of the cursor
            CursorPosition position = new CursorPosition(Console.CursorLeft, Console.CursorTop);

            // Get editor buffer-aware line and column indicies
            int zeroIndexedCurrentLine = editor.EditorBuffer.CurrentLineIndex;
            int zeroIndexedCurrentColumn = editor.EditorBuffer.CurrentColumnIndex;
            int zeroIndexedPositionToDelete = zeroIndexedCurrentColumn - 1;

            if (zeroIndexedPositionToDelete < 0) {
                if (zeroIndexedCurrentLine > 0) {
                    int textLengthFromPreviouLine = editor.TextBuffer.GetLine(zeroIndexedCurrentLine - 1).Length;
                    string textFromCurrentLine = editor.TextBuffer.GetLine(zeroIndexedCurrentLine);
                    editor.TextBuffer.RemoveLine(zeroIndexedCurrentLine);
                    editor.TextBuffer.AppendToLine(zeroIndexedCurrentLine - 1, textFromCurrentLine);

                    // Need to tell editor.EditorBuffer where cursor should be
                    editor.EditorBuffer.MoveCursorTo(textLengthFromPreviouLine, zeroIndexedCurrentLine);
                } else {
                    return;
                }
            } else {
                // Remove the character from the underlying text buffer
                editor.TextBuffer.Remove(zeroIndexedCurrentLine, zeroIndexedPositionToDelete, 1);

                // Reset cursor position
                position.Restore();

                // Advance curson position back by one
                editor.EditorBuffer.MoveCursorLeft();
            }
        }
        protected void HandleEnter()
        {
            // Get editor buffer-aware line and column indicies
            int zeroIndexedCurrentLine = editor.EditorBuffer.CurrentLineIndex;
            int zeroIndexedCurrentColumn = editor.EditorBuffer.CurrentColumnIndex;

            // Get string after cursor
            string remainingSubstring = editor.TextBuffer.GetLine(zeroIndexedCurrentLine).Length > 0 ?
                remainingSubstring = editor.TextBuffer.GetLine(zeroIndexedCurrentLine).Substring(zeroIndexedCurrentColumn) :
                "";

            editor.TextBuffer.Remove(zeroIndexedCurrentLine, zeroIndexedCurrentColumn, editor.TextBuffer.GetLine(zeroIndexedCurrentLine).Length - zeroIndexedCurrentColumn);
            editor.TextBuffer.InsertLine(zeroIndexedCurrentLine, remainingSubstring);

            // Clear out the text after the cursor of the current line
            // Console.Write("".PadRight(remainingSubstring.Length));

            // Add a new line
            editor.EditorBuffer.NewLine();

            // Remember the column position of the cursor
            CursorPosition newPosition = new CursorPosition(Console.CursorLeft, Console.CursorTop);

            // Write the new text
            Console.Write(remainingSubstring);

            // Move cursor back to beginning of new line
            newPosition.Restore();
        }
        protected void HandleInsert(char character)
        {
            // Remember the column position of the cursor
            CursorPosition position = new CursorPosition(Console.CursorLeft, Console.CursorTop);

            // Get editor buffer-aware line and column indicies
            int zeroIndexedCurrentLine = editor.EditorBuffer.CurrentLineIndex;
            int zeroIndexedCurrentColumn = editor.EditorBuffer.CurrentColumnIndex;

            // Insert character
            editor.TextBuffer.Insert(zeroIndexedCurrentLine, zeroIndexedCurrentColumn, character);

            // Restore the column position of the curor before rewriting the line
            position.Restore();

            // Advance cursor position by one
            editor.EditorBuffer.AdvanceCursor();
        }
    }
}