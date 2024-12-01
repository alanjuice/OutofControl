using Godot;

public class LevelChanger : Area2D
{
    private int _maxLevels = 6;

    public override void _Ready()
    {
        Connect("body_entered", this, nameof(_On_LevelChangerBodyEntered));
        
        var lava = GetParent().GetNode<Node2D>("Lava");

        //If there is lava in the level, else dont bother connecting signals
        if (lava != null)
        {
            foreach (Node child in lava.GetChildren())
            {
                child.Connect("Fried", this, nameof(_On_LavaFried));
            }
        }

    }
    private void _On_LevelChangerBodyEntered(Node body)
    {
        if (body.IsInGroup("Player"))
        {
            //Getting current level no from scene name
            var currentScene = GetTree().CurrentScene.Filename;
            var parts = currentScene.Split("/");
            var file_name = parts[parts.Length - 1];
            var currentLevel = file_name.Split(".")[0].ToInt();

            //Check if final level
            if (currentLevel>=_maxLevels)
            {
                BackToMenu();
            }

            var nextLevel = currentLevel+1;
            var nextLevelScene = "res://Scenes/Levels/" + nextLevel.ToString() + ".tscn";
            GetTree().ChangeScene(nextLevelScene);

        }
    }

    private void BackToMenu()
    {
        var nextLevelScene = "res://Scenes/Levels/1.tscn";
        GetTree().ChangeScene(nextLevelScene);
    }

    // Handles the "retry" action input
    public override void _Input(InputEvent @event)
    {
        if (@event.IsActionPressed("retry"))
        {
            Retry();
        }
        
    }

    private void _On_LavaFried()
    {
        Retry();
    }

    private void Retry()
    {
        var currentScene = GetTree().CurrentScene.Filename;
        GetTree().ChangeScene(currentScene);
    }
}
