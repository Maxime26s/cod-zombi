using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum TypePowerUp { MaxAmmo, OneShot, MOAB, Carpenter, DoublePoint, FireSale, ReviveAll }
public class PowerUp : MonoBehaviour
{
    public TypePowerUp type;
    public float rotationSpeed, sinSpeed;
    Vector3 startPosition;
    float startTime;

    // Start is called before the first frame update
    void Start()
    {
        startPosition = transform.position;
        startTime = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        transform.eulerAngles += new Vector3(0,rotationSpeed*Time.deltaTime,0);
        transform.position = startPosition + new Vector3(0, sinSpeed*Mathf.Sin(Time.time - startTime), 0);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            switch (type)
            {
                case TypePowerUp.MaxAmmo:
                    GameObject.Find("GameManager").GetComponent<GameManager>().MaxAmmo();
                    break;
                case TypePowerUp.Carpenter:
                    GameObject.Find("GameManager").GetComponent<GameManager>().Carpenter();
                    break;
                case TypePowerUp.MOAB:
                    GameObject.Find("GameManager").GetComponent<GameManager>().MOAB();
                    break;
                case TypePowerUp.DoublePoint:
                    GameObject.Find("GameManager").GetComponent<GameManager>().DoublePoint();
                    break;
                case TypePowerUp.OneShot:
                    GameObject.Find("GameManager").GetComponent<GameManager>().OneShot();
                    break;
                case TypePowerUp.FireSale:
                    break;
                case TypePowerUp.ReviveAll:
                    break;
            }
            Destroy(gameObject);
        }
    }
}
