using System;
using System.Text;

namespace TextEditor
{
    class CommandBuffer
    {
        protected StringBuilder buffer = new StringBuilder();
        public StringBuilder Buffer { get { return buffer; } }

        // This will end up being a one-line editor buffer
        // It's presentation will need to subscribe to updates
        // Good opportunity to create some interfaces

        // This will be just like EditorBufferLine/StringBuilder textModel

        // Eventually this will be passed to Editor.Dispatch()
    }
}