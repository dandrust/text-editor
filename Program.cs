using System;

namespace TextEditor
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.TreatControlCAsInput = true;
            Editor editor = new Editor();
            KeyPressHandler keyPressHandler = new KeyPressHandler(editor);

            keyPressHandler.listen();
            editor.Exit();
        }
    }
}
