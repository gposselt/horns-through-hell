using UnityEngine;

public class FluteProjectile : Projectile
{
    public override void LaunchProjectile(Vector2 velocity, float lifetime)
    {
        base.LaunchProjectile((velocity / 1.5f) + new Vector2(0, Random.Range(-0.5f, 0.5f)), lifetime - 0.4f);
    }
}
