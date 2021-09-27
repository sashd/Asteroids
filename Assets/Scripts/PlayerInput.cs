using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    public bool UseMouse
    {
        get { return useMouse; }
    }

    [SerializeField] private Player player;
    [SerializeField] private Camera cam;
    private static bool useMouse = false;
    
    private void Awake()
    {
        if (cam == null)
        {
            cam = Camera.main;
        }
    }

    private void Update()
    {
        float rotateDir = 0f;
        if (useMouse)
        {
            rotateDir = RotateByMouse();
            player.FollowMouse(rotateDir);
        }
        else
        {
            rotateDir = -Input.GetAxisRaw("Horizontal");
            player.Rotate(rotateDir);
        }   
        
        if (Input.GetAxisRaw("Vertical") == 1f || Input.GetButton("Thrust")) 
        {
            player.Thrust();
        }
        else
        {
            player.StopThrust();
        }

        if (Input.GetButtonDown("Shot"))
        {
            player.Shot();
        }
    }

    public void ChangeInput()
    {
        useMouse = !useMouse;
    }

    private float RotateByMouse()
    {
        var dir = cam.ScreenToWorldPoint(Input.mousePosition) - player.transform.position;
        float angleToPoint =  Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg - 90f;
        return angleToPoint;
    }

    
}
