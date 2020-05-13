using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    [HideInInspector]
    public Dictionary<string, float> distances = new Dictionary<string, float>() { };
    const float speedConst = 7.070001f;
    CharacterController cc;
    public float speed, gravity;
    Vector3 moveDirection = Vector3.zero;
    public GameObject bulletPrefab, bulletSpawnPoint, electricCherryPrefab;
    public Gun gun;
    public float maxHealth = 100;
    public float hp;
    public float money;
    [HideInInspector]
    public int nbCola = 0;
    [HideInInspector]
    private float nextShootingTime = 0f;
    public float regenAmount, regenCooldown;
    public TextMeshProUGUI ammoText;
    public Slider healthBar;
    public Color32 color;
    public Material material;
    private bool dashOnCooldown, isDashing, regenOnCooldown, ammoIsFlashing;
    [HideInInspector]
    public float dashCooldown;
    public List<TypeCola> colasOwned;
    public bool isDown;
    [HideInInspector]
    public bool beingRevived;
    private Coroutine regenCoroutine;

    // Start is called before the first frame update
    void Start()
    {
        cc = GetComponent<CharacterController>();
        healthBar.GetComponent<Slider>().value = 1f;
        hp = maxHealth;
        dashCooldown = 5f;
        UpdateBulletSP();
        bulletPrefab = Instantiate(bulletPrefab, transform);
        bulletPrefab.GetComponent<Bullet>().explosionPrefab = Instantiate(bulletPrefab.GetComponent<Bullet>().explosionPrefab, transform);
        bulletPrefab.GetComponent<Bullet>().deathParticles = Instantiate(bulletPrefab.GetComponent<Bullet>().deathParticles, transform);
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
            if (!regenOnCooldown && hp != maxHealth)
            {
                hp += (regenAmount * Time.deltaTime);
                if (hp >= maxHealth)
                    hp = maxHealth;
                healthBar.GetComponent<Slider>().value = hp / maxHealth;
            }

            if (Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1))
                ChangeSpeed(0.5f * gun.GetSpeedModifier());
            else if (Input.GetMouseButtonUp(0) && !Input.GetMouseButton(1) || Input.GetMouseButtonUp(1) && !Input.GetMouseButton(0))
            {
                ChangeSpeed(1f);
                ParticleSystem.MainModule main = electricCherryPrefab.GetComponent<ParticleSystem>().main;
                main.loop = false;
            }

            if (Input.GetButtonDown("Jump") && cc.isGrounded && !dashOnCooldown)
            {
                IEnumerator Dash()
                {
                    dashOnCooldown = true;
                    isDashing = true;
                    ChangeSpeed(3);
                    yield return new WaitForSeconds(0.1f);
                    if (Input.GetMouseButton(0) || Input.GetMouseButton(1))
                        ChangeSpeed(0.5f * gun.GetSpeedModifier());
                    else
                        ChangeSpeed(1f);
                    isDashing = false;
                    yield return new WaitForSeconds(dashCooldown);
                    dashOnCooldown = false;
                }
                StartCoroutine(Dash());
            }
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
            healthBar.GetComponent<Slider>().value = hp / maxHealth;
            if (hp <= 0)
                Destroy(this);
        }
        Shoot();
    }

    private void Shoot()
    {
        if (Input.GetMouseButtonDown(0) && Time.time >= nextShootingTime && gun.type == TypeGun.Semi)
        {
            ShootBullet();
        }
        else if (Input.GetMouseButton(0) && Time.time >= nextShootingTime && gun.type == TypeGun.Auto)
        {
            ShootBullet();
        }
        else if (Input.GetMouseButtonDown(0) && Time.time >= nextShootingTime && gun.type == TypeGun.Rafale)
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
        if (VerifyCola(TypeCola.ElectricCherry))
            ElectricCherry();
        if (!gun.spray.enabled)
        {
            if (gun.ammo > 0)
            {
                GameObject go = Instantiate(bulletPrefab, bulletSpawnPoint.transform.position, transform.rotation);
                BulletConstructor(go);
                Destroy(go, gun.bulletSelfDestruct);
            }
            else
                Instantiate(bulletPrefab.GetComponent<Bullet>().deathParticles, bulletSpawnPoint.transform.position, transform.rotation).SetActive(true);

        }
        else
        {
            if (gun.ammo > 0)
            {
                for (int i = 0; i < gun.spray.bulletAmount; i++)
                {

                    GameObject go = Instantiate(bulletPrefab, bulletSpawnPoint.transform.position, transform.rotation);
                    BulletConstructor(go);
                    go.GetComponent<Bullet>().direction = Quaternion.Euler(0, gun.spray.angles[i], 0) * go.GetComponent<Bullet>().direction;
                    Destroy(go, gun.bulletSelfDestruct);
                }
            }
            else
                Instantiate(bulletPrefab.GetComponent<Bullet>().deathParticles, bulletSpawnPoint.transform.position, transform.rotation).SetActive(true);
        }
        if (gun.ammo > 0)
        {
            gun.ammo--;
            ammoText.text = gun.ammo.ToString();
            if (gun.ammo == 0)
                ammoText.color = Color.red;
        }
        else if (!ammoIsFlashing)
        {
            IEnumerator NoAmmo()
            {
                ammoIsFlashing = true;
                for (int i = 0; i < 3; i++)
                {
                    ammoText.color = Color.white;
                    yield return new WaitForSeconds(0.1f);
                    ammoText.color = Color.red;
                    yield return new WaitForSeconds(0.1f);
                }
                ammoIsFlashing = false;
            }
            StartCoroutine(NoAmmo());
        }
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
            if (regenCoroutine != null)
                StopCoroutine(regenCoroutine);
            IEnumerator RegenCooldown()
            {
                regenOnCooldown = true;
                yield return new WaitForSeconds(regenCooldown);
                regenOnCooldown = false;
            }
            regenCoroutine = StartCoroutine(RegenCooldown());
            hp -= damage;
            healthBar.GetComponent<Slider>().value = hp / maxHealth;
            if (hp <= 0)
                PlayerDown();
        }
    }

    void ElectricCherry()
    {
        ParticleSystem.MainModule main = electricCherryPrefab.GetComponent<ParticleSystem>().main;
        main.loop = true;
        electricCherryPrefab.SetActive(true);
        Collider[] colliders = Physics.OverlapSphere(transform.position, 5, 1 << 9);
        foreach (Collider collider in colliders)
        {
            collider.GetComponent<Enemy>().TakeDamage(0.1f * (1 - (collider.transform.position - transform.position).magnitude / 5), this, DamageType.DOT, NumberType.Percent);
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
                    maxHealth = 100;
                    break;
                case TypeCola.MuteKick:
                    GetComponentInChildren<GunManager>().muleKick = false;
                    if (GetComponentInChildren<GunManager>().nbGunsOwned == 3)
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
                    GetComponentInChildren<GunManager>().nbGunsOwned = 2;
                    break;
                case TypeCola.Quick:
                    break;
                case TypeCola.StaminUp:
                    dashCooldown = 5f;
                    break;
                default:
                    break;
            }
        }
        foreach (Transform enfant in this.transform)
            if (enfant.gameObject.GetComponent<GunManager>() != null)
                foreach (Transform gun2 in enfant)
                {
                    if (gun2.GetComponent<Gun>().modele == ModelesGun.M1911)
                    {
                        gun2.gameObject.SetActive(true);
                        gun = gun2.GetComponent<Gun>();
                    }
                    else
                        gun2.gameObject.SetActive(false);
                }
        hp = 100;
        colasOwned.Clear();
        nbCola = 0;
        ChangeSpeed(0.2f);
    }

    public void Revive()
    {
        foreach (Transform enfant in this.transform)
            if (enfant.gameObject.GetComponent<GunManager>() != null)
            {
                foreach (Transform gun2 in enfant)
                    if (gun2.GetComponent<Gun>().modele == ModelesGun.M1911)
                        gun2.gameObject.SetActive(false);
                enfant.gameObject.GetComponent<GunManager>().ChangerDarme(1);
            }
        isDown = false;
        beingRevived = false;
        hp = maxHealth;
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
        bulletPrefab.GetComponent<Bullet>().deathParticles.GetComponent<ParticleSystemRenderer>().sharedMaterial = material;
    }

    public void ChangeSpeed(float multiplier)
    {
        speed = speedConst * multiplier;
    }

    public bool VerifyCola(TypeCola typeCola)
    {
        foreach (TypeCola cola in colasOwned)
            if (typeCola == cola)
                return true;
        return false;
    }
}