using System;
using System.Collections.Generic;

namespace TextEditor
{
    class EditorBuffer
    {
        protected int height;
        protected int width;
        protected CursorPosition rootPosition;
        protected int currentCursorRow;
        protected int currentCursorColumn;
        protected int relativeRow { get { return currentCursorRow - rootPosition.Row; } }
        protected int relativeColumn { get { return currentCursorColumn - rootPosition.Column; } }
        protected ColorConfiguration colorConfiguration;
        protected List<EditorBufferLine> bufferLines = new List<EditorBufferLine>();
        public int CurrentLineIndex { get { return relativeRow; } }
        public int CurrentColumnIndex { get { return relativeColumn; } }
        protected TextBuffer textModel;

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

        public EditorBuffer WithModel(TextBuffer textBuffer)
        {
            textModel = textBuffer;

            // Initialize a buffer line for each available line in the editor
            for (int i = 0; i < height; i++)
            {

                // Is there a real line for this editor line in the underlying model?
                // If so, grab the text.  Otherwise, return an empty string
                string text = textBuffer.Buffer.Count > i ?
                    textBuffer.GetLine(i) :
                    "";

                EditorBufferLine bufferLine = new EditorBufferLine()
                    .WithText(text)
                    .OfLength(width)
                    .AtPosition(new CursorPosition(0, i + 1));

                // Add the new buffer line to the line subscription list to get updates
                // The observer pattern!
                textBuffer.SubscribeToLine(i, bufferLine);

                bufferLines.Add(bufferLine);
            }

            // Paint the lines we've just created
            foreach (EditorBufferLine line in bufferLines)
            {
                line.Paint();
            }

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
            // Only allow the cursor to move to the last editable position
            if (relativeColumn < textModel.GetLine(relativeRow).Length)
            {
                if (relativeColumn < this.width)
                {
                    currentCursorColumn++;
                    UpdateCursor();
                }

            }
        }

        public void MoveCursorUp()
        {
            if (relativeRow > 0)
            {
                currentCursorRow--;
                currentCursorColumn = bufferLines[relativeRow].AllowedCursorColumn(relativeColumn);
                UpdateCursor();
            }
        }

        public void MoveCursorDown()
        {
            // If there's actually text on the next line
            if (relativeRow < textModel.Buffer.Count - 1)
            {
                // And we're not at the end of the buffer
                if (relativeRow < this.height - 1)
                {
                    currentCursorRow++;
                    currentCursorColumn = bufferLines[relativeRow].AllowedCursorColumn(relativeColumn);
                    UpdateCursor();
                }
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

        public void MoveCursorToFinalColumn()
        {
            currentCursorColumn = bufferLines[relativeRow].FinalColumn;
            UpdateCursor();
        }

        public void MoveCursorToOriginRow()
        {
            currentCursorRow = 0;
            currentCursorColumn = bufferLines[relativeRow].AllowedCursorColumn(relativeColumn);
            UpdateCursor();
        }

        public void MoveCursorToFinalRow()
        {
            currentCursorRow = textModel.Buffer.Count - 1;
            currentCursorColumn = bufferLines[relativeRow].AllowedCursorColumn(relativeColumn);
        }

        public void MoveCursorTo(int column, int row)
        {
            currentCursorColumn = column;
            currentCursorRow = row;

            UpdateCursor();
        }
    }
}