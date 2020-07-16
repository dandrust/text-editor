using System;
using System.Text;
using System.Collections.Generic;

namespace TextEditor
{
    class TextBuffer
    {
        protected List<StringBuilder> buffer = new List<StringBuilder>();
        public List<StringBuilder> Buffer { get { return buffer; } }
        protected List<IUpdatable> subscribers = new List<IUpdatable>();

        protected Dictionary<int, List<IUpdatable>> lineSubscriptions = new Dictionary<int, List<IUpdatable>>();

        public TextBuffer()
        {
            buffer.Add(new StringBuilder());
        }

        public void AppendToLine(int lineIndex, string text)
        {
            if (lineIndex >= buffer.Count) return;
            StringBuilder line = buffer[lineIndex];
            line.Append(text);
            notifySubscribers(lineIndex);
        }

        public void Insert(int lineIndex, int columnIndex, char character)
        {
            if (lineIndex >= buffer.Count) return;
            StringBuilder line = buffer[lineIndex];

            if (columnIndex > line.Length) return;

            // line.Insert(columnIndex, Convert.ToString(columnIndex));
            line.Insert(columnIndex, character);

            notifySubscribers(lineIndex);
        }

        public void Remove(int lineIndex, int columnIndex, int length)
        {
            buffer[lineIndex].Remove(columnIndex, length);
            notifySubscribers(lineIndex);
        }

        public void InsertLine(int lineIndex, string initialText)
        {
            if (lineIndex >= buffer.Count)
            {
                buffer.Add(new StringBuilder());
            }
            else
            {
                buffer.Insert(lineIndex + 1, new StringBuilder(initialText));
            }

            do
            {
                notifySubscribers(lineIndex++);
            } while (lineIndex <= buffer.Count);
        }

        public void RemoveLine(int lineIndex)
        {
            buffer.RemoveAt(lineIndex);

            do
            {
                notifySubscribers(lineIndex++);
            } while (lineIndex <= buffer.Count);
        }

        public string GetLine(int lineIndex)
        {
            if (lineIndex < buffer.Count)
            {
                return buffer[lineIndex].ToString();
            }
            else
            {
                return "";
            }

        }

        public override string ToString()
        {
            List<string> listOfStrings = buffer.ConvertAll(
                new Converter<StringBuilder, string>((StringBuilder builder) => builder.ToString())
            );

            string[] arrayOfStrings = listOfStrings.ToArray();

            return String.Join("-", arrayOfStrings);
        }

        public void registerSubscriber(IUpdatable subscriber)
        {
            this.subscribers.Add(subscriber);
        }

        public void SubscribeToLine(int lineIndex, IUpdatable subscriber)
        {
            List<IUpdatable> subscriberList = null;

            if (lineSubscriptions.TryGetValue(lineIndex, out subscriberList))
            {
                subscriberList.Add(subscriber);
            }
            else
            {
                subscriberList = new List<IUpdatable>();
                subscriberList.Add(subscriber);
                lineSubscriptions.Add(lineIndex, subscriberList);
            }
        }

        protected void notifySubscribers(int lineIndex)
        {
            subscribers.ForEach(delegate (IUpdatable subscriber)
            {
                subscriber.Update(this.ToString());
            });

            List<IUpdatable> subscriberList = null;
            if (lineSubscriptions.TryGetValue(lineIndex, out subscriberList))
            {
                subscriberList.ForEach(delegate (IUpdatable subscriber)
                {
                    subscriber.Update(GetLine(lineIndex));
                });
            }
        }

    }
}
