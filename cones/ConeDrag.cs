using Godot;

public partial class ConeDrag : Node2D
{
    private bool _selected = false;
    private Vector2 mouseOffset = Vector2.Zero;

    private Node2D instance;    // creates new instance of cone when clicked
    private Area2D _pileOfCones;  // store reference to pileOfCones
    private PackedScene cone;   // packed scene for cone (draggableCone.tscn)

    public void SetPileOfCones(Area2D pileOfCones)
    {
        _pileOfCones = pileOfCones;
    }

    public override void _Process(double delta)     // if mouse left clicks = object follows mouse
    {
        if (_selected && instance != null)
        {
            FollowMouse();
        }
    }

    private void FollowMouse()
    {
        instance.GlobalPosition = GetViewport().GetMousePosition() + mouseOffset;    // pos = pos of mouse + mouse offset -> smooth dragging
    }

    private void _OnArea2DInputEvent(Node viewport, InputEvent @event, long shapeIdx)
    {
        if (@event is InputEventMouseButton mouseEvent && mouseEvent.ButtonIndex == MouseButton.Left)
        {
            if (mouseEvent.Pressed && instance == null && cone != null)
            {
                instance = (Node2D)cone.Instantiate();  // creates new instance of cone when clicked
                AddChild(instance);

                mouseOffset = GlobalPosition - GetViewport().GetMousePosition();    // offset for smooth dragging
                _selected = true;
            }
            else if(!mouseEvent.Pressed && instance!= null)
            {
                instance.QueueFree();   // removes cone from scene
                instance = null;
                _selected = false;
                // GD.Print("Cone scene is null");   // for debugging
            }
        }
    }

    public override void _Ready()
    {
        cone = GD.Load<PackedScene>("res://cones/draggableCone.tscn");  // loads cone scene
        _pileOfCones = GetNodeOrNull<Area2D>("/root/main/conePile/pileOfCones");    // reference to pileOfCones in the scene
        _pileOfCones.InputEvent += _OnArea2DInputEvent;
        

        // if (cone == null)
        // {
        //     GD.PrintErr("Failed to load 'draggableCone.tscn'. Please check the file path.");
        // }

        // if (_pileOfCones == null)
        // {
        //     GD.PrintErr("pileOfCones not found. Please check the node path.");
        // }
        // else
        // {
        //     GD.Print(_pileOfCones); // debugging: prints reference to pileOfCones
        //     _pileOfCones.InputEvent += _OnArea2DInputEvent;
        // }
    }
}
