using System;

namespace TextEditor
{
    class Program
    {

        static void Main(string[] args)
        {
            Console.TreatControlCAsInput = true;
            Editor editor = new Editor();
            KeyPressHandler keyPressHandler = new KeyPressHandler(editor);

            keyPressHandler.listen();
            editor.Exit();
        }

        static void HandleBackspace(TextBuffer buffer)
        {

            int positionToDelete = Console.CursorLeft - 1 ;
            if (positionToDelete < 0 ) return;
            buffer.Remove(0, positionToDelete, 1);
            string remainingSubstring = buffer.ToString().Substring(positionToDelete);

            Console.SetCursorPosition(positionToDelete, Console.CursorTop);
            Console.Write(remainingSubstring);
            Console.Write(' ');
            Console.SetCursorPosition(positionToDelete, Console.CursorTop);
        }

        static void HandleInsert(TextBuffer buffer, char character)
        {
            // Remember current cursor position
            int left = Console.CursorLeft;

            // Get string after cursor
            string remainingSubstring = buffer.ToString().Substring(Console.CursorLeft);

            // Insert character
            buffer.Insert(0, Console.CursorLeft, character);

            // Append new character + string after insert to display
            Console.Write(character);
            Console.Write(remainingSubstring);

            // Advance curson position by one
            Console.CursorLeft = left + 1;
        }

        static void HandleEnter(TextBuffer buffer)
        {
            buffer.AddNewLine(Console.CursorTop); // 1

            Console.SetCursorPosition(0, Console.CursorTop + 1);
        }
    }
}
