using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutoutObject : MonoBehaviour
{
    [SerializeField] private float _cutOutSize = 0.1f;
    [SerializeField] private float _fallOffSize = 0.03f;
    [SerializeField] private float _cutOutPosY = 0.05f;

    [SerializeField]
    private Transform targetObject;

    [SerializeField]
    private LayerMask wallMask;

    private Camera mainCamera;
    

    private void Awake()
    {
        mainCamera = GetComponent<Camera>();
        targetObject = GameObject.FindWithTag("Player").GetComponent<Transform>();
    }

    private void Update()
    {
        Vector2 cutoutPos = mainCamera.WorldToViewportPoint(targetObject.position);
        cutoutPos.y /= (Screen.width / Screen.height);
        cutoutPos.y += _cutOutPosY;
        Vector3 offset = targetObject.position - transform.position;
        RaycastHit[] hitObjects = Physics.RaycastAll(transform.position, offset, offset.magnitude, wallMask);

        for (int i = 0; i < hitObjects.Length; ++i)
        {
            Material[] materials = hitObjects[i].transform.GetComponent<Renderer>().materials;

            for (int m = 0; m < materials.Length; ++m)
            {
                materials[m].SetVector("_CutoutPos", cutoutPos);
                materials[m].SetFloat("_CutoutSize", _cutOutSize);
                materials[m].SetFloat("_FalloffSize", _fallOffSize);
            }
        }
    }
}
