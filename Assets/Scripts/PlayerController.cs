using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class PlayerController : MonoBehaviour {
    public float lookSensitivity = 1.0f;
    public float moveSpeed = 5.0f;
    public float jumpSpeed = 5.0f;
    public bool clipWhileFlying = false;
    public float groundedTime = 0.0f;

    public const float MAX_STEEP = 50.0f;
    public const float TRY_JUMP_LENIENCE = 0.2f;

    public Animator anim;

    public Rigidbody rigid { get; private set; }
    Collider col;

    readonly RaycastHit[] hits = new RaycastHit[16];

    bool flyMode = false;
    float jumpCooldown = 0.0f;
    float timeSinceJump = 100.0f;
    float timeSinceTryJump = 100.0f;

    float pitch = 0.0f;

    Transform camRoot;

    // Use this for initialization
    void Start() {
        rigid = GetComponent<Rigidbody>();
        col = GetComponentInChildren<Collider>();

        camRoot = transform.Find("CamRoot");

        rigid.useGravity = !flyMode;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

    }


    public void ToggleFlyMode() {
        flyMode = !flyMode;
        //rigid.isKinematic = flyMode;
        rigid.useGravity = !flyMode;
        col.enabled = clipWhileFlying || !flyMode;
    }

    private void OnApplicationFocus(bool focus) {
        if (focus) {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        } else {
            Cursor.visible = true;
        }
    }

    public void ZeroPitch() {
        pitch = 0.0f;
    }

    // Update is called once per frame
    void Update() {
        timeSinceJump += Time.deltaTime;
        timeSinceTryJump += Time.deltaTime;
        jumpCooldown -= Time.deltaTime;

        if (Input.GetKeyDown(KeyCode.F1)) {
            ToggleFlyMode();
        }

        if (Input.GetButtonDown("Jump")) {
            timeSinceTryJump = 0.0f;
        }

        float yaw = Input.GetAxis("Mouse X") * lookSensitivity;
        //pitch = -25f;
        pitch += Input.GetAxis("Mouse Y") * lookSensitivity;
        pitch = Mathf.Clamp(pitch, -35, -20);
        //pitch = Mathf.Clamp(pitch, -89, 89); // annoying to clamp pitch if dont save variable

        transform.Rotate(Vector3.up, yaw, Space.Self);
        camRoot.localRotation = Quaternion.AngleAxis(pitch, Vector3.left);

        Vector3 forward = transform.forward;
        forward.y = 0.0f;
        forward = forward.normalized;

        Vector3 move = forward * Input.GetAxisRaw("Vertical") + transform.right * Input.GetAxisRaw("Horizontal");

        float speed = moveSpeed;
        //if (Input.GetKey(KeyCode.LeftControl)) {
        //    speed *= 3;
        //}

        move = move.normalized * speed;

        //Run 
        if (Input.GetAxisRaw("Vertical") != 0 || Input.GetAxisRaw("Horizontal") != 0)
        {
            anim.SetBool("Run", true);
        }
        else
        {
            anim.SetBool("Run", false);
        }

        if (flyMode) {
            float upDir = 0.0f;
            if (Input.GetKey(KeyCode.LeftShift)) {
                upDir -= 1.0f;
            } else if (Input.GetKey(KeyCode.Space)) {
                upDir += 1.0f;
            }

            move += upDir * Vector3.up * speed;
        } else {    // check for jump and grounded stuff
            float yvel = rigid.velocity.y;
            Vector3 start = transform.position + Vector3.up * 0.4f;
            //bool grounded = Physics.SphereCast(start, 0.5f, Vector3.down, 0.75f, 1);
            int count = Physics.SphereCastNonAlloc(start, 0.4f, Vector3.down, hits, 0.2f);

            bool grounded = false;
            Vector3 normal = Vector3.zero;
            for (int i = 0; i < count; ++i) {
                if (hits[i].collider != col && !hits[i].collider.isTrigger) {
                    grounded = true;
                    normal += hits[i].normal;
                }
            }
            if (grounded) {
                groundedTime += Time.deltaTime;
                anim.SetBool("Jump", false);
            } else {
                groundedTime = 0.0f;
                anim.SetBool("Jump", true);
            }

            bool onSteep = grounded && Vector3.Angle(Vector3.up, normal.normalized) > MAX_STEEP;

            if (onSteep) {
                move.x = rigid.velocity.x;
                move.z = rigid.velocity.z;
            }

            //Debug.DrawRay(transform.position, normal, onSteep ? Color.magenta : Color.green, 5.0f);
            //Debug.Log(grounded + " " + onSteep);

            if (timeSinceTryJump < TRY_JUMP_LENIENCE && grounded && !onSteep && jumpCooldown <= 0.0f) {
                grounded = false;
                yvel = jumpSpeed;
                jumpCooldown = 0.25f;
                timeSinceJump = 0.0f;
            }
            move.y = yvel;

            rigid.useGravity = true;
            if (groundedTime > 0.25f && !onSteep && timeSinceJump > 0.2f) {
                if (move.x == 0 && move.z == 0) { // dont apply gravity if grounded and no inputs
                    rigid.useGravity = false;
                }
                move = Vector3.ProjectOnPlane(move, normal);
            }

        }

        rigid.velocity = move;
    }

}
