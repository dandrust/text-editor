using System;

namespace TextEditor
{
    class Program
    {
        static void Main(string[] args)
        {
            Editor editor = new Editor();

            KeyPressHandler keyPressHandler = new KeyPressHandler(editor);
            keyPressHandler.registerController(Editor.EditorMode.Edit, new EditModeController(editor));
            keyPressHandler.registerController(Editor.EditorMode.Command, new CommandModeController(editor));

            keyPressHandler.listen();
            editor.Exit();
        }
    }
}
