using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.InputSystem;
public class activateteleportationray : MonoBehaviour
{
    public GameObject leftTeleportation;
    public GameObject rightTeleportation;

    public InputActionProperty leftActive;
    public InputActionProperty rightActive;

    public InputActionProperty leftCancel;
    public InputActionProperty RightCancel;

    public XRRayInteractor leftRay;
    public XRRayInteractor RightRay;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        bool isLeftRayHovering = leftRay.TryGetHitInfo(out Vector3 leftpos, out Vector3 leftNormal, out int leftNumber, out bool leftValue);

        leftTeleportation.SetActive(!isLeftRayHovering && leftCancel.action.ReadValue<float>()==0&& leftActive.action.ReadValue<float>() > 0.1f);

        bool isRightRayHovering = RightRay.TryGetHitInfo(out Vector3 rightpos, out Vector3 rightNormal, out int rightNumber, out bool rightValue);

        rightTeleportation.SetActive(!isRightRayHovering&& RightCancel.action.ReadValue<float>() == 0&&rightActive.action.ReadValue<float>() > 0.1f);
    }
}
