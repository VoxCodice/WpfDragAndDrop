using System;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using static System.Windows.UIElement;

namespace WpfDragAndDrop
{
    public partial class Draggable : Freezable
    {
        protected internal FrameworkElement associatedElement = new();

        private readonly RoutedEventHandler lostCaptureHandler;
        private readonly RoutedEventHandler dragStartHandler;
        private readonly RoutedEventHandler dragDelayCancellationHandler;
        private readonly RoutedEventHandler dragEventHandler;
        private readonly RoutedEventHandler dragStopHandler;

        private bool dragging = false;
        private bool shouldCancel = true;
        private Point startPoint;
        private FrameworkElement? dragPreviewElement;
        private Point initialPosition;
        private InputDevice inputDevice = Mouse.PrimaryDevice; // ToDo: test what this does when no mouse is connected!
        private DragInitiator currentInitiator = DragInitiator.Any;
        private Target? currentDragTarget = null;
        private Target? newDragTarget = null;

        public Draggable()
        {
            SetDragDropGroups(this, new DragDropGroupCollection());
            lostCaptureHandler = new(OnLostCapture);
            dragStartHandler = new(OnStartDrag);
            dragDelayCancellationHandler = new(OnDragDelayCancellation);
            dragEventHandler = new(OnDrag);
            dragStopHandler = new(OnDragStop);
        }
        protected internal void Initialize()
        {
            Detach();
            Attach();
        }

        protected override Freezable CreateInstanceCore() => new Draggable();

        private void Attach()
        {
            switch (Initiator)
            {
                case DragInitiator.Any:
                    associatedElement?.AddHandler(PreviewTouchDownEvent, dragStartHandler, true);
                    associatedElement?.AddHandler(PreviewMouseDownEvent, dragStartHandler, true);
                    return;
                case DragInitiator.Mouse:
                    associatedElement?.AddHandler(PreviewMouseDownEvent, dragStartHandler, true);
                    return;
                case DragInitiator.LeftMouse:
                    associatedElement?.AddHandler(PreviewMouseLeftButtonDownEvent, dragStartHandler, true);
                    return;
                case DragInitiator.RightMouse:
                    associatedElement?.AddHandler(PreviewMouseRightButtonDownEvent, dragStartHandler, true);
                    return;
                case DragInitiator.Touch:
                    associatedElement?.AddHandler(PreviewTouchDownEvent, dragStartHandler, true);
                    return;
            }
        }

        private void Detach()
        {
            associatedElement?.RemoveHandler(PreviewTouchDownEvent, dragStartHandler);
            associatedElement?.RemoveHandler(PreviewMouseDownEvent, dragStartHandler);
            associatedElement?.RemoveHandler(PreviewMouseLeftButtonDownEvent, dragStartHandler);
            associatedElement?.RemoveHandler(PreviewMouseRightButtonDownEvent, dragStartHandler);
        }

        private void OnLostCapture(object? sender, RoutedEventArgs e)
        {
            if (dragging)
                CaptureDevice();
        }

        private void OnStartDrag(object? sender, RoutedEventArgs e)
        {
            if (dragging)
                return;

            if (Container == null)
                throw new NullReferenceException("Container canvas is null! The container property must be set.");

            if (associatedElement == null)
                throw new NullReferenceException("Associated element is null! The draggable control must be placed within a valid framework element.");

            SetCurrentInitiator(e);
            inputDevice = ((InputEventArgs)e).Device;

            if (Delay == 0)
            {
                StartDragging(e);
                return;
            }

            shouldCancel = false;
            var initialPoint = GetPosition(e, Container);
            associatedElement.AddHandler(PreviewMouseUpEvent, dragDelayCancellationHandler, true);

            Task.Delay(Delay).ContinueWith(t => Dispatcher.Invoke(() =>
            {
                associatedElement.RemoveHandler(PreviewMouseUpEvent, dragDelayCancellationHandler);

                // There might be a race condition on this boolean but,
                // being a boolean, it would be a benign race.
                if (shouldCancel)
                    return;

                var point = GetPosition(e, Container);
                if (Point.Subtract(initialPoint, point).Length <= DelayThreshold)
                    StartDragging(e);
            }));
        }

        private void OnDragDelayCancellation(object? sender, RoutedEventArgs e)
        {
            associatedElement.RemoveHandler(PreviewMouseUpEvent, dragDelayCancellationHandler);
            shouldCancel = true;
        }

        private void SetCurrentInitiator(RoutedEventArgs e)
        {
            if (Initiator != DragInitiator.Any)
            {
                currentInitiator = Initiator;
                return;
            }

            currentInitiator = e switch
            {
                MouseButtonEventArgs mbea => mbea.ChangedButton switch
                {
                    MouseButton.Left => DragInitiator.LeftMouse,
                    MouseButton.Right => DragInitiator.RightMouse,
                    _ => DragInitiator.Mouse,
                },
                TouchEventArgs => DragInitiator.Touch,
                _ => throw new ArgumentOutOfRangeException(nameof(e), e, "Cannot set current initiator due to invalid event args!")
            };
        }

        private Point GetPosition(EventArgs e, FrameworkElement relativeTo)
        {
            return currentInitiator switch
            {
                DragInitiator.Mouse or DragInitiator.LeftMouse or DragInitiator.RightMouse => ((MouseEventArgs)e).GetPosition(relativeTo),
                DragInitiator.Touch => ((TouchEventArgs)e).GetTouchPoint(relativeTo).Position,
                _ => throw new ArgumentOutOfRangeException(nameof(currentInitiator), currentInitiator, "Drag Initiator property invalid!"),
            };
        }

        private void StartDragging(EventArgs e)
        {
            DragStartCommand?.Execute(DragStartCommandParameter);

            SetDragPreviewElement();

            initialPosition = associatedElement.TranslatePoint(new(0, 0), Container);
            initialPosition.X -= associatedElement.Margin.Left;
            initialPosition.Y -= associatedElement.Margin.Top; //ToDo: why do I substract the margin?

            Canvas.SetLeft(dragPreviewElement, initialPosition.X);
            Canvas.SetTop(dragPreviewElement, initialPosition.Y);
            Container.Children.Add(dragPreviewElement);

            startPoint = GetPosition(e, Container);

            switch (currentInitiator)
            {
                case DragInitiator.Touch:
                    associatedElement.AddHandler(PreviewTouchMoveEvent, dragEventHandler, true);
                    associatedElement.AddHandler(PreviewTouchUpEvent, dragStopHandler, true);
                    break;

                case DragInitiator.Mouse:
                case DragInitiator.LeftMouse:
                case DragInitiator.RightMouse:
                    associatedElement.AddHandler(PreviewMouseMoveEvent, dragEventHandler, true);
                    associatedElement.AddHandler(PreviewMouseUpEvent, dragStopHandler, true);
                    break;
            }

            CaptureDevice();
        }

        private void OnDrag(object? sender, EventArgs e)
        {
            ((InputEventArgs)e).Handled = true;
            DragPreviewElement(GetPosition(e, Container));
            PerformHitTest(GetPosition(e, Window.GetWindow(associatedElement)));
        }

        private void DragPreviewElement(Point point)
        {
            var deltaX = point.X - startPoint.X;
            var deltaY = point.Y - startPoint.Y;
            Canvas.SetLeft(dragPreviewElement, initialPosition.X + deltaX);
            Canvas.SetTop(dragPreviewElement, initialPosition.Y + deltaY);
        }

        private void PerformHitTest(Point point)
        {
            newDragTarget = null;
            VisualTreeHelper.HitTest(Window.GetWindow(associatedElement),
                                     null,
                                     new(HitTestResultCallback),
                                     new PointHitTestParameters(point));

            if (newDragTarget != null)
            {
                if (currentDragTarget != null)
                    OnDragLeft();
                OnDragEntered();
            }
            else if (currentDragTarget != null)
                OnDragLeft();
        }

        private HitTestResultBehavior HitTestResultCallback(HitTestResult hitTestResult)
        {
            var element = hitTestResult.VisualHit;
            newDragTarget = WpfTrueDragAndDrop.GetTarget(element);
            while (element != null && newDragTarget == null)
            {
                element = VisualTreeHelper.GetParent(element);
                if (element != null)
                    newDragTarget = WpfTrueDragAndDrop.GetTarget(element);
            }

            return newDragTarget == null ? HitTestResultBehavior.Continue : HitTestResultBehavior.Stop;
        }

        private void OnDragEntered()
        {
            if (newDragTarget is null)
                return;

            if (newDragTarget == currentDragTarget)
                return;

            var targetGroups = Target.GetDragDropGroups(newDragTarget);
            var groups = GetDragDropGroups(this);
            if (!targetGroups.Any(tg => groups.Any(g => g.Key == tg.Key)))
                return;

            currentDragTarget = newDragTarget;
            currentDragTarget.DragEnterCommand?.Execute(currentDragTarget.DragEnterCommandParameter);
        }

        private void OnDragLeft()
        {
            currentDragTarget?.DragLeaveCommand?.Execute(currentDragTarget.DragLeaveCommandParameter);
            currentDragTarget = null;
        }

        private void OnDragStop(object? sender, EventArgs e)
        {
            ReleaseDevice();

            switch (currentInitiator)
            {
                case DragInitiator.Touch:
                    associatedElement.RemoveHandler(PreviewTouchMoveEvent, dragEventHandler);
                    associatedElement.RemoveHandler(PreviewTouchUpEvent, dragStopHandler);
                    break;

                case DragInitiator.Mouse:
                case DragInitiator.LeftMouse:
                case DragInitiator.RightMouse:
                    associatedElement.RemoveHandler(PreviewMouseMoveEvent, dragEventHandler);
                    associatedElement.RemoveHandler(PreviewMouseUpEvent, dragStopHandler);
                    break;
            }

            Container?.Children.Remove(dragPreviewElement);
            dragPreviewElement = null;

            var dragComplete = false;
            object? targetObject = null;
            if (currentDragTarget != null)
            {
                dragComplete = true;
                targetObject = currentDragTarget.TargetObject;
                OnDragLeft();
            }

            DragStopCommand?.Execute(DragStopCommandParameter);
            if (dragComplete)
                DragCompleteCommand?.Execute(new DragCompleteParams(DraggableObject, targetObject));
        }

        private void CaptureDevice()
        {
            dragging = true;
            var canCapture = false;

            switch (currentInitiator)
            {
                case DragInitiator.Mouse:
                case DragInitiator.LeftMouse:
                case DragInitiator.RightMouse:
                    associatedElement.AddHandler(LostMouseCaptureEvent, lostCaptureHandler, true);
                    canCapture = associatedElement.CaptureMouse();
                    break;
                case DragInitiator.Touch:
                    associatedElement.AddHandler(LostTouchCaptureEvent, lostCaptureHandler, true);
                    canCapture = associatedElement.CaptureTouch((TouchDevice)inputDevice);
                    break;
            }

            if (canCapture)
                return;

            dragging = false;
            System.Diagnostics.Trace.WriteLine("WpfDragDrop: Could not capture mouse or touch!");
        }

        private void ReleaseDevice()
        {
            if (!dragging)
                return;

            dragging = false;
            associatedElement.RemoveHandler(LostTouchCaptureEvent, lostCaptureHandler);
            associatedElement.RemoveHandler(LostMouseCaptureEvent, lostCaptureHandler);
            associatedElement.ReleaseMouseCapture();
            associatedElement.ReleaseAllTouchCaptures();
        }

        private void SetDragPreviewElement()
        {
            var visualBrush = new VisualBrush(associatedElement) { TileMode = TileMode.None };
            dragPreviewElement = new Rectangle()
            {
                Fill = visualBrush,
                Width = associatedElement.ActualWidth,
                Height = associatedElement.ActualHeight,
                Margin = associatedElement.Margin
            };
        }
    }
}