# WpfDragAndDrop

**WpfDragAndDrop** is a library for adding drag-and-drop functionality to your WPF applications. 

## Features

*   Touchscreen enabled.
*   Drag and drop support for UI elements within a single application window.
*   Customizable drag sources and drop targets.
*   Commands for tracking drag start, drag enter, drag leave, and drop actions.
*   Support for dragging data objects between UI elements.

## Installation

In order to use this library, please download the source code and build the library manually and add it as a reference to your project.

## Usage

Follow these simple steps to get started with the **WpfDragAndDrop** library:

1.  Add a reference to the **WpfDragAndDrop** project or the built `WpfDragAndDrop.dll` in your WPF project.

2.  Import the library namespace into your XAML file:

    ```xml
    xmlns:dragDrop="clr-namespace:WpfDragAndDrop;assembly=WpfDragAndDrop"
    ```

3.  Add a canvas:
    ```xml
    <Window ...>
        ...
        <Canvas x:Name="DragDropCanvas" Grid.Row="0" Grid.Column="0" Grid.ColumnSpan="2" ClipToBounds="True"></Canvas>
        ...
    </Window>
    ```
    
4.  Add the drag and drop attributes to the elements you want to drag:
    ```xml
    <Button x:Name="My Fancy Draggable Button>
        ...
        <dragDrop:WpfTrueDragAndDrop.Draggable>
            <dragDrop:Draggable Delay="500" 
                                DelayThreshold="20"
                                Container="{Binding ElementName=DragDropCanvas}"
                                DraggableObject="{Binding DragData}"
                                DragStartCommand="{Binding DragStartCommand}"
                                DragStartCommandParameter="{Binding DragStartData}"
                                DragStopCommand="{Binding DragStopCommand}"
                                DragStopCommandParameter="{Binding DragStopData}"
                                DragCompleteCommand="{Binding DragCompleteCommand}"
                                DragCompleteCommandParameter="{Binding DragCompleteData}">
            </dragDrop:Draggable>
        </dragDrop:WpfTrueDragAndDrop.Draggable>
        ...
    </Button>
    ```

5.  Specify the group(s) that this draggble belongs to:
    ```xml
    <dragDrop:WpfTrueDragAndDrop.Draggable ...>
        <dragDrop:Draggable.DragDropGroups>
            <dragDrop:DragDropGroup Key="Group1"/>
            <dragDrop:DragDropGroup Key="Group2"/>
        </dragDrop:Draggable.DragDropGroups>
    </dragDrop:WpfTrueDragAndDrop.Draggable>
    ```

6.  Choose a drag and drop target (or targets) by adding the attributes to the target elements:
    ```xml
    <Grid x:Name="My Fancy Drag Drop Target">
        ...
        <dragDrop:WpfTrueDragAndDrop.Target>
            <dragDrop:Target DragEnterCommand="{Binding DragEnterCommand}"
                             DragEnterCommandParameter="{Binding DragEntedData}"
                             DragLeaveCommand="{Binding DragLeaveCommand}"
                             DragLeaveCommandParameter="{Binding DragLeaveData}"
                             TargetObject="{Binding DragTargetData}">
            </dragDrop:Target>
        </dragDrop:WpfTrueDragAndDrop.Target>
        ...
    </Grid>
    ```
    
7.  Specify the group(s) that this target belongs to:
    ```xml
    <dragDrop:WpfTrueDragAndDrop.Target>
        <dragDrop:Target ...>
            <dragDrop:Target.DragDropGroups>
                <dragDrop:DragDropGroup Key="Group1" />
            </dragDrop:Target.DragDropGroups>
        </dragDrop:Target>
    </dragDrop:WpfTrueDragAndDrop.Target>
    ```
    
8.  Bind the various commands to your view model.

## Sample
Coming soon.

## API Reference

### Draggable
Used to specify that an object is draggable and configure it's behavior.

The class exposes the following dependency properties.

<table>
<thead>
<tr>
<th> Dependency Property </th>
<th> Type                </th>
<th> Default             </th>
<th> Description         </th>
<th> Example Usage       </th>
</tr>
</thead>
<tbody>
<tr>
<td> Container </td> <td>

`Canvas`

</td> <td> </td> <td> Canvas on which the dragged element is painted. </td>
<td> 

```xml
<dragDrop:Draggable Container="{Binding ElementName=canvasName}"/>
```

</td>
</tr>
<tr>
<td> Initiator </td> <td>

`DragInitiator`

</td> <td> 

`DragInitiator.Any` 

</td> <td> 

(Optional) Value indicating whether dragging is enabled for mouse, touch, or both. You can use the `DragInitiator` enum to set this property. 

</td>
<td> 

```xml
<!-- Single value -->
<dragDrop:Draggable Initiator="LeftMouse"/>

<!-- Or multiple values -->
<dragDrop:Draggable.Initiator>
    <dragDrop:DragInitiator> Touch, RightMouse </dragDrop:DragInitiator>
</dragDrop:Draggable.Initiator>
```

</td>
</tr>
<tr>
<td> Delay </td> <td>

`int`

</td> <td> 0 </td> <td> (Optional) Duration (in milliseconds) for which the user must hold the mouse or touch down before the drag is initiated. </td>
<td> 

```xml
<dragDrop:Draggable Delay="500"/>
```

</td>
</tr>
<tr>
<td> DelayThreshold </td> <td>

`int`

</td> <td> 0 </td> <td> (Optional) Maximum allowable movement (in pixels) while holding the mouse or touch down over a draggable before the drag is canceled. </td>
<td> 

```xml
<dragDrop:Draggable DelayThreshold="5"/>
```

</td>
</tr>
<tr>
<td> DragStartCommand </td> <td>

`ICommand`

</td> <td> </td> <td> Command to execute when the drag operation starts. </td>
<td> 

```xml
<dragDrop:Draggable DragStartCommand="{Binding DragStartCommand}"/>
```

</td>
</tr>
<tr>
<td> DragStartCommandParameter </td> <td> 

```csharp
object?
```

</td> <td> </td> <td> 

Command parameter for the `DragStartCommand`. 

</td>
<td> 

```xml
<dragDrop:Draggable DragStartCommandParameter="{Binding DragStartData}"/>
```

</td>
</tr>
<tr>
<td> DragStopCommand </td> <td>

`ICommand`

</td> <td> </td> <td> Command to execute when the drag operation stops. </td>
<td> 

```xml
<dragDrop:Draggable DragStopCommand="{Binding DragStopCommand}"/>
```

</td>
</tr>
<tr>
<td> DragStopCommandParameter </td> <td> 

`object?` 

</td> <td> </td> <td> 

Command parameter for the `DragStopCommand`.

</td>
<td> 

```xml
<dragDrop:Draggable DragStopCommandParameter="{Binding DragStopData}"/>
```

</td>
</tr>
<tr>
<td> DragCompleteCommand </td> <td>

`ICommand`

</td> <td> </td> <td> Command to execute when the drag operation is completed. </td>
<td> 

```xml
<dragDrop:Draggable DragCompleteCommand="{Binding DragCompleteCommand}"/>
```

</td>
</tr>
<tr>
<td> DragCompleteCommandParameter </td> <td> 

`object?` 

</td> <td> </td> <td> 

Command parameter for the `DragCompleteCommand`.

</td>
<td> 

```xml
<dragDrop:Draggable DragCompleteCommandParameter="{Binding DragCompleteData}"/>
```

</td>
</tr>
</tbody>
</table>

The class exposes the following attached dependency properties.

<table>
<thead>
<tr>
<th> Attached Dependency Property </th>
<th> Type </th>
<th> Description </th>
<th> Example Usage </th>
</tr>
</thead>
<tbody>
<tr>
<td> DragDropGroups </td> <td>

`DragDropGroupCollection`

</td> <td> Specifies which group(s) the draggable object belongs to. The object can only be dropped on targets belonging to the same group(s). </td>
<td> 

```xml
<dragDrop:Draggable.DragDropGroups>
    <dragDrop:DragDropGroup Key="Group1"/>
    <dragDrop:DragDropGroup Key="Group2"/>
</dragDrop:Draggable.DragDropGroups>
```

</td>
</tr>
</tbody>
</table>

### Target
Used to specify that an object is a drop target and configure it's behavior.

The class exposes the following dependency properties.

<table>
<thead>
<tr>
<th> Dependency Property </th>
<th> Type </th>
<th> Description </th>
<th> Example Usage </th>
</tr>
</thead>
<tbody>
<tr>
<td> DragEnterCommand </td><td> 

`ICommand`

</td><td> Command to execute when a draggable is dragged over the target object. </td>
<td> 

```xml
<dragDrop:Target DragEnterCommand="{Binding DragEnterCommand}"/>
```

</td>
</tr>
<tr>
<td> DragEnterCommandParameter </td><td>

`DragEnterParams`

</td><td>

Command parameter for the `DragEnterCommand`.

</td>
<td>

```xml
<dragDrop:Target DragEnterCommandParameter="{Binding DragEnterData}"/>
``` 

</td>
</tr>
<tr>
<td> DragLeaveCommand </td><td>

`ICommand`

</td><td> Command to execute when a draggable is dragged outside of or otherwise leaves the target object. </td>
<td>

```xml
<dragDrop:Target DragLeaveCommand="{Binding DragLeaveCommand}"/>
```

</td>
</tr>
<tr>
<td> DragLeaveCommandParameter </td><td>

`DragCompleteParams`

</td><td>

Command parameter for the `DragLeaveCommand`.

</td>
<td>

```xml
<dragDrop:Target DragLeaveCommandParameter="{Binding DragLeaveData}"/>
```

</td>
</tr>
<tr>
<td> TargetObject </td><td>

`object?` 

</td><td> An object that can be used to store data associated with the target object. </td>
<td>

```xml
<dragDrop:Target TargetObject="{Binding DragTargetData}"/>
```

</td>
</tr>
</tbody>
</table>

The class exposes the following attached dependency properties.

<table>
<thead>
<tr>
<th> Attached Dependency Property </th>
<th> Type </th>
<th> Description </th>
<th> Example Usage </th>
</tr>
</thead>
<tbody>
<tr>
<td> DragDropGroups </td> <td>

`DragDropGroupCollection`

</td> <td> Specifies which group(s) the target object belongs to. The object is only a target for draggables belonging to the same group(s). </td>
<td> 

```xml
<dragDrop:Draggable.DragDropGroups>
    <dragDrop:DragDropGroup Key="Group1"/>
    <dragDrop:DragDropGroup Key="Group2"/>
</dragDrop:Draggable.DragDropGroups>
```

</td>
</tr>
</tbody>
</table>

### DragDropGroupCollection
A freezable collection of `DragDropGroup`s. For usage, see above examples.

### DragDropGroup
Use this to specify which group(s) draggable and target objects belong to. You will only be able to drag an object on a target if they belong to the same group(s).

<table>
<thead>
<tr>
<th> Dependency Property </th>
<th> Type </th>
<th> Description </th>
<th> Example Usage </th>
</tr>
</thead>
<tbody>
<tr>
<td> Key </td> <td>

`string`

</td> <td> Uniquely identifies a drag and drop group. </td>
<td> 

```xml
<dragDrop:DragDropGroup Key="Group1"/>
```

</td>
</tr>
</tbody>
</table>

### DragCompleteParams
The `DragCompleteCommand` will pass an instance of this class as a parameter when it is executed.

<table>
<thead>
<tr>
<th> Property </th>
<th> Type </th>
<th> Description </th>
</tr>
</thead>
<tbody>
<tr>
<td> DraggableObject </td>
<td>

`object?`

</td> <td> The data object associated with the draggable that triggered the drag complete command. </td>
</tr>

<tr>
<td> TargetObject </td>
<td>

`object?`

</td> <td> The data object associated with the target that triggered the drag complete command. </td>

</tr>

<tr>
<td> DragEnterDirection </td>
<td>

`DragEnterDirection`

</td> <td> 

A `DragEnterDirection` enum which specifies from what direction an object was dragged on the target.

</td>

</tr>
</tbody>
</table>

### DragEnterParams
The `DragEnterCommand` will pass an instance of this class as a parameter when it is executed.

<table>
<thead>
<tr>
<th> Property </th>
<th> Type </th>
<th> Description </th>
</tr>
</thead>
<tbody>
<tr>
<td> TargetObject </td>
<td>

`object?`

</td> <td> The data object associated with the target that triggered the drag complete command. </td>

</tr>

<tr>
<td> DragEnterDirection </td>
<td>

`DragEnterDirection`

</td> <td> 

A `DragEnterDirection` enum which specifies from what direction an object was dragged on the target.

</td>

</tr>
</tbody>
</table>

#### Usage
```csharp
public class DragEnterCommand : ICommand
{
    public event EventHandler? CanExecuteChanged;

    public bool CanExecute(object? parameter) => true;

    public void Execute(object? parameter)
    {
        if (parameter is not DragEnterParams dragEnterParams)
            return;

        if (dragEnterParams.TargetObject is not SomeDataObject target)
            return;

        // Do something
        Trace.WriteLine($"{target} was entered from {direction}.");
    }
}
```

#### Usage
```csharp
public class DragCompleteCommand : ICommand
{
    public event EventHandler? CanExecuteChanged;

    public bool CanExecute(object? parameter) => true;

    public void Execute(object? parameter)
    {
        if (parameter is not DragCompleteParams dragCompleteParams)
            return;
	    
        if (dragCompleteParams.DraggableObject is not SomeDataObject draggable)
            return;
	    
        if (dragCompleteParams.TargetObject is not SomeOtherDataObject target)
            return;
	    
        var direction = dragCompleteParams.DragEnterDirection;
	    
        // Do something
        Trace.WriteLine($"{draggable} was dragged over {target} from {direction}.");
    }
}
```

### DragEnterDirection
An enum used to represent from which direction a target was entered.

The enum supports bitwise operations as it is marked with the `[Flags]` attribute.

#### Enum values
| Name  | Value    |
|-------|----------|
| North | `0b0001` |
| East  | `0b0010` |
| South | `0b0100` |
| West  | `0b1000` |

### DragInitiator
An enum used to specify which kind of user interaction can trigger a drag and drop event on a draggable object.

The enum supports bitwise operations as it is marked with the `[Flags]` attribute.

#### Enum values
| Name       | Value    |
|------------|----------|
| Any        | `0b1111` |
| Mouse      | `0b0001` |
| LeftMouse  | `0b0010` |
| RightMouse | `0b0100` |
| Touch      | `0b1000` |

## License
Licensed under the <a href="https://raw.githubusercontent.com/VoxCodice/WpfDragAndDrop/master/LICENSE.txt">GPL-3.0 license</a>.