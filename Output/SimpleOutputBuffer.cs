using  System;

namespace TextEditor
{
    class SimpleOutputBuffer : OutputBuffer
    {
        protected string content = "";
        
        public void SetContent(string content)
        {
            this.content = content;
        }

        protected override string GenerateContent(int lineIndex)
        {
            return content.PadRight(width);
        }
    }
}