using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum TypePowerUp { MaxAmmo, OneShot, MOAB, Carpenter, DoublePoint, FireSale, ReviveAll, InfiniteAmmo }
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
        if(other.CompareTag("Player"))
        {
            switch (type)
            {
                case TypePowerUp.MaxAmmo:
                    GameManager.Instance.MaxAmmo();
                    break;
                case TypePowerUp.Carpenter:
                    GameManager.Instance.Carpenter();
                    break;
                case TypePowerUp.MOAB:
                    GameManager.Instance.MOAB();
                    break;
                case TypePowerUp.DoublePoint:
                    GameManager.Instance.DoublePoint();
                    break;
                case TypePowerUp.OneShot:
                    GameManager.Instance.OneShot();
                    break;
                case TypePowerUp.FireSale:
                    GameManager.Instance.FireSale();
                    break;
                case TypePowerUp.ReviveAll:
                    GameManager.Instance.ReviveAll();
                    break;
                case TypePowerUp.InfiniteAmmo:
                    GameManager.Instance.InfiniteAmmo();
                    break;
            }
            Destroy(gameObject);
        }
    }
}
