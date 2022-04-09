using UnityEngine;
using UnityEngine.UI;

class PlayerMovement : MonoBehaviour
{
	public GameObject mainCamera;
	public GameObject tpsView;
	public GameObject fpsView;

	public float speed = 6f;
	public float maxSpeed = 10f;    
	Vector3 movement;
	Animator anim;
	Rigidbody playerRigidBody;
	int floorMask;
	float camRayLength = 100f;
    Text speedText;

	private void Awake() {

		if (CameraModeManager.cameraMode == CameraMode.TPS)
        {
			mainCamera.SetActive(true);
			tpsView.SetActive(true);
			fpsView.SetActive(false);
        }
		else if (CameraModeManager.cameraMode == CameraMode.FPS)
        {
			mainCamera.SetActive(false);
			tpsView.SetActive(false);
			fpsView.SetActive(true);
		}

		// Mendapatkan nilai mask dari layer yang bernama floor.
		floorMask = LayerMask.GetMask("Floor");

		// Mendapatkan komponen animator.
		anim = GetComponent<Animator>();

		// Mendapatkan komponen rigidbody.
		playerRigidBody = GetComponent<Rigidbody>();

		// Get the speedText from hudcanvas.
		speedText = GameObject.Find("Speed").GetComponent<Text>();

	}

    // TODO: This part is commented because we already used command pattern. 
    private void FixedUpdate()
    {
        // Mendapatkan nilai input horizontal (-1,0,1)
        float h = Input.GetAxisRaw("Horizontal");

        // Mendapatkan nilai input vertical (-1,0,1)
        float v = Input.GetAxisRaw("Vertical");

		// Membuat movement berdasarkan input horizontal dan vertical.
		if (CameraModeManager.cameraMode == CameraMode.TPS)
		{
			Move(h, v);
			Turning();
			Animating(h, v);
		}
		else if (CameraModeManager.cameraMode == CameraMode.FPS)
        {
			FPSMove(h, v);
        }

		speedText.text = speed.ToString();
    }

    public void Animating(float h, float v) {
		bool walking = h != 0f || v != 0f;
		anim.SetBool("IsWalking", walking);
	}

	public void FPSMove(float h, float v) { 
		Vector3 move = transform.right * h + transform.forward * v;

		playerRigidBody.MovePosition(transform.position + move * speed * Time.deltaTime);
	}

	// Method berjalan.
	public void Move(float h, float v) {
		// Set nilai x dan y.
		movement.Set(h, 0f, v);

		// Menormalisasi nilai vector agar total panjang dari vector adalah 1.
		movement = movement.normalized * speed * Time.deltaTime;

		// Membuat komponen rigidbody bergerak.
		playerRigidBody.MovePosition(transform.position + movement);
	}

	public void Turning() {
		//Buat Ray dari posisi mouse di layar
		Ray camRay = Camera.main.ScreenPointToRay(Input.mousePosition);

		//Buat raycast untuk floorHit
		RaycastHit floorHit;

		//Lakukan raycast
		if (Physics.Raycast(camRay, out floorHit, camRayLength, floorMask)) {
			// Mendapatkan vector dari posisi player dan posisi floorHit
			Vector3 playerToMouse = floorHit.point - transform.position;
			playerToMouse.y = 0f;

			// Mendapatkan look rotation baru ke hit position.
			Quaternion newRotation = Quaternion.LookRotation(playerToMouse);

			// Rotasi player.
			playerRigidBody.MoveRotation(newRotation);
		}
	}

	public void PickupOrb(float amount)
    {
        // Menambahkan speed dengan amount.
        if(speed + amount >= maxSpeed){
            speed = maxSpeed;
        }else{
            speed += amount;
        }
        
        speedText.text = speed.ToString();
    }
}
