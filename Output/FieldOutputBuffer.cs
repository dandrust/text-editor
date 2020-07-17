using System;
using System.Text;
using System.Collections.Generic;

namespace TextEditor
{
    class FieldOutputBuffer : OutputBuffer
    {
        protected List<string> fields = new List<string>();

        public void AddField(string text)
        {
            fields.Add(text);
        }

        protected override string GenerateContent(int lineIndex)
        {
            string bufferSpace = "   ";
            StringBuilder content = new StringBuilder();

            foreach(string fieldContent in fields)
            {
                content.Append($"{fieldContent}{bufferSpace}");
            }

            string generatedContent = content.ToString();

            return generatedContent.PadRight(width);
        }

    }
}