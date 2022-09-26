using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParentToWrist : MonoBehaviour
{
    [SerializeField] private GameObject parentObject;
    private void Awake()
    {
        transform.parent = parentObject.transform;
    }
}
