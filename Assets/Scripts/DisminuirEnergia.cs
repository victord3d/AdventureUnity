using UnityEngine;
using System.Collections;

public class DisminuirEnergia : MonoBehaviour {

	private InventarioDelJugador inventarioJugador;
	public int tipoDeDisminucion; 

	void Start () {
	
		inventarioJugador = GameObject.FindGameObjectWithTag ("Player").GetComponent<InventarioDelJugador> ();

	}

	void OnTriggerEnter(Collider Otro)
	{
		if(Otro.tag == "Player")
		{
			if(tipoDeDisminucion == 1)
			{
				inventarioJugador.energia -= 0.1f;
			}
			if(tipoDeDisminucion == 2)
			{
				inventarioJugador.energia = 0.0f;	
			}


		}
	}

}
