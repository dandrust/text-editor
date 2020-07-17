using System.Collections.Generic;

namespace TextEditor
{
    class EditorBuffer : OutputBuffer
    {
        public int CurrentLineIndex { get { return cursor.Row; } }
        public int CurrentColumnIndex { get { return cursor.Column; } }
        // Use CursorPosition to eliminate need for CurrentLineIndex and  CurrentColumnIndex.
        // Consider returning a named tuple instead (to fix naming  of line/row)
        public CursorPosition CursorPosition { get { return new CursorPosition(cursor.Column, cursor.Row); } }

        // underlying  models   
        protected List<EditorBufferLine> bufferLines = new List<EditorBufferLine>();
        protected TextBuffer textModel;
        protected Cursor cursor;


        public void SetModel(TextBuffer textBuffer)
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

                EditorBufferLine bufferLine = new EditorBufferLine();
                bufferLine.SetContent(text);
                bufferLine.SetDimensions(1, width);
                bufferLine.SetColorConfiguration(colorConfiguration);
                bufferLine.SetPosition(new CursorPosition(0, i + 1));

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
        }

        public void Start()
        {
            cursor = new Cursor(rootPosition);
        }

        public void AdvanceCursor()
        {
            MoveCursorRight();
        }

        public void MoveCursorLeft()
        {
            if (cursor.Column > 0)
            {
                cursor.MoveLeft();
            }
        }

        public void MoveCursorRight()
        {
            // Only allow the cursor to move to the last editable position
            if (cursor.Column < textModel.GetLine(cursor.Row).Length)
            {
                if (cursor.Column < this.width)
                {
                    cursor.MoveRight();
                }

            }
        }

        public void MoveCursorUp()
        {
            if (cursor.Row > 0)
            {
                cursor.MoveUp(getColumnForMoveUp());
            }
        }

        public void MoveCursorDown()
        {
            // If there's actually text on the next line
            if (cursor.Row < textModel.Buffer.Count - 1)
            {
                // And we're not at the end of the buffer
                if (cursor.Row < this.height - 1)
                {
                    cursor.MoveDown(getColumnForMoveDown());
                }
            }
        }

        protected int getColumnForMoveUp()
        {
            int row = cursor.Row < 1 ? 0 : cursor.Row - 1;
            return getColumnForVerticalMove(row);
        }

        protected int getColumnForMoveDown()
        {
            int row = cursor.Row < bufferLines.Count ? cursor.Row + 1 : bufferLines.Count;
            return getColumnForVerticalMove(row);
        }

        protected int getColumnForVerticalMove(int row)
        {
            return bufferLines[row].AllowedCursorColumn(cursor.Column);
        }

        public void NewLine()
        {
            MoveCursorDown();
            MoveCursorToOriginColumn();
        }

        public void MoveCursorToOriginColumn()
        {
            cursor.MoveToOriginColumn();
        }

        public void MoveCursorToFinalColumn()
        {
            MoveCursorToColumn(bufferLines[cursor.Row].FinalColumn);
        }

        public void MoveCursorToOriginRow()
        {
            cursor.MoveToOriginRow(getColumnForMoveUp());
        }

        public void MoveCursorToFinalRow()
        {
            MoveCursorToRow(textModel.Buffer.Count - 1);
        }

        protected void MoveCursorToRow(int row)
        {
            cursor.MoveToPosition(getColumnForVerticalMove(row), row);
        }

        protected void MoveCursorToColumn(int column)
        {
            cursor.MoveToColumn(column);
        }

        // Can we get this to be protected?
        public void MoveCursorTo(int column, int row)
        {
            cursor.MoveToPosition(column, row);
        }
    }
}