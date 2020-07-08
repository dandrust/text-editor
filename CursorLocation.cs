using System;

namespace TextEditor
{
    class CursorPosition
    {
        protected int column;
        protected int row;

        public CursorPosition(int column, int row)
        {
            this.column = column;
            this.row = row;
        }

        public int GetRow()
        {
            return row;
        }

        public int GetColumn()
        {
            return column;
        }

        public void Restore()
        {
            Console.CursorLeft = column;
            Console.CursorTop = row;
        }

    }
}