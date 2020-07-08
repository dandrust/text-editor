using System;

namespace TextEditor 
{
    class ScreenBuffer
    {
        protected int topOffset;
        protected int height;
        protected int width;

        protected int cursorRow;
        protected int cursorColumn;

        public ScreenBuffer(int height, int width, int offset)
        {
            this.height = height;
            this.width = width;
            this.topOffset = offset;
        }
    }
}