using System;
using UnityEngine;
using UnityEngine.UI;
using UnityStandardAssets.CrossPlatformInput;
using UnityEngine.Networking;

namespace UnityStandardAssets.Vehicles.Car
{
    [RequireComponent(typeof (CarController))]
    public class CarUserControl : NetworkBehaviour
    {
		public GameObject shot;
		public Transform shotSpawn;
		public float fireRate;
		public AudioSource tiro;
		private float nextFire;
        private CarController m_Car; // the car controller we want to use
		public float tiempo;
		private GameObject g;

		void Start () {
			if (!isLocalPlayer){
				return;
			}
			g=transform.Find("Sphere").gameObject;
			g.active = true;
		}

        private void Awake()
        {
            // get the car controller
            m_Car = GetComponent<CarController>();
        }
			

		[Command]
		void CmdFire()
		{
			nextFire = Time.time + fireRate;
			// Create the Bullet from the Bullet Prefab
			var bullet = (GameObject)Instantiate(shot,shotSpawn.position,shotSpawn.rotation);

			// Spawn the bullet on the Clients
			NetworkServer.Spawn(bullet);

			// Destroy the bullet after 2 seconds
			Destroy(bullet, 2.0f);
		}

		private void Update(){
			if (!isLocalPlayer){
				return;
			}
			if (Input.GetButton("Fire1") && Time.time > nextFire) 
	        {
				tiro.Play ();
				CmdFire ();
	        }
		}
        private void FixedUpdate()
        {
			if (!isLocalPlayer){
				return;
			}
			//time.text = "Tiempo: "+tiempo.ToString("0.0");
			//tiempo = tiempo - Time.deltaTime;
            // pass the input to the car!
            float h = CrossPlatformInputManager.GetAxis("Horizontal");
            float v = CrossPlatformInputManager.GetAxis("Vertical");
#if !MOBILE_INPUT
            float handbrake = CrossPlatformInputManager.GetAxis("Jump");
            m_Car.Move(h, v, v, handbrake);
#else
            m_Car.Move(h, v, v, 0f);
#endif
        }
    }
}
