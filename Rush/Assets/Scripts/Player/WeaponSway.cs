using System;
using UnityEngine;

public class WeaponSway : MonoBehaviour {

    [SerializeField] private float smooth;
    [SerializeField] private float multiplier;
    public float tiltAngle;
    private Rifle rifle;
    void Awake()
    {
        rifle = GameObject.Find("Rifle").GetComponent<Rifle>();
    }
    private void FixedUpdate()
    {
        // get mouse input
        float mouseX = Input.GetAxisRaw("Mouse X") * multiplier;
        float mouseY = Input.GetAxisRaw("Mouse Y") * multiplier;

        // calculate target rotation
        Quaternion rotationX = Quaternion.AngleAxis(-mouseY, Vector3.right);
        Quaternion rotationY = Quaternion.AngleAxis(mouseX, Vector3.up);

        Quaternion recoil = Quaternion.AngleAxis(-Mathf.Min(rifle.recoil, 6f), Vector3.right);
        Quaternion tilt = Quaternion.AngleAxis(tiltAngle, Vector3.forward);
        Quaternion targetRotation = (rotationX * recoil) * rotationY * tilt;

        // rotate 
        transform.localRotation = Quaternion.Slerp(transform.localRotation, targetRotation, smooth * Time.deltaTime);
    }
}