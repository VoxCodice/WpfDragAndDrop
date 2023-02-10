namespace WpfDragAndDrop
{
    public enum DragInitiator
    {
        Any        = 0b00001,
        Mouse      = 0b00010,
        LeftMouse  = 0b00100,
        RightMouse = 0b01000,
        Touch      = 0b10000
    }
}