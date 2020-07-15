using System;

namespace TextEditor
{
    class KeyPressHandler
    {
        protected Editor editor;
        public KeyPressHandler(Editor editor)
        {
            this.editor = editor;
        }

        public void listen()
        {
            while (HandleKeyPress(Console.ReadKey(true))) { }

        }

        protected bool HandleKeyPress(ConsoleKeyInfo keyPress)
        {
            if (EscapeKey(keyPress)) return false;

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
                editor.GetEditorBuffer().MoveCursorLeft();
            }
            else if (keyPress.Key == ConsoleKey.RightArrow)
            {
                editor.GetEditorBuffer().MoveCursorRight();
            }
            else if (keyPress.Key == ConsoleKey.UpArrow)
            {
                editor.GetEditorBuffer().MoveCursorUp();
            }
            else if (keyPress.Key == ConsoleKey.DownArrow)
            {
                editor.GetEditorBuffer().MoveCursorDown();
            }
        }
        protected void HandleBackpace()
        {
            // Remember the column position of the cursor
            CursorPosition position = new CursorPosition(Console.CursorLeft, Console.CursorTop);

            // zero-indexed
            int zeroIndexedCurrentLine = editor.GetEditorBuffer().GetCurrentLineIndex();

            // zero-indexed
            int zeroIndexedCurrentColumn = editor.GetEditorBuffer().GetCurrentColumnIndex();

            int zeroIndexedPositionToDelete = zeroIndexedCurrentColumn - 1;
            if (zeroIndexedPositionToDelete < 0) return;

            // Get string after cursor
            string remainingSubstring = editor.GetTextBuffer().GetLine(zeroIndexedCurrentLine).Length >= zeroIndexedCurrentColumn ?
                remainingSubstring = editor.GetTextBuffer().GetLine(zeroIndexedCurrentLine).Substring(zeroIndexedCurrentColumn) :
                "";

            // Remove the character from the underlying text buffer
            editor.GetTextBuffer().Remove(zeroIndexedCurrentLine, zeroIndexedPositionToDelete, 1);

            // Move the curor left one position to write over to-be-deleted character
            Console.SetCursorPosition(position.GetColumn() - 1, position.GetRow());

            // Write remaining substring + a space to cover removed character
            Console.Write(remainingSubstring);
            Console.Write(" ");

            // Reset cursor position
            position.Restore();

            // Advance curson position back by one
            editor.GetEditorBuffer().MoveCursorLeft();
        }
        protected void HandleEnter()
        {
            // zero-indexed
            int zeroIndexedCurrentLine = editor.GetEditorBuffer().GetCurrentLineIndex();

            // zero-indexed
            int zeroIndexedCurrentColumn = editor.GetEditorBuffer().GetCurrentColumnIndex();

            // Get string after cursor
            string remainingSubstring = editor.GetTextBuffer().GetLine(zeroIndexedCurrentLine).Length > 0 ?
                remainingSubstring = editor.GetTextBuffer().GetLine(zeroIndexedCurrentLine).Substring(zeroIndexedCurrentColumn) :
                "";

            editor.GetTextBuffer().Remove(zeroIndexedCurrentLine, zeroIndexedCurrentColumn, editor.GetTextBuffer().GetLine(zeroIndexedCurrentLine).Length - zeroIndexedCurrentColumn);
            editor.GetTextBuffer().InsertLine(zeroIndexedCurrentLine, remainingSubstring);

            // Clear out the text after the cursor of the current line
            Console.Write("".PadRight(remainingSubstring.Length));

            // Add a new line
            editor.GetEditorBuffer().NewLine();

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

            // zero-indexed
            int zeroIndexedCurrentLine = editor.GetEditorBuffer().GetCurrentLineIndex();

            // zero-indexed
            int zeroIndexedCurrentColumn = editor.GetEditorBuffer().GetCurrentColumnIndex();

            // Get string after cursor
            string remainingSubstring = editor.GetTextBuffer().GetLine(zeroIndexedCurrentLine).Length > 0 ?
                remainingSubstring = editor.GetTextBuffer().GetLine(zeroIndexedCurrentLine).Substring(zeroIndexedCurrentColumn) :
                "";

            // Insert character
            editor.GetTextBuffer().Insert(zeroIndexedCurrentLine, zeroIndexedCurrentColumn, character);

            // Append new character + remaining substring after insert to display
            Console.Write(character);
            Console.Write(remainingSubstring);

            // Restore the column position of the curor before rewriting the line
            position.Restore();

            // Advance curson position by one
            editor.GetEditorBuffer().AdvanceCursor();
        }
    }
}