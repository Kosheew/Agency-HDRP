using System;

namespace SimpleDialogue
{
    [Serializable]
    public struct IntervalChar
    {
        public Char Char;
        public Single IntervalScale;

        public IntervalChar(Char ch, Single intervalScale)
        {
            Char = ch;
            IntervalScale = intervalScale;
        }
    }
}
