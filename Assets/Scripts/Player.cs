using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    const float speedConst = 7.070001f;
    CharacterController cc;
    public float speed, jump, gravity;
    Vector3 moveDirection = Vector3.zero;
    public GameObject bulletPrefab, bulletSpawnPoint;
    public Gun gun;
    public float maxhealth = 100;
    public float hp;
    public float money;
    [HideInInspector]
    public int nbCola = 0;
    [HideInInspector]
    private float nextShootingTime = 0f;
    [HideInInspector]
    public float regenTime;
    [HideInInspector]
    public float regenCoolDown;
    public TextMeshProUGUI ammoText;
    public Slider healthBar;
    public Color32 color;
    public Material material;
    private float nextDashTime;
    [HideInInspector]
    public float dashCoolDown;
    public List<TypeCola> colasOwned;
    public bool isDown;
    [HideInInspector]
    public bool beingRevived;

    // Start is called before the first frame update
    void Start()
    {
        cc = GetComponent<CharacterController>();
        healthBar.GetComponent<Slider>().value = 1f;
        hp = maxhealth;
        dashCoolDown = 5f;
        UpdateBulletSP();
        bulletPrefab = Instantiate(bulletPrefab, transform);
        bulletPrefab.GetComponent<Bullet>().explosionPrefab = Instantiate(bulletPrefab.GetComponent<Bullet>().explosionPrefab, transform);
        material = new Material(material);
        UpdateColors();
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
        if (!isDown)
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

            Shoot();

            if (Input.GetButtonDown("Jump") && cc.isGrounded)
            {
                //moveDirection.y = jump;
                if (Time.time >= nextDashTime)
                    StartCoroutine(Dash());
            }

            if (Input.GetMouseButtonDown(0))
                ChangeSpeed(0.3f);
            else if (Input.GetMouseButtonUp(0))
                ChangeSpeed(1);
        }

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Plane plane = new Plane(Vector3.up, new Vector3(0, transform.position.y, 0));
        if (plane.Raycast(ray, out float length))
        {
            transform.LookAt(new Vector3(ray.GetPoint(length).x, transform.position.y, ray.GetPoint(length).z));
        }

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

        cc.Move(moveDirection * Time.deltaTime);
        if (!cc.isGrounded)
            moveDirection.y -= gravity * Time.deltaTime;

        Camera.main.transform.position = new Vector3(7.5f, 12, -7.5f) + transform.position;

        if (isDown && !beingRevived)
        {
            hp -= (2 * Time.deltaTime);
            healthBar.GetComponent<Slider>().value = hp / maxhealth;
            if (hp <= 0)
                Destroy(this);
        }
    }

    IEnumerator Dash()
    {
        ChangeSpeed(3);
        yield return new WaitForSeconds(0.1f);
        ChangeSpeed(1);
        nextDashTime = dashCoolDown + Time.time;
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
                    yield return new WaitForSeconds(0.1f);
                }
            }
            StartCoroutine(TirerRafale());
        }
    }

    void ShootBullet()
    {
        nextShootingTime = Time.time + 1f / (gun.GetComponent<Gun>().fireRate * gun.GetComponent<Gun>().fireRateMultiplier);
        if (!gun.spray.enabled)
        {
            GameObject go = Instantiate(bulletPrefab, bulletSpawnPoint.transform.position, transform.rotation);
            BulletConstructor(go);
            Destroy(go, gun.bulletSelfDestruct);
        }
        else
        {
            for (int i = 0; i < gun.spray.bulletAmount; i++)
            {
                GameObject go = Instantiate(bulletPrefab, bulletSpawnPoint.transform.position, transform.rotation);
                BulletConstructor(go);
                go.GetComponent<Bullet>().direction = Quaternion.Euler(0, gun.spray.angles[i], 0) * go.GetComponent<Bullet>().direction;
                Destroy(go, gun.bulletSelfDestruct);
            }
        }
        gun.ammo--;
        ammoText.text = gun.ammo.ToString();
    }

    public void AddMoney(int money)
    {
        this.money += (int)(money * GameManager.Instance.pointMultiplier);
    }

    void BulletConstructor(GameObject go)
    {
        Bullet bullet = go.GetComponent<Bullet>();
        bullet.player = this;
        bullet.direction = go.transform.position - transform.position;
        bullet.direction.y = 0;
        bullet.isPiercing = gun.isPiercing;
        bullet.isExplosive = gun.isExplosive;
        bullet.electric = gun.electric;
        bullet.fire = gun.fire;
        bullet.ice = gun.ice;
        bullet.poison = gun.poison;
        go.SetActive(true);
        IEnumerator Dissolve()
        {
            yield return new WaitForSeconds(gun.bulletSelfDestruct * 0.8f);
            bullet.dissolveTime = 0.2f / (gun.bulletSelfDestruct * 0.2f);
            bullet.dissolve = true;
        }
        StartCoroutine(Dissolve());
    }

    public void TakeDamage(float damage)
    {
        if (!isDown)
        {
            hp -= damage;
            regenTime = regenCoolDown + Time.time;
            healthBar.GetComponent<Slider>().value = hp / maxhealth;
            if (hp <= 0)
                PlayerDown();
        }
    }

    public void PlayerDown()
    {
        //this.transform.rotation = Quaternion.Euler(90, 0, 0);
        isDown = true;
        foreach (TypeCola cola in colasOwned)
        {
            switch (cola)
            {
                case TypeCola.DeadshotDai:
                    foreach (Transform enfant in this.transform)
                        if (enfant.gameObject.GetComponent<GunManager>() != null)
                            foreach (Transform gun2 in enfant)
                                gun2.GetComponent<Gun>().damageMultiplier = 1f;
                    break;
                case TypeCola.DoubleTap:
                    foreach (Transform enfant in this.transform)
                        if (enfant.gameObject.GetComponent<GunManager>() != null)
                            foreach (Transform gun2 in enfant)
                                gun2.GetComponent<Gun>().fireRateMultiplier = 1f;
                    break;
                case TypeCola.ElectricCherry:
                    break;
                case TypeCola.JuggerNog:
                    maxhealth = 100;
                    break;
                case TypeCola.MuteKick:
                    this.GetComponentInChildren<GunManager>().muleKick = false;
                    if (this.GetComponentInChildren<GunManager>().nbGunsOwned == 3)
                        foreach (Transform enfant in this.transform)
                            if (enfant.gameObject.GetComponent<GunManager>() != null)
                            {
                                bool enlever = false;
                                foreach (Transform gun2 in enfant)
                                {
                                    if (gun2.GetComponent<Gun>().isOwned && !gun2.GetComponent<Gun>().inUse && !enlever)
                                    {
                                        gun2.GetComponent<Gun>().isOwned = false;
                                        enlever = true;
                                        enfant.gameObject.GetComponent<GunManager>().gunsOwned.Remove(gun2.GetComponent<Gun>().modele);
                                    }
                                }
                            }
                    this.GetComponentInChildren<GunManager>().nbGunsOwned = 2;
                    break;
                case TypeCola.Quick:
                    break;
                case TypeCola.StaminUp:
                    dashCoolDown = 5f;
                    break;
                default:
                    break;
            }
        }
        hp = 100;
        colasOwned.Clear();
        nbCola = 0;
        ChangeSpeed(0.2f);
    }

    public void Revive()
    {
        isDown = false;
        beingRevived = false;
        hp = 100;
        ChangeSpeed(1f);
    }

    public void UpdateColors()
    {
        material.SetColor("_BaseColor", color);
        material.SetColor("_EmissionColor", color);
        bulletPrefab.GetComponent<Light>().color = color;
        bulletPrefab.GetComponent<MeshRenderer>().sharedMaterial = material;
        bulletPrefab.GetComponent<TrailRenderer>().material = material;
        bulletPrefab.GetComponent<Bullet>().explosionPrefab.GetComponent<ParticleSystemRenderer>().sharedMaterial = material;
        bulletPrefab.GetComponent<Bullet>().explosionPrefab.GetComponent<ParticleSystemRenderer>().trailMaterial = material;
        ParticleSystem.MainModule main = bulletPrefab.GetComponent<Bullet>().explosionPrefab.GetComponent<ParticleSystem>().main;
        main.startColor = material.color;
        bulletPrefab.GetComponent<Bullet>().explosionPrefab.GetComponent<Light>().color = color;
    }
    public void ChangeSpeed(float percentage)
    {
        speed = speedConst * percentage;
    }
}

