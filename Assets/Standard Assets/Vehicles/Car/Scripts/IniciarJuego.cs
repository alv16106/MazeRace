using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IniciarJuego : MonoBehaviour {
	//public GameObject jugador;
	public Vector3[] positions;
	public GameObject puntos;
	public int cantidadP;
	public float resetDelay = 1f;
	public AudioSource termina;
	public GameObject efectoSpawn;
	public static IniciarJuego instance = null;

	void Awake(){
		instance = this;
		SetUp ();
	}

	// Use this for initialization
	void SetUp () {
		//int posIn = Random.Range (0, 3);
		//Instantiate(jugador, positions[posIn], Quaternion.identity);
		//Instantiate(efectoSpawn, positions[posIn], Quaternion.identity);
		for (int i = 0; i < cantidadP; i++) {
			CrearPuntos ();
		}
	}

	public void isTerminado (int puntaje) {
		if (puntaje >= 10) {
			juegoAcabado ();
		} else {
			return;
		}
	}

	public void juegoAcabado(){
		termina.Play();
		Invoke ("Reset", resetDelay);
	}

	void Reset()
	{
		Time.timeScale = 1f;
		//Application.LoadLevel(Application.loadedLevel);
	}

	void CrearPuntos(){
		int x = Random.Range (-80, 65);
		int z = Random.Range (-70,60);
		Instantiate(puntos, new Vector3(x,0,z), Quaternion.identity);
	}
}