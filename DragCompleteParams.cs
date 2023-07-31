namespace WpfDragAndDrop
{
    public class DragCompleteParams
    {
        public DragCompleteParams(DragEnterDirection dragEnterDirection, object? draggableObject, object? targetObject)
        {
            DragEnterDirection = dragEnterDirection;
            DraggableObject = draggableObject;
            TargetObject = targetObject;
        }

        public object? TargetObject { get; }
        public DragEnterDirection DragEnterDirection { get; }
        public object? DraggableObject { get; }
    }
}