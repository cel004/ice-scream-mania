using Godot;

public partial class ConeDrag : Node2D
{
    private bool _selected = false;
    private Vector2 mouseOffset = Vector2.Zero;

    public override void _Process(double delta)     // if mouse left clicks = object follows mouse
    {
        if (_selected)
        {
            FollowMouse();
        }
    }

    private void FollowMouse()
    {
        GlobalPosition = GetViewport().GetMousePosition() + mouseOffset;    // pos = pos of mouse + mouse offset -> smooth dragging
    }

    private void _OnArea2DInputEvent(Node viewport, InputEvent @event, long shapeIdx)//+
    {
        if (@event is InputEventMouseButton mouseEvent && mouseEvent.ButtonIndex == MouseButton.Left)
        {
            if (mouseEvent.Pressed)
            {
                mouseOffset = GlobalPosition - GetViewport().GetMousePosition();
                _selected = true;
            }
            else{
                // not selected
                _selected = false;
            }
        }
    }

    public override void _Ready()
    {
        var area = GetNode<Area2D>("pileOfCones");
        area.InputEvent += _OnArea2DInputEvent;
    }
}