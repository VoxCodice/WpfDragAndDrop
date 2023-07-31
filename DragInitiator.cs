using System;

namespace WpfDragAndDrop
{
    [Flags]
    public enum DragInitiator
    {
        Any        = 0b1111,
        Mouse      = 0b0001,
        LeftMouse  = 0b0010,
        RightMouse = 0b0100,
        Touch      = 0b1000
    }
}