using UnityEngine;


public class HeadLogic : MonoBehaviour
{
    private float minYAngle = -60f;
    private float maxYAngle = 60f;
    private float sightSpeed = 3f;
    private float currentAngle = 0;


    private void LateUpdate()
    {
        ChangePlayerViewY();
    }

    void ChangePlayerViewY()
    {
        // Rotation sur l’axe X
        float v = -Input.GetAxis("Mouse Y") * sightSpeed;
        currentAngle += v;
        if (currentAngle > maxYAngle || currentAngle < minYAngle)
        {
            v = 0;
        }
        transform.Rotate(v, 0, 0);
    }

}
