using System.Windows;

namespace WpfDragAndDrop
{
    public class WpfTrueDragAndDrop : Freezable
    {
        public static readonly DependencyProperty DraggableProperty = DependencyProperty.RegisterAttached("Draggable", typeof(Draggable), typeof(WpfTrueDragAndDrop), new PropertyMetadata(null, OnDraggableChanged));
        public static readonly DependencyProperty TargetProperty = DependencyProperty.RegisterAttached("Target", typeof(Target), typeof(WpfTrueDragAndDrop), new PropertyMetadata(null, OnTargetChanged));

        public static Draggable GetDraggable(DependencyObject obj) => (Draggable)obj.GetValue(DraggableProperty);
        public static void SetDraggable(DependencyObject obj, Draggable value) => obj.SetValue(DraggableProperty, value);

        public static Target GetTarget(DependencyObject obj) => (Target)obj.GetValue(TargetProperty);
        public static void SetTarget(DependencyObject obj, Target value) => obj.SetValue(TargetProperty, value);

        protected override Freezable CreateInstanceCore() => new WpfTrueDragAndDrop();

        private static void OnDraggableChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (e.NewValue == null)
                return;

            ((Draggable)e.NewValue).associatedElement = (FrameworkElement)d;
            ((Draggable)e.NewValue).Initialize();
        }

        private static void OnTargetChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (e.NewValue == null)
                return;

            ((Target)e.NewValue).associatedElement = (FrameworkElement)d;
        }
    }
}