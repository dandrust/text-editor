using System;
using System.Collections.Generic;

namespace TextEditor
{
    class KeyPressHandler
    {
        protected Editor editor;
        protected Dictionary<Editor.EditorMode, KeyPressController> handlerRegistry = new Dictionary<Editor.EditorMode, KeyPressController>();
        public KeyPressHandler(Editor editor)
        {
            this.editor = editor;
        }

        public void registerController(Editor.EditorMode mode, KeyPressController controller)
        {
            handlerRegistry[mode] = controller;
        }

        public void listen()
        {
            while (HandleKeyPress(Console.ReadKey(true))) { }

        }

        protected bool HandleKeyPress(ConsoleKeyInfo keyPress)
        {
            KeyPressController handler;

            if (handlerRegistry.TryGetValue(editor.Mode, out handler))
            {
                return handler.Process(keyPress);
            }

            throw new Exception($"There is no handler configured for {editor.Mode} mode");
        }
    }
}