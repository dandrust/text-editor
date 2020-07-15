using System;

namespace TextEditor
{
    class CursorPosition
    {
        protected int column;
        public int Column { get { return column; } }
        protected int row;
        public int Row { get { return row; } }

        public CursorPosition(int column, int row)
        {
            this.column = column;
            this.row = row;
        }

        public void Restore()
        {
            Console.CursorLeft = column;
            Console.CursorTop = row;
        }

    }
}