using System.Windows;
using System.Windows.Input;

namespace WpfDragAndDrop
{
    public partial class Target : Freezable
    {
        protected internal FrameworkElement associatedElement = new();

        public static readonly DependencyProperty DragDropGroupsProperty =
            DependencyProperty.RegisterAttached("DragDropGroups", typeof(DragDropGroupCollection), typeof(Target), new FrameworkPropertyMetadata());

        public static readonly DependencyProperty DragEnterCommandProperty =
            DependencyProperty.Register(nameof(DragEnterCommand), typeof(ICommand), typeof(Target), new PropertyMetadata(null));

        public static readonly DependencyProperty DragEnterCommandParameterProperty =
            DependencyProperty.Register(nameof(DragEnterCommandParameter), typeof(object), typeof(Target), new PropertyMetadata(null));

        public static readonly DependencyProperty DragLeaveCommandProperty =
            DependencyProperty.Register(nameof(DragLeaveCommand), typeof(ICommand), typeof(Target), new PropertyMetadata(null));

        public static readonly DependencyProperty DragLeaveCommandParameterProperty =
            DependencyProperty.Register(nameof(DragLeaveCommandParameter), typeof(object), typeof(Target), new PropertyMetadata(null));

        public static readonly DependencyProperty TargetObjectProperty =
            DependencyProperty.Register("TargetObject", typeof(object), typeof(Target), new PropertyMetadata(null));

        public Target() => SetDragDropGroups(this, new DragDropGroupCollection());

        public ICommand? DragEnterCommand
        {
            get => (ICommand?)GetValue(DragEnterCommandProperty);
            set => SetValue(DragEnterCommandProperty, value);
        }

        public object? DragEnterCommandParameter
        {
            get => GetValue(DragEnterCommandParameterProperty);
            set => SetValue(DragEnterCommandParameterProperty, value);
        }

        public ICommand? DragLeaveCommand
        {
            get => (ICommand?)GetValue(DragLeaveCommandProperty);
            set => SetValue(DragLeaveCommandProperty, value);
        }

        public object? DragLeaveCommandParameter
        {
            get => GetValue(DragLeaveCommandParameterProperty);
            set => SetValue(DragLeaveCommandParameterProperty, value);
        }

        public object? TargetObject
        {
            get => GetValue(TargetObjectProperty);
            set => SetValue(TargetObjectProperty, value);
        }

        public static DragDropGroupCollection GetDragDropGroups(DependencyObject obj) => (DragDropGroupCollection)obj.GetValue(DragDropGroupsProperty);
        public static void SetDragDropGroups(DependencyObject obj, DragDropGroupCollection value) => obj.SetValue(DragDropGroupsProperty, value);

        protected override Freezable CreateInstanceCore() => new Target();
    }
}