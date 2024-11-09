using Godot;

public partial class Drag : Node2D
{
    private bool selected = false;
    private Vector2 mouseOffset = Vector2.Zero;

    public override void _Process(double delta)
    {
        if (selected)
        {
            FollowMouse();
        }
    }

    private void FollowMouse()
    {
        GlobalPosition = GetViewport().GetMousePosition() + mouseOffset;
    }


    private void _OnArea2DInputEvent(Node viewport, InputEvent @event, long shapeIdx)//+
    {
        if (@event is InputEventMouseButton mouseEvent && mouseEvent.ButtonIndex == MouseButton.Left)
        {
            if (mouseEvent.Pressed)
            {
                mouseOffset = GlobalPosition - GetViewport().GetMousePosition();
                selected = true;
            }
            else
            {
                selected = false;
            }
        }
    }

    public override void _Ready()
    {
        var area = GetNode<Area2D>("Area2D");
        area.InputEvent += _OnArea2DInputEvent;
    }
}
