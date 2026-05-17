using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Weapon : MonoBehaviour
{
    [Header("WeaponStats")]
    [SerializeField] private int bulletsMax;
    [SerializeField] private int rechargeTime;
    [SerializeField] private float timeBtwBullet;
    [SerializeField] private float weaponDamage;
    public float bulletDamage;
    public Vector3 bulletSpread;
    [SerializeField] private float bulletForce;
    private int bulletsRemaining;
    private bool canShoot;
    private bool isRecharging;
    public bool enemyHit;
    public bool isAbleToShoot = true; //camera control


    [Header("Recoil")]
    [SerializeField] private Transform recoilTarget;
    [SerializeField] private float recoilBackSpeed = 15f;
    [SerializeField] private float recoilReturnSpeed = 20f;
    [SerializeField] private float aimingSpeed = 20f;
    private bool isRecoiling;

    [Header("HitMarker")]
    public RectTransform top;
    public RectTransform bottom;
    public RectTransform left;
    public RectTransform right;
    public float fill = 0f;
    [SerializeField] private float hitMarkerSpeed;

    [Header("Objects")]
    public GameObject muzzleFlash;
    private Vector3 startLocalPos;
    public Animator anim;
    [SerializeField] private Transform bulletExit;
    [SerializeField] private GameObject bulletPrefab;
    public AudioSource bulletSound;
    private void Start()
    {
        bulletsRemaining = bulletsMax;
        canShoot = true;
        startLocalPos = transform.localPosition;
        muzzleFlash.SetActive(false);
    }
    private void Update()
    {
        if (Input.GetMouseButton(0) && bulletsRemaining > 0 && canShoot && isRecharging == false && isAbleToShoot)
        {
            StartCoroutine(ShootColdown(timeBtwBullet));
        }   
        if (bulletsRemaining < bulletsMax)
        {
            if (Input.GetKeyDown(KeyCode.R) && isRecharging == false)
            {
                StartCoroutine(Recharge(rechargeTime));
            }
        }

        if (Input.GetMouseButton(0) && bulletsRemaining > 0 && isRecharging == false && isAbleToShoot)
        {
            bulletSound.UnPause();
            anim.SetBool("Shooting", true);
        }
        else
        {
            bulletSound.Pause();
            anim.SetBool("Shooting", false);
        }
        HandleRecoil();

        //hitMarker();
        if (enemyHit)
        {
            fill = 1;
            enemyHit = false;
        }
    }
    private void Shoot()
    {
        Vector3 directionSpread = GetShootingDirectionSpread();
        GameObject bullet = Instantiate(bulletPrefab, bulletExit.position, bulletExit.transform.rotation);
        bullet.GetComponent<Bullet>().bulletDamege(weaponDamage);
        bullet.GetComponent<Rigidbody>().AddForce((bullet.transform.forward += directionSpread) * bulletForce, ForceMode.Impulse);

        bulletsRemaining -= 1;
    }
    IEnumerator ShootColdown(float time)
    {
        Shoot();
        canShoot = false;
        isRecoiling = true;

        yield return new WaitForSeconds(time);
        isRecoiling = false;
        canShoot = true;
    }
    IEnumerator Recharge(float time)
    {
        isRecharging = true;
        //anim.SetBool("Recharge", true);
        yield return new WaitForSeconds(time);
        //anim.SetBool("Recharge", false);
        bulletsRemaining = bulletsMax;
        isRecharging = false;
    }
    private void HandleRecoil()
    {
        if (isRecoiling)
        {
            muzzleFlash.SetActive(true);
            transform.localPosition = Vector3.Lerp(
                transform.localPosition,
                recoilTarget.localPosition,
                recoilBackSpeed * Time.deltaTime
            );

            if (Vector3.Distance(transform.localPosition, recoilTarget.localPosition) < 0.01f)
                isRecoiling = false;
            if (Vector3.Distance(transform.localPosition, recoilTarget.localPosition) < 0.09f)
            {
                muzzleFlash.SetActive(false);
            }
        }
        else
        {
            muzzleFlash.SetActive(false);
            transform.localPosition = Vector3.Lerp(
                transform.localPosition,
                startLocalPos,
                recoilReturnSpeed * Time.deltaTime
            );
        }
    }
    private Vector3 GetShootingDirectionSpread()
    {
        Vector3 direction = new Vector3(
            Random.Range(-bulletSpread.x, bulletSpread.x),
            Random.Range(-bulletSpread.y, bulletSpread.y),
            Random.Range(-bulletSpread.z, bulletSpread.z));
        return direction;
    }
    private void hitMarker()
    {
        top.GetComponent<Image>().fillAmount = fill;
        left.GetComponent<Image>().fillAmount = fill;
        bottom.GetComponent<Image>().fillAmount = fill;
        right.GetComponent<Image>().fillAmount = fill;
        if (fill > 0)
        {
            fill = Mathf.MoveTowards(fill, 0f, hitMarkerSpeed * Time.deltaTime);
        }

    }
}
