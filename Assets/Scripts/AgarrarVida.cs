using UnityEngine;
using System.Collections;

public class AgarrarVida : MonoBehaviour {

	InventarioDelJugador inventarioJugador;

	void Start () {

		inventarioJugador = GameObject.FindGameObjectWithTag ("Player").GetComponent<InventarioDelJugador> ();
	
	}

	void OnTriggerEnter(Collider Otro)
	{
		if(Otro.tag == "Player")
		{
			inventarioJugador.vidas++;
			gameObject.SetActive (false);

		}
	}
}
