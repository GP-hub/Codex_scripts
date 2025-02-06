using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_ClickToMove : Character_Move
{
    [SerializeField]
    private float speed;
    private Vector3 targetPosition;
    private bool isMoving;
    //public GameObject bulletPrefab;
    //public Transform bulletSpawn;

    void Start()
    {
        targetPosition = transform.position;
        isMoving = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            SetTargetPosition();
        }
        if (isMoving)
        {
            MovePlayer();
        }

        /*if (Input.GetKeyDown(KeyCode.Space))
        {
            CmdFire();
        }*/
    }

    void SetTargetPosition()
    {
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit))
        {
            targetPosition = new Vector3(hit.point.x, 0f, hit.point.z);
        }
        isMoving = true;
    }
    void MovePlayer()
    {
        transform.LookAt(targetPosition);
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);

        if (transform.position == targetPosition)
        {
            isMoving = false;
        }
    }

   /* void CmdFire()
    {
        GameObject bullet = (GameObject)Instantiate(bulletPrefab, bulletSpawn.position, bulletSpawn.rotation);
        bullet.GetComponent<Rigidbody>().velocity = bulletSpawn.transform.forward * 20;

        Destroy(bullet, 2.0f);

        //NetworkServer.Spawn(bullet);
    }*/
}
