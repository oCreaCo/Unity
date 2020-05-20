using UnityEngine;
using System.Collections;

public class PigCloudScript : MonoBehaviour {

    public Transform pigCloud;
    float timeToInstCloud = 0;
    [SerializeField] private float cloudRate = 6;

    Transform cloudPoint;

    void Awake ()
    {
        cloudPoint = transform.FindChild("cloudPoint");
        if (cloudPoint == null)
        {
            Debug.LogError("No 'cloudPoint' object!");
        }
        if (pigCloud == null)
        {
            Debug.LogError("No 'pigCloud' object");
        }
    }
	
	void Update () {
	    if (Time.time > timeToInstCloud)
        {
            cloudInst();
            timeToInstCloud = Time.time + 1 / cloudRate;
        }
	}

    void cloudInst ()
    {
        Transform clone = Instantiate(pigCloud, cloudPoint.transform.position, cloudPoint.transform.rotation) as Transform;
        Destroy(clone.gameObject, 0.72f);
    }
}
