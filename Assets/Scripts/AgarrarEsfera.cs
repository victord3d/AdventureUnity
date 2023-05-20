using UnityEngine;
using System.Collections;

public class AgarrarEsfera : MonoBehaviour {

	InventarioDelJugador inventarioJugador;
	GestorDeActividades gActividades;

	void Start () {
		
		inventarioJugador = GameObject.FindGameObjectWithTag ("Player").GetComponent<InventarioDelJugador> ();
		gActividades = GameObject.FindGameObjectWithTag ("Player").GetComponent<GestorDeActividades> ();
	}


	void OnTriggerEnter(Collider Otro)
	{
		if(Otro.tag == "Player")
		{
			inventarioJugador.esferasAgarradas++;
			gameObject.SetActive (false);

			for(int i=0; i<gActividades.actividades.Length; i++)
			{
				if (gameObject.GetInstanceID () == gActividades.actividades[i].esfera.GetInstanceID()) 
				{
					gActividades.actividades [i].jugadorEsfera = true;
				}
			}
		}
	}
}
