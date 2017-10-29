using UnityEngine;
using UnityEngine.Networking;

public class PlayerController : NetworkBehaviour
{
    public GameObject bulletPrefab;
    public Transform bulletSpawn;

    public float speed = 3.0f;
    public float jumpHeight = 50.0f;

    private Rigidbody rigidBody;
    
    public override void OnStartLocalPlayer()
    {
        GetComponent<MeshRenderer>().material.color = Color.blue;
    }

    void Start()
    {
        rigidBody = GetComponent<Rigidbody>();
    }

    void Update()
    {
        if (!isLocalPlayer)
            return;

        var xPosition = Input.GetAxis("Horizontal") * Time.deltaTime * speed;
        var zPosition = Input.GetAxis("Vertical") * Time.deltaTime * speed;

        var xRotation = Input.GetAxis("Mouse X");
        //var yRotation = Input.GetAxis("Mouse Y");

        transform.Rotate(0, xRotation, 0);
        transform.Translate(xPosition, 0, zPosition);

        if (Input.GetKeyDown(KeyCode.Mouse0))
            CmdFire();

        if (Input.GetKeyDown(KeyCode.Space))
        {
            rigidBody.AddForce(new Vector3(0, jumpHeight, 0), ForceMode.Impulse);
        }
    }

    [Command]
    void CmdFire()
    {
        var bullet = Instantiate(bulletPrefab, bulletSpawn.position, bulletSpawn.rotation);
        bullet.GetComponent<Rigidbody>().velocity = bullet.transform.forward * 6;

        NetworkServer.Spawn(bullet);

        Destroy(bullet, 2.0f);
    }
}
