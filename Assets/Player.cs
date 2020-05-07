using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    CharacterController cc;
    public float speed, jump, gravity;
    bool onGround;
    Vector3 moveDirection = Vector3.zero;
    public GameObject bullet, bulletSpawnPoint, gun;
    public int hp = 10;
    public int money;

    private float nextShootingTime = 0f;
   
    // Start is called before the first frame update
    void Start()
    {
        cc = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        Plane plane = new Plane(Vector3.up, Vector3.zero);
        if(plane.Raycast(ray, out float length))
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
        if(!cc.isGrounded)
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
        if (Input.GetMouseButtonDown(0) && Time.time >= nextShootingTime && gun.GetComponent<Gun>().type == TypeGun.Semi)
        {
            nextShootingTime = Time.time + 1f / gun.GetComponent<Gun>().fireRate;
            GameObject thing = Instantiate(bullet, bulletSpawnPoint.transform.position, transform.rotation);
            thing.GetComponent<Bullet>().player = this;
            thing.GetComponent<Bullet>().direction = bulletSpawnPoint.transform.position - transform.position;
            thing.GetComponent<Bullet>().direction.y = 0;
            Destroy(thing, 1);
            gun.GetComponent<Gun>().ammo--;
        }
        if (Input.GetMouseButton(0) && Time.time >= nextShootingTime && gun.GetComponent<Gun>().type == TypeGun.Auto)
        {
            nextShootingTime = Time.time + 1f / gun.GetComponent<Gun>().fireRate;
            GameObject thing = Instantiate(bullet, bulletSpawnPoint.transform.position, transform.rotation);
            thing.GetComponent<Bullet>().player = this;
            thing.GetComponent<Bullet>().direction = bulletSpawnPoint.transform.position - transform.position;
            thing.GetComponent<Bullet>().direction.y = 0;
            Destroy(thing, 1);
            gun.GetComponent<Gun>().ammo--;
        }
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

