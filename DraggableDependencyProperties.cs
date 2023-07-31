using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace WpfDragAndDrop
{
    public partial class Draggable : Freezable
    {
        public static readonly DependencyProperty DelayProperty = DependencyProperty.Register(nameof(Delay), typeof(int), typeof(Draggable), new PropertyMetadata(0));
        public static readonly DependencyProperty DelayThresholdProperty = DependencyProperty.Register(nameof(DelayThreshold), typeof(int), typeof(Draggable), new PropertyMetadata(0));
        public static readonly DependencyProperty ContainerProperty = DependencyProperty.Register(nameof(Container), typeof(Canvas), typeof(Draggable), new PropertyMetadata(null));
        public static readonly DependencyProperty InitiatorProperty = DependencyProperty.Register(nameof(Initiator), typeof(DragInitiator), typeof(Draggable), new PropertyMetadata(DragInitiator.Any, OnInitiatorChanged));
        public static readonly DependencyProperty DragDropGroupsProperty = DependencyProperty.RegisterAttached("DragDropGroups", typeof(DragDropGroupCollection), typeof(Draggable), new FrameworkPropertyMetadata());

        public static readonly DependencyProperty DragStartCommandProperty = DependencyProperty.Register(nameof(DragStartCommand), typeof(ICommand), typeof(Draggable), new PropertyMetadata(null));
        public static readonly DependencyProperty DragStartCommandParameterProperty = DependencyProperty.Register(nameof(DragStartCommandParameter), typeof(object), typeof(Draggable), new PropertyMetadata(null));
        public static readonly DependencyProperty DragStopCommandProperty = DependencyProperty.Register(nameof(DragStopCommand), typeof(ICommand), typeof(Draggable), new PropertyMetadata(null));
        public static readonly DependencyProperty DragStopCommandParameterProperty = DependencyProperty.Register(nameof(DragStopCommandParameter), typeof(object), typeof(Draggable), new PropertyMetadata(null));
        public static readonly DependencyProperty DragCompleteCommandProperty = DependencyProperty.Register("DragCompleteCommand", typeof(ICommand), typeof(Draggable), new PropertyMetadata(null));
        public static readonly DependencyProperty DragCompleteCommandParameterProperty = DependencyProperty.Register("DragCompleteCommandParameter", typeof(object), typeof(Draggable), new PropertyMetadata(null));

        public int Delay
        {
            get => (int)GetValue(DelayProperty);
            set => SetValue(DelayProperty, value);
        }

        public int DelayThreshold
        {
            get => (int)GetValue(DelayThresholdProperty);
            set => SetValue(DelayThresholdProperty, value);
        }

        public Canvas Container
        {
            get => (Canvas)GetValue(ContainerProperty);
            set => SetValue(ContainerProperty, value);
        }

        public DragInitiator Initiator
        {
            get => (DragInitiator)GetValue(InitiatorProperty);
            set => SetValue(InitiatorProperty, value);
        }

        public ICommand? DragStartCommand
        {
            get => (ICommand?)GetValue(DragStartCommandProperty);
            set => SetValue(DragStartCommandProperty, value);
        }

        public object DragStartCommandParameter
        {
            get => GetValue(DragStartCommandParameterProperty);
            set => SetValue(DragStartCommandParameterProperty, value);
        }

        public ICommand? DragStopCommand
        {
            get => (ICommand?)GetValue(DragStopCommandProperty);
            set => SetValue(DragStopCommandProperty, value);
        }

        public object DragStopCommandParameter
        {
            get => GetValue(DragStopCommandParameterProperty);
            set => SetValue(DragStopCommandParameterProperty, value);
        }

        public ICommand? DragCompleteCommand
        {
            get => (ICommand?)GetValue(DragCompleteCommandProperty);
            set => SetValue(DragCompleteCommandProperty, value);
        }

        public object? DragCompleteCommandParameter
        {
            get => GetValue(DragCompleteCommandParameterProperty);
            set => SetValue(DragCompleteCommandParameterProperty, value);
        }

        public static DragDropGroupCollection GetDragDropGroups(DependencyObject obj) => (DragDropGroupCollection)obj.GetValue(DragDropGroupsProperty);
        public static void SetDragDropGroups(DependencyObject obj, DragDropGroupCollection value) => obj.SetValue(DragDropGroupsProperty, value);

        private static void OnInitiatorChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((Draggable)d).Initiator = (DragInitiator)e.NewValue;
            ((Draggable)d).Initialize();
        }
    }
}