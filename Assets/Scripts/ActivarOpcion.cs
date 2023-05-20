using UnityEngine;
using System.Collections;

public class ActivarOpcion : MonoBehaviour {

	GestorDeActividades gActividades;
	ControladorDelJugador cJugador; // i indicador


	void Start () {

		gActividades = GameObject.FindGameObjectWithTag ("Player").GetComponent<GestorDeActividades> ();
		cJugador = GameObject.FindGameObjectWithTag ("Player").GetComponent<ControladorDelJugador> ();

	}
		

	void OnTriggerEnter(Collider Otro)
	{
		
		for(int i=0; i < gActividades.actividades.Length; i++)
		{
			
			if(gActividades.actividades[i].puzleCompletado == false && gActividades.actividades[i].activarZonaPuzle == true)
			{ 
				if(gActividades.actividades[i].tipo == 1 || gActividades.actividades[i].tipo == 2 || gActividades.actividades[i].tipo == 3 || gActividades.actividades[i].tipo == 5)
				{
					for (int j=0; j < gActividades.actividades[i].opciones.Length; j++)
					{
						if(gameObject.GetInstanceID() == gActividades.actividades[i].opciones[j].zonaOpcion.GetInstanceID())
						{
							gActividades.actividades[i].opciones[j].activo = true;
							gActividades.actividades[i].opciones[j].opcion.GetComponent<Renderer>().material.color = gActividades.actividades[i].opciones[j].colorOpcion;

						}
					}
				}

				if(gActividades.actividades[i].tipo == 4)
				{
					if(Otro.tag == "Planta")
					{
						for (int j = 0; j < gActividades.actividades [i].opciones.Length; j++) 
						{
							if (gameObject.GetInstanceID () == gActividades.actividades [i].opciones [j].zonaOpcion.GetInstanceID ()) 
							{
								gActividades.actividades[i].opciones[j].activo = true;
								gActividades.actividades[i].opciones[j].opcion.GetComponent<Renderer>().material.color = gActividades.actividades[i].opciones[j].colorOpcion;

								for (int k = 0; k < cJugador.objetosClaves.Length; k++) 
								{
									if (cJugador.objetosClaves [k].tipo == 4 && gActividades.actividades[i].opciones[j].correcta == true) 
									{
										// Ubicacion de la planta en la maceta
										cJugador.objetosClaves[k].objetoReal.transform.position = new Vector3(transform.position.x, 0.13f,  transform.position.z);
										cJugador.objetosClaves[k].objetoReal.transform.rotation = transform.rotation;
										cJugador.objetosClaves [k].objetoReal.GetComponent<Rigidbody> ().constraints = RigidbodyConstraints.FreezeAll;
										cJugador.objetosClaves [k].disponible = false;
									}
								}

							}
						}
					}
				}
					
			}
				
		}
	}
}
