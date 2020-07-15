using System;
using System.Text;
using System.Collections.Generic;

namespace TextEditor
{
    class TextBuffer
    {
        protected List<StringBuilder> buffer = new List<StringBuilder>();
        protected List<StatusBuffer> subscribers = new List<StatusBuffer>();

        public TextBuffer()
        {
            buffer.Add(new StringBuilder());
        }

        public void Insert(int lineIndex, int columnIndex, char character)
        {
            if (lineIndex >= buffer.Count) return;
            StringBuilder line = buffer[lineIndex];

            if (columnIndex > line.Length) return;

            // line.Insert(columnIndex, Convert.ToString(columnIndex));
            line.Insert(columnIndex, character);

            notifySubscribers();
        }

        public void Remove(int lineIndex, int columnIndex, int length)
        {
            buffer[lineIndex].Remove(columnIndex, length);
            notifySubscribers();
        }

        public void InsertLine(int lineIndex, string initialText)
        {
            if (lineIndex >= buffer.Count) {
                buffer.Add(new StringBuilder());
            } else {
                buffer.Insert(lineIndex + 1, new StringBuilder(initialText));
            }
            
        }

        public string GetLine(int lineIndex)
        {
            if (lineIndex < buffer.Count) {
                return buffer[lineIndex].ToString();
            } else {
                return "";
            }

        }

        public override string ToString()
        {
            List<string> listOfStrings = buffer.ConvertAll(
                new Converter<StringBuilder, string>((StringBuilder builder) => builder.ToString() )
            );

            string[] arrayOfStrings = listOfStrings.ToArray();

            return String.Join("-", arrayOfStrings);
        }

        public void registerSubscriber(StatusBuffer subscriber)
        {
            this.subscribers.Add(subscriber);
        }

        protected void notifySubscribers()
        {
            this.subscribers.ForEach(delegate (StatusBuffer subscriber) 
            {
                subscriber.update(this.ToString());
            });
        }

    }
}
