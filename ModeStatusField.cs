using System;

namespace TextEditor
{
    class ModeStatusField : Field
    {
        public void HandleModeChangedEvent(object sender, EditorModeEventArgs e)
        {
            string mode = e.Mode == Editor.EditorMode.Command ? "COMMAND" : "EDIT";
            Update(mode);
        }
    }
}