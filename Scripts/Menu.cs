using Godot;

public class Menu : Node2D
{
    private Area2D _aboutNode;

    public override void _Ready()
    {
        _aboutNode = GetNode<Area2D>("About");
        _aboutNode.Connect("body_entered", this, nameof(_On_AboutBodyEntered));
    }

    private void _On_AboutBodyEntered(Node body)
    {
        if (body.IsInGroup("Player"))
        {
            //If body enters the about area, display the message for 3 seconds
            _aboutNode.GetNode<Label>("about").Visible = true;
            var timer = GetTree().CreateTimer(3f);
            timer.Connect("timeout", this, nameof(OnAboutTimeout));
        }
    }

    private void OnAboutTimeout()
    {
        if (_aboutNode != null)
        {
            _aboutNode.GetNode<Label>("about").Visible = false;
        }
    }
}
