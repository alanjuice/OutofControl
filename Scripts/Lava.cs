using Godot;

public class Lava : Area2D
{
    [Signal]
    public delegate void Fried();

    public override void _Ready()
    {
        Connect("body_entered", this, nameof(_On_Lava_Player_Entered));
    }

    private void _On_Lava_Player_Entered(Node body)
    {
        if (body.IsInGroup("Player"))
        {
            EmitSignal(nameof(Fried)); 
            //body.QueueFree(); 
        }
    }
}
