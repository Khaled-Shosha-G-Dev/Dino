using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    [SerializeField] private float view;
    [SerializeField] private Transform PlayerTransform;
    // Update is called once per frame
    void LateUpdate()
    {
        transform.position = PlayerTransform.position+new Vector3(0,1,view);
    }
}
