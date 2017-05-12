using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.Collections;

public class Health : NetworkBehaviour {
	public GameObject playerExplosion;
	public Text score;
	public Text time;
	public GameObject youWon;
	public int puntaje;
	public const int maxHealth = 100;
	public bool destroyOnDeath;

	[SyncVar(hook = "OnChangeHealth")]
	public int currentHealth = maxHealth;

	public Text healthBar;

	private NetworkStartPosition[] spawnPoints;

	void Start ()
	{
		if (isLocalPlayer)
		{
			spawnPoints = FindObjectsOfType<NetworkStartPosition>();
		}
	}

	void OnCollisionEnter(Collision other){
		if (other.collider.tag == "puntos") {
			Instantiate (playerExplosion, other.collider.transform.position, other.collider.transform.rotation);
			Destroy (other.collider.gameObject);
			puntaje++;
			score.text = "Puntaje: " + puntaje;
			IniciarJuego.instance.isTerminado (puntaje);
		} else if (other.collider.tag == "ganador") {
			youWon.SetActive (true);
			Time.timeScale = .25f;
		} else if (other.collider.tag == "bala") {
			TakeDamage (10);
		} else {
			return;
		}
	}

	public void TakeDamage(int amount)
	{
		if (!isServer)
			return;

		currentHealth -= amount;
		if (currentHealth <= 0)
		{
			if (destroyOnDeath)
			{
				Destroy(gameObject);
			} 
			else
			{
				currentHealth = maxHealth;

				// called on the Server, invoked on the Clients
				RpcRespawn();
			}
		}
	}

	void OnChangeHealth (int currentHealth )
	{
		healthBar.text = "Health: " + currentHealth;
	}

	[ClientRpc]
	void RpcRespawn()
	{
		if (isLocalPlayer)
		{
			// Set the spawn point to origin as a default value
			Vector3 spawnPoint = Vector3.zero;

			puntaje = 0;
			score.text = "Puntaje: " + puntaje;

			// If there is a spawn point array and the array is not empty, pick one at random
			if (spawnPoints != null && spawnPoints.Length > 0)
			{
				spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)].transform.position;
			}

			// Set the player’s position to the chosen spawn point
			transform.position = spawnPoint;
		}
	}
}