using UnityEngine;
using System.Collections;

[RequireComponent(typeof(AudioSource))]
public class AgarrarDiamante : MonoBehaviour {

	InventarioDelJugador inventarioJugador;
	int cantDiamantesVida;

	AudioSource sonidoDiamante;

	void Start () {
		
		inventarioJugador = GameObject.FindGameObjectWithTag ("Player").GetComponent<InventarioDelJugador> ();

		cantDiamantesVida = 50;

		sonidoDiamante = GameObject.Find ("Diamantes").GetComponent<AudioSource>();
	}
		

	void OnTriggerEnter(Collider Otro)
	{
		if(Otro.tag == "Player")
		{
			sonidoDiamante.Play ();
			
			inventarioJugador.diamantesAgarrados++;
			gameObject.SetActive (false);

			if(inventarioJugador.energia < 1.0f)
			{
				inventarioJugador.energia += 0.1f; 
			}
				
			if(inventarioJugador.diamantesAgarrados == cantDiamantesVida)
			{
				inventarioJugador.vidas++;
				inventarioJugador.diamantesAgarrados = 0;
			}
		}
	}
}
