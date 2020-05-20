using UnityEngine;
using System.Collections;

public class WeaponRotation : MonoBehaviour{

	[SerializeField] private int offset = 0;

	// Update is called once per frame
	void Update ()
    { 

        //subtracting the position of the player from the mouse position
        Vector3 difference = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
        difference.Normalize();

        float rotZ = Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg; // finding the angle in degrees
        transform.rotation = Quaternion.Euler(0f, 0f, rotZ + offset);

	}
}
