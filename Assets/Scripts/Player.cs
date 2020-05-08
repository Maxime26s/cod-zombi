using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    CharacterController cc;
    public float speed, jump, gravity;
    bool onGround;
    Vector3 moveDirection = Vector3.zero;
    public GameObject bulletPrefab, bulletSpawnPoint;
    public Gun gun;
    public float hp = 100;
    public int money;
    public int nbCola = 0;
    public bool oneShot;
    public float pointMultiplier;
    public TextMeshProUGUI ammoText;
    private float nextShootingTime = 0f;
    public float regenTime;
    public float regenCoolDown;
    public float maxhealth = 100;
    public Slider healthBar;

    // Start is called before the first frame update
    void Start()
    {
        cc = GetComponent<CharacterController>();
        healthBar.GetComponent<Slider>().value = 1f;
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
        RaycastHit hit;
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

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Ground")
            onGround = true;
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
        nextShootingTime = Time.time + 1f / (gun.GetComponent<Gun>().fireRate * gun.GetComponent<Gun>().frMultiplier);
        GameObject go = Instantiate(bulletPrefab, bulletSpawnPoint.transform.position, transform.rotation);
        Bullet bullet = go.GetComponent<Bullet>();
        bullet.player = this;
        bullet.direction = bulletSpawnPoint.transform.position - transform.position;
        bullet.direction.y = 0;
        bullet.oneShot = oneShot;
        bullet.pointMultiplier = pointMultiplier;
        bullet.piercing = gun.piercing;
        Destroy(go, 1);
        gun.ammo--;
        ammoText.text = gun.ammo.ToString();
    }

    public void AddMoney(int money)
    {
        this.money += (int)(money * pointMultiplier);
    }

    bool GroundCheck()
    {
        RaycastHit hit;
        float distance = 1f;
        Vector3 dir = new Vector3(0, -1);

        return Physics.Raycast(transform.position, dir, out hit, distance);
    }
    /*
    void ShootLaserFromTargetPosition(Vector3 targetPosition, Vector3 direction, float length)
    {
        Ray ray = new Ray(targetPosition, direction);
        RaycastHit raycastHit;
        Vector3 endPosition = targetPosition + (length * direction);

        if (Physics.Raycast(ray, out raycastHit, length))
        {
            endPosition = raycastHit.point;
        }

        laserLineRenderer.SetPosition(0, targetPosition);
        laserLineRenderer.SetPosition(1, endPosition);
    }
    */
}

