using System.Windows;

namespace WpfDragAndDrop
{
    public class DragDropGroupCollection : FreezableCollection<DragDropGroup> { }

    public class DragDropGroup : Freezable
    {
        public static readonly DependencyProperty KeyProperty = DependencyProperty.Register(nameof(Key), typeof(string), typeof(DragDropGroup), new PropertyMetadata(null));

        public string Key
        {
            get => (string)GetValue(KeyProperty);
            set => SetValue(KeyProperty, value);
        }

        protected override Freezable CreateInstanceCore() => new DragDropGroup();
    }
}