// ILSpyBased#2
using UnityEngine;

[AddComponentMenu("Weapon Control/WeaponLook")]
public class WeaponLook : MonoBehaviour
{
    public float sensitivityX = 200f;

    public float sensitivityY = 200f;

    public float minimumX = -360f;

    public float maximumX = 360f;

    public float minimumY = -13f;

    public float maximumY = 25f;

    private float rotationY;

    private Quaternion originalRotation;

    public bool Active;

    private float lastY;

    private void Update()
    {
    }
}


