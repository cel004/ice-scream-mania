using Godot;

public partial class ConeDrag : Node2D
{
    private bool _selected = false;
    private Vector2 mouseOffset = Vector2.Zero;

    private Node2D _instance;    // creates new instance of cone when clicked
    private Area2D _pileOfCones;  // store reference to pileOfCones
    private PackedScene _cone;   // packed scene for cone (draggableCone.tscn)

    public void SetPileOfCones(Area2D pileOfCones)
    {
        _pileOfCones = pileOfCones;
    }

    public override void _Process(double delta)     // if mouse left clicks = object follows mouse
    {
        if (_selected && _instance != null)
        {
            FollowMouse();
        }
    }

    private void FollowMouse()
    {
        _instance.GlobalPosition = GetViewport().GetMousePosition() + mouseOffset;    // pos = pos of mouse + mouse offset -> smooth dragging
    }

    private void _createCone(){
        if(_instance != null){
            return;
        }

        _instance = (Node2D)_cone.Instantiate();  // creates new instance of cone when clicked
        AddChild(_instance);

        mouseOffset = _instance.GlobalPosition;    // offset so that cone spawns at mouse position
        _selected = true;
    }

    private void _removeCone(){
        if(_instance == null){
            return;
        }
        _instance.QueueFree();   // removes cone from scene
        _instance = null;
        _selected = false;
        // GD.Print("Cone scene is null");   // for debugging
    }

    private void _OnArea2DInputEvent(Node viewport, InputEvent @event, long shapeIdx)
    {
        if (@event is InputEventMouseButton mouseEvent && mouseEvent.ButtonIndex == MouseButton.Left)
        {
            if (mouseEvent.Pressed && _instance == null && _cone != null)
            {
                _createCone();
                
            }
            else{
                _removeCone();
            }
        }
    }

    public override void _Ready()
    {
        _cone = GD.Load<PackedScene>("res://cones/draggableCone.tscn");  // loads cone scene
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
