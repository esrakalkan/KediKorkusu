using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bakis : MonoBehaviour
{
    private Vector2 mD;
    private float inputH;
    private float inputVMouse;
    // Start is called before the first frame update


    private Transform myBody;
    void Start()
    {
        myBody = this.transform.parent.transform.parent;
    }

    // Update is called once per frame
    void Update()
    {

        Vector2 mC = new Vector2(Input.GetAxisRaw("Mouse X"), Input.GetAxisRaw("Mouse Y"));

        mD += mC;

        this.transform.localRotation = Quaternion.AngleAxis(-mD.y, Vector3.right);
        myBody.localRotation = Quaternion.AngleAxis(mD.x, Vector3.up);

    }
}