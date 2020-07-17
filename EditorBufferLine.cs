using System;

namespace TextEditor
{
    class EditorBufferLine : SimpleOutputBuffer, IUpdatable
    {

        public int Length { get { return width; } }

        public int FinalColumn { get { return content.Length; } }

        public int AllowedCursorColumn(int currentColumn)
        {
            return currentColumn < content.Length ? currentColumn : content.Length;
        }

        public void Update(string content)
        {
            this.content = content;
            Paint();
        }
    }
}