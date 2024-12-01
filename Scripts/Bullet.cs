using Godot;

public class Bullet : Area2D
{
    [Export]
    private Vector2 velocity = Vector2.Zero;

    public override void _PhysicsProcess(float delta)
    {
        Translate(velocity);
    }

    public override void _Ready()
    {
        this.Connect("body_entered", this, nameof(_Bullet_Hit_Object));
    }

    private async void _Bullet_Hit_Object(Node body)
    {
        if (!body.IsInGroup("Player"))
        {
            GetNode<Sprite>("bullet").Visible = false;
            GetNode<CollisionShape2D>("collision").SetDeferred("disabled", true);
            GetNode<CPUParticles2D>("explode").Emitting = true;
            SetPhysicsProcess(false);

            await ToSignal(GetTree().CreateTimer(1), "timeout");
            QueueFree();
        }
    }
}
