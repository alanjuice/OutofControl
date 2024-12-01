using Godot;
using System;

public class Player : RigidBody2D
{
    private Vector2 velocity = Vector2.Zero;
    [Export] public PackedScene bullet;
    [Export] public float recoilVelocity = 50;
    private Timer _timer;

    public override void _Ready()
    {
        _timer = GetNode<Timer>("Timer");
    }

    public override void _PhysicsProcess(float delta)
    {
        // Make the gun look at the mouse position
        var gun = GetNode<Node2D>("Gun");
        gun.LookAt(GetGlobalMousePosition());

        // Update the reload value based on the timer's time left
        var reload = GetNode<TextureProgress>("Gun/reload");
        reload.Value = _timer.TimeLeft * 100;

        // Fire if the fire button is pressed and the RayCast2D is colliding
        var rayCast = GetNode<RayCast2D>("Gun/RayCast2D");
        if (Input.IsActionJustPressed("fire") && rayCast.IsColliding())
        {
            var collider = rayCast.GetCollider();
            if (!(collider is Area2D)) // Prevent shooting if collider is an Area2D
            {
                Shoot();
            }
        }
    }

    private void Shoot()
    {
        // Fire the gun
        if (_timer.TimeLeft == 0) // Prevent rapid firing
        {
            GD.Print("Gh");
            var bulletInstance = (Area2D)bullet.Instance();
            GetParent().AddChild(bulletInstance);  // Add bullet to the scene
            bulletInstance.GlobalTransform = GetNode<Node2D>("Gun").GlobalTransform;

            // Apply recoil
            velocity = (Position - GetGlobalMousePosition()).Normalized() * -5;
            LinearVelocity = -velocity * recoilVelocity;

            // Set bullet velocity
            bulletInstance.Set("velocity", velocity);

            // Play firing sound and gun explosion animation
            var fireSound = GetNode<AudioStreamPlayer2D>("fire");
            fireSound.Play();
            var gunExplosion = GetNode<CPUParticles2D>("Gun/GunExplosion");
            gunExplosion.Emitting = true;

            // Start the timer to prevent rapid shooting
            _timer.Start();
        }
    }

    private void _on_outofscreen_screen_exited()
    {
        // Reload the scene after 1 second when off-screen
        var timer = GetTree().CreateTimer(1f);
        timer.Connect("timeout", this, nameof(OnReloadSceneTimeout));
    }

    private void OnReloadSceneTimeout()
    {
        GetTree().ReloadCurrentScene();
    }
}
