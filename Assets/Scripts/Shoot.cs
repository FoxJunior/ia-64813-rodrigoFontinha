using UnityEngine;
using TMPro;

public class Shoot : MonoBehaviour
{
    [Header("References")]
    public Transform cam, attackPoint, gun, gunAimSpot, gunOriginalSpot;
    public GameObject objectToShoot, weapon, spotForReload;
    public TMP_Text shootText;
    public Animator gunAnim;
    public AudioSource shootAudio;
    public AudioSource reloadAudio;
    public ParticleSystem shootParticle;

    [Header("Settings")]
    public int totalBulletsInMag, bulletsToReload;
    private int bulletsInMag;
    public float shootCooldown;
    private int upgradeLevel = 1;

    [Header("Shooting")]
    public KeyCode shootKey = KeyCode.Mouse0;
    public KeyCode reloadKey = KeyCode.R;
    public float shootForce;

    bool readyToShoot;
    public static bool gunThrowed;
    private bool gunReloading;
    private bool gunAimed;

    void Start()
    {
        readyToShoot = true;
        gunThrowed = gunReloading = gunAimed = false;
        bulletsInMag = totalBulletsInMag;
        UpdateAmmoText();
    }

    void Update()
    {
        if (!PauseMenu.isPaused)
        {
            if (Input.GetKeyDown(shootKey))
            {
                if (readyToShoot && !gunReloading && bulletsInMag > 0)
                {
                    GunShoot();
                }
                else if (readyToShoot && bulletsInMag <= 0 && bulletsToReload > 0 && !gunReloading && !gunAimed)
                {
                    GunReload();
                }

            }
            else if (Input.GetKeyDown(reloadKey) && bulletsInMag < totalBulletsInMag && bulletsToReload > 0 && !gunReloading && !gunAimed)
            {
                GunReload();
            }
            else if (Input.GetMouseButtonDown(1) && !gunReloading)
            {
                GunAim(true);
            }
            else if (Input.GetMouseButtonUp(1))
            {
                GunAim(false);
            }else if (Input.GetKey(KeyCode.F) && !gunReloading)
            {
                ThrowGun();
            }
        }
    }


    private void GunReload()
    {
        readyToShoot = false;
        gunReloading = true;

        spotForReload.SetActive(true);

        gunAnim.SetTrigger("Reload");

        Invoke(nameof(ResetReload), 2.5f);
    }

    private void ResetReload()
    {
        reloadAudio.Play();
        int numOfBulletsToReload = totalBulletsInMag - bulletsInMag;
        if (numOfBulletsToReload > bulletsToReload)
        {
            numOfBulletsToReload = bulletsToReload;
        }
        bulletsInMag += numOfBulletsToReload;
        bulletsToReload -= numOfBulletsToReload;
        UpdateAmmoText();
        spotForReload.SetActive(false);
        gunReloading = false;
        readyToShoot = true;
    }

    private void ThrowGun()
    {
        if (!gunThrowed)
        {
            readyToShoot = false;
            gunThrowed = true;
            gunAnim.SetTrigger("Throw");
            Invoke(nameof(ResetThrow), 1f);
        }
    }

    private void ResetThrow()
    {
        gunThrowed = false;
        readyToShoot = true;
    }

    private void UpdateAmmoText()
    {
        shootText.text = bulletsInMag + " / " + bulletsToReload;
    }

    private void GunShoot()
    {
        readyToShoot = false;

        shootParticle.Play();

        GameObject bullet = Instantiate(objectToShoot, attackPoint.position, cam.rotation);

        Rigidbody bulletRb = bullet.GetComponent<Rigidbody>();

        Vector3 forceToAdd = cam.transform.forward * shootForce;

        bulletRb.AddForce(forceToAdd, ForceMode.Impulse);

        Destroy(bulletRb, 3);

        shootAudio.Play();

        bulletsInMag--;

        UpdateAmmoText();

        Invoke(nameof(ResetShoot), shootCooldown);
    }

    private void ResetShoot()
    {
        readyToShoot = true;
    }

    private void GunAim(bool flag)
    {
        if (flag)
        {
            gunAimed = true;
            gun.SetPositionAndRotation(Vector3.Slerp(gunOriginalSpot.position, gunAimSpot.position, 1f), Quaternion.Slerp(gunOriginalSpot.rotation, gunAimSpot.rotation, 1f));
        }
        else
        {
            gunAimed = false;
            gun.SetPositionAndRotation(Vector3.Slerp(gunAimSpot.position, gunOriginalSpot.position, 1f), Quaternion.Slerp(gunAimSpot.rotation, gunOriginalSpot.rotation, 1f));
        }
    }

    public void UpgradeLevel()
    {
        upgradeLevel += 1;
    }
    public int GetUpgradeLevel()
    {
        return upgradeLevel;
    }

    public void BuyMagazine()
    {
        bulletsToReload += totalBulletsInMag;
        UpdateAmmoText();
    }

}
