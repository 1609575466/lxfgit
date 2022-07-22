using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;

[Serializable]
public class PlayerController : MonoBehaviour
{
   [Header("BaseParameters")]
    public float walkSpeed = 1f;
    public float rotateSpeed = 10f;
    public Transform muzzlePos;//枪口位置
    WaitForSeconds interval = new WaitForSeconds(2f);
    private void Update()
    {
        Control();
        Fire();
        StopFire();
    }

    private void Control()
    {
        if (Input.GetKey(KeyCode.W))
        {
            transform.Translate(Vector3.forward * (Time.deltaTime * walkSpeed));
        }
        if (Input.GetKey(KeyCode.S))
        {
            transform.Translate(Vector3.back * (Time.deltaTime * walkSpeed));
        }
        if (Input.GetKey(KeyCode.A))
        {
            transform.Rotate(Vector3.down * (Time.deltaTime * rotateSpeed));
        }
        if (Input.GetKey(KeyCode.D))
        {
            transform.Rotate(Vector3.up * (Time.deltaTime * rotateSpeed));
        }
    }

    private void Fire()
    {

        if (Input.GetMouseButtonDown(0))
            StartCoroutine(nameof(FireCoroutine));

    }

    private void StopFire()
    {

        if (!Input.GetMouseButtonDown(0))
            StopCoroutine(nameof(FireCoroutine));
    }
    IEnumerator FireCoroutine()
    {
        while (true)
        {
            GameObject curGo = PoolManager.Instance.GetFromPool("Sphere", muzzlePos.position, Quaternion.identity);
            curGo.GetComponent<Rigidbody>().AddForce(transform.forward * 20f);
            
            yield return interval;
        }
    }
}
