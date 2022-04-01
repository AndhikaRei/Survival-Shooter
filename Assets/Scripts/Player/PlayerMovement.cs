using UnityEngine;
public class PlayerMovement : MonoBehaviour
{
	public float speed = 6f;
	Vector3 movement;
	Animator anim;
	Rigidbody playerRigidBody;
	int floorMask;
	float camRayLength = 100f;

	private void Awake() {
		// Mendapatkan nilai mask dari layer yang bernama floor.
		floorMask = LayerMask.GetMask("Floor");

		// Mendapatkan komponen animator.
		anim = GetComponent<Animator>();

		// Mendapatkan komponen rigidbody.
		playerRigidBody = GetComponent<Rigidbody>();
	}

	// TODO: This part is commented because we already used command pattern. 
	// private void FixedUpdate() {
	// 	// Mendapatkan nilai input horizontal (-1,0,1)
	// 	float h = Input.GetAxisRaw("Horizontal");

	// 	// Mendapatkan nilai input vertical (-1,0,1)
	// 	float v = Input.GetAxisRaw("Vertical");

	// 	// Membuat movement berdasarkan input horizontal dan vertical.
	// 	Move(h, v);
	// 	Turning();
	// 	Animating(h, v);
	// }

	public void Animating(float h, float v) {
		bool walking = h != 0f || v != 0f;
		anim.SetBool("IsWalking", walking);
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
}
