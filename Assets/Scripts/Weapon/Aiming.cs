using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Aiming : MonoBehaviour
{
    [Header("CrossHair")]
    public RectTransform top;
    public RectTransform bottom;
    public RectTransform left;
    public RectTransform right;

    [Header("Distancias")]
    public float abierto = 30f;
    public float cerrado = 10f;
    public float velocidad = 10f;

    [Header("Aiming")]
    [SerializeField] private Vector3 aimingPos;
    [SerializeField] private Camera cam;
    [SerializeField] private float aimingSpeed;
    [SerializeField] private float newFOV;
    [SerializeField] private Vector3 aimBulletSpread;
    private Vector3 startAimBulletSpread;
    private float oldFOV;
    private bool aiming;
    [SerializeField] private Weapon weapon;

    private Vector3 startPos;
    private void Start()
    {
        startAimBulletSpread = weapon.bulletSpread;
        oldFOV = cam.fieldOfView;
        startPos = transform.localPosition;
    }
    private void Update()
    {
        if (Input.GetMouseButton(1))
        {
            aiming = true;
            transform.localPosition = Vector3.Lerp(
                transform.localPosition, aimingPos,
                aimingSpeed * Time.deltaTime);
            cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, newFOV, aimingSpeed * Time.deltaTime);
        }
        else
        {
            aiming = false;
            transform.localPosition = Vector3.Lerp(
                transform.localPosition, startPos,
                aimingSpeed * Time.deltaTime);
            cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, oldFOV, aimingSpeed * Time.deltaTime);
        }
        ActualizarCrosshair();
    }
    private void ActualizarCrosshair()
    {
        float target = aiming ? cerrado : abierto;
        if (Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0)
        {
            target += velocidad;
        }

        weapon.bulletSpread = (aimBulletSpread * target / 10);

        top.anchoredPosition = Vector2.Lerp(
            top.anchoredPosition,
            new Vector2(0, target),
            Time.deltaTime * velocidad
        );

        bottom.anchoredPosition = Vector2.Lerp(
            bottom.anchoredPosition,
            new Vector2(0, -target),
            Time.deltaTime * velocidad
        );

        left.anchoredPosition = Vector2.Lerp(
            left.anchoredPosition,
            new Vector2(-target, 0),
            Time.deltaTime * velocidad
        );

        right.anchoredPosition = Vector2.Lerp(
            right.anchoredPosition,
            new Vector2(target, 0),
            Time.deltaTime * velocidad
        );
    }
}
