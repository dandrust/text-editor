using System;

namespace  TextEditor
{
    class Field : SimpleOutputBuffer, IUpdatable
    {
        public void Update(string content)
        {
            this.content = content;
            Paint();
        }
    }
}