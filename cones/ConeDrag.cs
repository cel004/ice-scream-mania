using Godot;

public partial class ConeDrag : Node2D
{
    private bool _selected = false;
    private bool _isConeSpawned = false;
    private Vector2 mouseOffset = Vector2.Zero;

    private Node2D _instance;    // creates new instance of cone when clicked
    private Area2D _pileOfCones;  // store reference to pileOfCones
    private PackedScene _cone;   // packed scene for cone (draggableCone.tscn)S

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
        if(!_isConeSpawned && _cone != null){
            _instance = (Node2D)_cone.Instantiate();  // creates new instance of cone when clicked
            AddChild(_instance);

            _isConeSpawned = true;
            _selected = true;
        }
        if(_isConeSpawned == true){
            return;
        }
    }

    // private void _removeCone(){
    //     if(_instance == null){
    //         return;
    //     }
    //     _instance.QueueFree();   // removes cone from scene
    //     _instance = null;
    //     _selected = false;
    //     // GD.Print("Cone scene is null");   // for debugging
    // }

    private void _OnArea2DInputEvent(Node viewport, InputEvent @event, long shapeIdx)
    {
        if (@event is InputEventMouseButton mouseEvent && mouseEvent.ButtonIndex == MouseButton.Left)
        {
            if (mouseEvent.Pressed && !_isConeSpawned)
            {
                _createCone();
                
            }
            if (mouseEvent.Pressed && _isConeSpawned){
                return;
            }
            // else{
            //     _removeCone();
            // }
        }
    }

    public override void _Ready()
    {
        _cone = GD.Load<PackedScene>("res://cones/draggableCone.tscn");  // loads cone scene
        _pileOfCones = GetNodeOrNull<Area2D>("/root/main/spawnCone/pileOfCones");    // reference to pileOfCones in the scene
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