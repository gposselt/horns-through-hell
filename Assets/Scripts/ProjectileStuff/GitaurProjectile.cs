using UnityEngine;

public class GitaurProjectile : Projectile
{
    public override void LaunchProjectile(Vector2 velocity, float lifetime)
    {
        base.LaunchProjectile(velocity * 2, lifetime);
    }
}
