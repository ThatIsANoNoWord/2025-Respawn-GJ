using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public GameObject followObject;
    public GameObject dayNightEffect;
    public float maxXDiff;
    public float maxYDiff;

    // Update is called once per frame
    void FixedUpdate()
    {
        if (followObject.transform.position.x > transform.position.x + maxXDiff)
        {
            transform.position += Vector3.right * (followObject.transform.position.x - transform.position.x - maxXDiff);
        }
        if (followObject.transform.position.x < transform.position.x - maxXDiff)
        {
            transform.position += Vector3.right * (followObject.transform.position.x - transform.position.x + maxXDiff);
        }
        if (followObject.transform.position.y > transform.position.y + maxYDiff)
        {
            transform.position += Vector3.up * (followObject.transform.position.y - transform.position.y - maxYDiff);
        }
        if (followObject.transform.position.y < transform.position.y - maxYDiff)
        {
            transform.position += Vector3.up * (followObject.transform.position.y - transform.position.x + maxYDiff);
        }

        dayNightEffect.transform.position = new Vector3(followObject.transform.position.x, transform.position.y, 0);

    }
}
