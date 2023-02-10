namespace WpfDragAndDrop
{
    public class DragCompleteParams
    {
        public DragCompleteParams(object? draggableObject, object? targetObject)
        {
            DraggableObject = draggableObject;
            TargetObject = targetObject;
        }

        public object? TargetObject { get; }
        public object? DraggableObject { get; }
    }
}