using System;

namespace TextEditor
{
    class EditorModeEventArgs : EventArgs
    {
        protected Editor.EditorMode mode;
        public Editor.EditorMode Mode { get { return mode; } }
        public EditorModeEventArgs(Editor.EditorMode mode)
        {
            this.mode = mode;
        }
    }
}