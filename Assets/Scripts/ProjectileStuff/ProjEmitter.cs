using UnityEngine;

public class ProjEmitter : MonoBehaviour
{
    public float SHOOT_COOLDOWN = 0.5f;
    
    public Projectile[] projPrefab;
    public float shootTimer = 0.0f;

    public bool lastDirection = true;

    public float projectileLifetime = 1.0f;

    void TryShootProjectile()
    {
        if (shootTimer <= Constants.TimeEpsilon)
        {
            // Player.Masks currentWeapon = activeMask;
            
                
            // Shoot has been charged up.
            //shoot hahs been charged op.
            Vector3 direction;

            if (!lastDirection)
            {
                direction = Vector3.left;
            }
            else
            {
                direction = Vector3.right;
            }


            // if (currentWeapon == Player.Masks.None)
            // {
                ShootLyreShot(direction);
            // }
            // else
            // {
            //     //shoot the right shot here
            //     
            //         
            //
            //     if (ammo.remainingAmmo[(int)currentWeapon] == 0)
            //     {
            //         //change current weapon type to mask
            //
            //         activeMask = Player.Masks.None;
            //     }
            // }
            
            
        }
        
    }

    void ShootLyreShot(Vector3 direction)
    {
        Projectile proj;
        if (Random.Range(0.0f, 1.0f) < 0.5f)
        {
            proj = Instantiate(projPrefab[0], transform.position + direction, Quaternion.identity);
        }
        else
        {
            proj = Instantiate(projPrefab[1], transform.position + direction, Quaternion.identity);

        }
        proj.LaunchProjectile(direction * 10.0f, projectileLifetime);
        shootTimer = SHOOT_COOLDOWN;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (shootTimer > Constants.TimeEpsilon)
            shootTimer -= Time.deltaTime;
        else
        {
            TryShootProjectile();
        }
    }
}
