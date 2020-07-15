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
            currentCursorRow = rootPosition.Row;
            currentCursorColumn = rootPosition.Column;

            UpdateCursor();
        }

        public void UpdateCursor()
        {
            Console.SetCursorPosition(currentCursorColumn, currentCursorRow);
        }

        public void AdvanceCursor()
        {
            MoveCursorRight();
        }

        public void MoveCursorLeft()
        {
            if (relativeColumn > 0)
            {
                currentCursorColumn--;
                UpdateCursor();
            }
        }

        public void MoveCursorRight()
        {
            if (relativeColumn < this.width)
            {
                currentCursorColumn++;
                UpdateCursor();
            }
        }

        public void MoveCursorUp()
        {
            if (relativeRow > 0)
            {
                currentCursorRow--;
                UpdateCursor();
            }
        }

        public void MoveCursorDown()
        {
            if (relativeRow < this.height - 1)
            {
                currentCursorRow++;
                UpdateCursor();
            }
        }

        public void NewLine()
        {
            MoveCursorDown();
            MoveCursorToOriginColumn();
        }

        public void MoveCursorToOriginColumn()
        {
            currentCursorColumn = rootPosition.Column;
            UpdateCursor();
        }

        protected int relativeRow { get { return currentCursorRow - rootPosition.Row; } }

        protected int relativeColumn { get { return currentCursorColumn - rootPosition.Column; } }

        public int CurrentLineIndex { get { return relativeRow; } }

        public int CurrentColumnIndex { get { return relativeColumn; } }
    }
}