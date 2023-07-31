using System;

namespace WpfDragAndDrop
{
    [Flags]
    public enum DragEnterDirection
    {
        North = 0b0001,
        East  = 0b0010,
        South = 0b0100,
        West  = 0b1000
    }
}