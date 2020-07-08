using System;

namespace TextEditor
{
    class EditorBuffer
    {
        protected int height;
        protected int width;
        protected CursorPosition rootPosition;
        protected int currentCursorRow;
        protected int currentCursorColumn;
        protected ColorConfiguration colorConfiguration;

        public EditorBuffer(int height, int width)
        {
            this.height = height;
            this.width = width;
        }

        public EditorBuffer AtPosition(CursorPosition position)
        {
            rootPosition = position;
            return this;
        }

        public EditorBuffer WithColorConfiguration(ColorConfiguration configuration)
        {
            colorConfiguration = configuration;
            return this;
        }

        public void Start()
        {
            currentCursorRow = rootPosition.GetRow();
            currentCursorColumn = rootPosition.GetColumn();

            UpdateCursor();
        }

        public void UpdateCursor()
        {
            Console.SetCursorPosition(currentCursorColumn, currentCursorRow);
        }

        public void MoveCursorLeft()
        {
            if (getRelativeColumn() > 0) {
                currentCursorColumn--;
                UpdateCursor();
            }
        }

        public void MoveCursorRight()
        {
            if (getRelativeColumn() < this.width) {
                currentCursorColumn++;
                UpdateCursor();
            }
        }

        public void MoveCursorUp()
        {
            if (getRelativeRow() > 0) {
                currentCursorRow--;
                UpdateCursor();
            }
        }

        public void MoveCursorDown()
        {
            if (getRelativeRow() < this.height - 1) {
                currentCursorRow++;
                UpdateCursor();
            }
        }

        protected int getRelativeRow()
        {
            return currentCursorRow - rootPosition.GetRow();
        }

        protected int getRelativeColumn()
        {
            return currentCursorColumn - rootPosition.GetColumn();
        }

        public int GetCurrentLineIndex()
        {
            return getRelativeRow();
        }

        public int GetCurrentColumnIndex()
        {
            return getRelativeColumn();
        }
    }
}