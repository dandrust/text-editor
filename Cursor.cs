using System;

namespace TextEditor
{
    // This class is responsible for:
    //  * Converting (x, y) coordinates of it's context to (x, y) coordinates of the screen
    //  * Setting the position of the Console's cursor
    class Cursor
    {
        protected int row = 0;
        protected int rowOffset;
        protected int column = 0;
        protected int columnOffset;
        public int Row { get { return row; } }
        public int Column { get { return column; } }

        public Cursor(CursorPosition rootPosition, CursorPosition position = null)
        {
            rowOffset = rootPosition.Row;
            columnOffset = rootPosition.Column;

            if (position != null)
            {
                row = position.Row;
                column = position.Column;
            }
            Update();
        }

        protected void Update()
        {
            Console.SetCursorPosition(Column +  columnOffset, Row + rowOffset);
        }

        public void MoveLeft()
        {
            column--;
            Update();
        }

        public void MoveRight()
        {
            column++;
            Update();
        }

        public void MoveUp(int columnOverride)
        {
            row--;
            column = columnOverride;
            Update();
        }

        public void MoveDown(int columnOverride)
        {
            row++;
            column = columnOverride;
            Update();
        }

        public void MoveToOriginColumn()
        {
            column = 0;
            Update();
        }

        public void MoveToOriginRow(int columnOverride)
        {
            row = 0;
            column = columnOverride;
            Update();
        }

        public void MoveToPosition(int column, int row)
        {
            this.column = column;
            this.row = row;
        }

        public void MoveToColumn(int column)
        {
            this.column = column;
        }
    }
}