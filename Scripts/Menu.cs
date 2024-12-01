using Godot;

public class AreaHandler : Node2D
{
    // Called when a body enters the "Play" area
    private void _On_PlayBodyEntered(Node body)
    {
        if (body.IsInGroup("Player"))
        {
        }
    }

    // Called when a body enters the "About" area
    private void _On_AboutBodyEntered(Node body)
    {
        if (body.IsInGroup("Player"))
        {
            var about = GetNode<CanvasLayer>("about");
            about.Visible = true;
            // Wait for 3 seconds and then hide the "about" element
            var timer = GetTree().CreateTimer(3f);
            timer.Connect("timeout", this, nameof(OnAboutTimeout), new Godot.Collections.Array { about });
        }
    }

    // Callback when the "about" timeout is finished
    private void OnAboutTimeout(CanvasLayer about)
    {
        about.Visible = false;
    }
}
