using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour
{
	// misca camera la fiecare click
    public void MoveCamera()
    {
        transform.position += new Vector3(0,0.1f,0);
    }
 

}
