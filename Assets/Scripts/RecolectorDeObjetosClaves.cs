using UnityEngine;
using System.Collections;

public class RecolectorDeObjetosClaves : MonoBehaviour {

	ControladorDelJugador cJugador; 	// i indicador
	public int tipoDeObjetoARecibir;	// 1. Regadera - 2.Suelos - 3. Cajas - 4. Planta - 5. Tronco 
	public GameObject posObjeto;		// Posicion del objeto a colocar en el piso

	GestorDeActividades gActividades;
	public int idActividad;

	int contObjetosClaves;
	public int idObjetoARecibir;

	GameObject sueloDinamico;

	bool objetoColocado; // Para que solo cuente los objetos colocados en el recolector

	//Casa duende
	GameObject casaDuende;

	void Awake()
	{
		sueloDinamico = GameObject.Find ("SueloDinamico");
		casaDuende = GameObject.Find ("Casa");
	}
		
	void Start () {
		cJugador = GameObject.FindGameObjectWithTag ("Player").GetComponent<ControladorDelJugador> ();
		gActividades = GameObject.FindGameObjectWithTag ("Player").GetComponent<GestorDeActividades> ();
		contObjetosClaves = 0;

		sueloDinamico.SetActive(false);

		objetoColocado = false;

	}

	void OnTriggerEnter(Collider Otro)
	{
		
		for(int i=0 ; i<cJugador.objetosClaves.Length; i++)
		{
			if (cJugador.objetosClaves [i].agarrado == true) 
			{

				if (tipoDeObjetoARecibir == cJugador.objetosClaves [i].tipo ) 
				{


					if(cJugador.objetosClaves [i].tipo != 5 && idObjetoARecibir == cJugador.objetosClaves [i].idObjeto)
					{

						cJugador.objetosClaves[i].agarrado = false;

						// apago el objeto animado
						// y coloco el real en el piso
						cJugador.objetosClaves[i].objetoAnimacion.SetActive (false);

						cJugador.objetosClaves[i].objetoReal.transform.position = posObjeto.transform.position;
						posObjeto.SetActive (false);

						cJugador.objetosClaves[i].objetoBase.SetActive (true);

						cJugador.objetosClaves[i].disponible = false;

						cJugador.objetosClaves [i].colocadoEnDestino = true;

						// desactivar la zona para que no coloque el objeto y coloque el que le corresponde
						gameObject.SetActive(false);
						cJugador.objetosClaves [i].objetoReal.GetComponent<Rigidbody> ().constraints = RigidbodyConstraints.FreezeAll;
						objetoColocado = true;

					}

					if(cJugador.objetosClaves [i].tipo == 5)
					{
						cJugador.objetosClaves[i].agarrado = false;
						cJugador.objetosClaves[i].objetoAnimacion.SetActive (false);

						cJugador.objetosClaves[i].objetoReal.transform.position = posObjeto.transform.position;

						cJugador.objetosClaves[i].disponible = false;
						cJugador.objetosClaves [i].colocadoEnDestino = true;
						contObjetosClaves++;

					}

					// Activa zona de puzle despues de que coloca el objeto utilizando id Asignado manualmente
					for(int j=0; j<gActividades.actividades.Length; j++)
					{
						if(gActividades.actividades[j].idActividad == idActividad)
						{
							if(gActividades.actividades[j].tipo == 3)
							{
								gActividades.actividades [j].activarZonaPuzle = true;	
							}

							if (gActividades.actividades [j].tipo == 4) 
							{

								if(objetoColocado == true)
								{
									for(int g=0; g<cJugador.objetosClaves.Length; g++)
									{
										if(cJugador.objetosClaves [g].tipo == 2 && cJugador.objetosClaves [g].colocadoEnDestino == true)
										{
											contObjetosClaves++;
										}
									}

									objetoColocado = false;
								}


								// Al cololocar los 3 objetos claves de este tipo de actividad
								if(contObjetosClaves==3)
								{
									print ("Coloco los 3 ");
									// Colocar los 3 cubos
									// Activar planta

									for(int g=0; g<cJugador.objetosClaves.Length; g++)
									{
										// Objeto de tipo 4 Planta
										if(cJugador.objetosClaves[g].tipo == 4)
										{
											cJugador.objetosClaves [g].disponible = true;


										}
									}

									// Activar Suelo para llegar a la planta
									sueloDinamico.SetActive (true);

									// Activar zona puzle
									gActividades.actividades[j].activarZonaPuzle = true;

									contObjetosClaves =0;
								}
							} // Fin Actividad de tipo 4


							if (gActividades.actividades [j].tipo == 5) 
							{
								print ("Contador: "+ contObjetosClaves);



								if(contObjetosClaves==3)
								{
									print ("Coloco los 3 Troncos");
									// Colocar los 3 cubos

									//Desactivar zona de recolector
									gameObject.SetActive(false);

									// Activar zona puzle
									gActividades.actividades[j].activarZonaPuzle = true;

									//Activar casa del duende
									casaDuende.SetActive(true);

									contObjetosClaves = 0;
								}
							}

						}
					}
				}
			}
		}

	}
		
}
