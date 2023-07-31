namespace WpfDragAndDrop
{
    public class DragEnterParams
    {
        public DragEnterParams(object? targetObject, DragEnterDirection dragEnterDirection)
        {
            TargetObject = targetObject;
            DragEnterDirection = dragEnterDirection;
        }

        public object? TargetObject { get; }

        public DragEnterDirection DragEnterDirection { get; }
    }
}