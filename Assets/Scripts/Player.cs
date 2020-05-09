using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    CharacterController cc;
    public float speed, jump, gravity;
    Vector3 moveDirection = Vector3.zero;
    public GameObject bulletPrefab, bulletSpawnPoint;
    public Gun gun;
    public float maxhealth = 100;
    public float hp;
    public int money;
    [HideInInspector]
    public int nbCola = 0;
    [HideInInspector]
    public bool oneShot;
    [HideInInspector]
    public float pointMultiplier;
    private float nextShootingTime = 0f;
    [HideInInspector]
    public float regenTime;
    [HideInInspector]
    public float regenCoolDown;
    public TextMeshProUGUI ammoText;
    public Slider healthBar;

    // Start is called before the first frame update
    void Start()
    {
        cc = GetComponent<CharacterController>();
        healthBar.GetComponent<Slider>().value = 1f;
        hp = maxhealth;
        UpdateBulletSP();
    }

    public void UpdateBulletSP()
    {
        foreach (Transform transforms in gun.transform)
            if (transforms.gameObject.CompareTag("BulletSP"))
                bulletSpawnPoint = transforms.gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time >= regenTime)
        {
            if (hp != maxhealth)
            {
                if (hp + (5 * Time.deltaTime) <= maxhealth)
                    hp += (5 * Time.deltaTime);
                else
                    hp = maxhealth;
            }
            healthBar.GetComponent<Slider>().value = ((float)hp) / ((float)maxhealth);
        }
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Plane plane = new Plane(Vector3.up, new Vector3(0, transform.position.y, 0));
        if (plane.Raycast(ray, out float length))
        {
            transform.LookAt(new Vector3(ray.GetPoint(length).x, transform.position.y, ray.GetPoint(length).z));
        }

        Shoot();

        Vector3 vector = new Vector3(0, 0, 0);
        if (Input.GetKey(KeyCode.W))
            vector += new Vector3(Camera.main.transform.forward.normalized.x, 0, Camera.main.transform.forward.normalized.z);
        if (Input.GetKey(KeyCode.S))
            vector -= new Vector3(Camera.main.transform.forward.normalized.x, 0, Camera.main.transform.forward.normalized.z);
        if (Input.GetKey(KeyCode.A))
            vector -= new Vector3(Camera.main.transform.right.normalized.x, 0, Camera.main.transform.right.normalized.z);
        if (Input.GetKey(KeyCode.D))
            vector += new Vector3(Camera.main.transform.right.normalized.x, 0, Camera.main.transform.right.normalized.z);
        vector = vector.normalized * speed;
        moveDirection = new Vector3(vector.x, moveDirection.y, vector.z);
        if (Input.GetButton("Jump") && cc.isGrounded)
        {
            moveDirection.y = jump;
        }
        if (!cc.isGrounded)
            moveDirection.y -= gravity * Time.deltaTime;
        cc.Move(moveDirection * Time.deltaTime);

        Camera.main.transform.position = new Vector3(7.5f, 12, -7.5f) + transform.position;
    }

    private void Shoot()
    {
        if (Input.GetMouseButtonDown(0) && Time.time >= nextShootingTime && gun.type == TypeGun.Semi)
        {
            ShootBullet();
        }
        if (Input.GetMouseButton(0) && Time.time >= nextShootingTime && gun.type == TypeGun.Auto)
        {
            ShootBullet();
        }
        if (Input.GetMouseButtonDown(0) && Time.time >= nextShootingTime && gun.type == TypeGun.Rafale)
        {
            IEnumerator TirerRafale()
            {
                for (int i = 0; i < 3; i++)
                {
                    ShootBullet();
                    yield return new WaitForSeconds(0.15f);
                }
            }
            StartCoroutine(TirerRafale());
        }
    }

    void ShootBullet()
    {
        nextShootingTime = Time.time + 1f / (gun.GetComponent<Gun>().fireRate * gun.GetComponent<Gun>().fireRateMultiplier);
        if (!gun.spray)
        {
            GameObject go = Instantiate(bulletPrefab, bulletSpawnPoint.transform.position, transform.rotation);
            BulletConstructor(go);
            Destroy(go, 1);
        }
        else
        {
            for (int i = 0; i < gun.bulletAmount; i++)
            {
                GameObject go = Instantiate(bulletPrefab, bulletSpawnPoint.transform.position, transform.rotation);
                BulletConstructor(go);
                go.GetComponent<Bullet>().direction = Quaternion.Euler(0, gun.angles[i], 0) * go.GetComponent<Bullet>().direction;
                Destroy(go, 1);
            }
        }
        gun.ammo--;
        ammoText.text = gun.ammo.ToString();
    }

    public void AddMoney(int money)
    {
        this.money += (int)(money * pointMultiplier);
    }

    void BulletConstructor(GameObject go)
    {
        Bullet bullet = go.GetComponent<Bullet>();
        bullet.player = this;
        bullet.direction = go.transform.position - transform.position;
        bullet.direction.y = 0;
        bullet.oneShot = oneShot;
        bullet.pointMultiplier = pointMultiplier;
        bullet.piercing = gun.piercing;
    }
}

