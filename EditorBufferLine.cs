using System;

namespace TextEditor
{
    class EditorBufferLine : IUpdatable
    {
        protected int length;
        public int Length { get { return length; } }
        protected CursorPosition rootPosition;
        protected string text = "";
        public int FinalColumn { get { return text.Length; } }


        public EditorBufferLine OfLength(int length)
        {
            this.length = length;
            return this;
        }

        public EditorBufferLine WithText(string text)
        {
            this.text = text;
            return this;
        }

        public EditorBufferLine AtPosition(CursorPosition position)
        {
            this.rootPosition = position;
            return this;
        }

        public int AllowedCursorColumn(int currentColumn)
        {
            return currentColumn < text.Length ? currentColumn : text.Length;
        }

        public void Paint()
        {
            Console.SetCursorPosition(rootPosition.Column, rootPosition.Row);
            Console.Write(text.PadRight(length));
        }

        public void Update(string text)
        {
            this.text = text;
            Paint();
        }
    }
}