using System;

namespace TextEditor
{
    class CommandModeController : KeyPressController
    {
        public CommandModeController(Editor editor) : base (editor)
        {
            this.editor = editor;
        }

        public override bool Process(ConsoleKeyInfo keyPress)
        {
            if (keyPress.Key == ConsoleKey.Q)
            {
                return false;
            }
            else if (EscapeKey(keyPress))
            {
                editor.ToggleMode();
            }

            return true;
        }
    }
}