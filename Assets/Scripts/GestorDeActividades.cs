using UnityEngine;
using System.Collections;

[System.Serializable]
public class Actividades
{
	public int idActividad;
	public int idDuende;
	public int numeroOpcionesCorrectas;
	public bool activarZonaPuzle;
	public bool revisarZonaPuzle;
	public bool puzleCompletado;
	public GameObject zona;
	public GameObject cuboEsfera;
	public GameObject esfera;
	public GameObject recompensa;		// Se debe colocar como referencia el Prefab del objeto 
	public bool jugadorEsfera;
	public Opciones[] opciones;

	public int tipo;					// Puede ser: 1(Seleccion Multiple sin orden) 2(Seleccion Multiple con un orden) 3(Seleccion Multiple con un orden y un objeto)
	public bool repetir;				// Se activa si se quiere repetir la actividad

	public int contOpCorrectas;
	public bool[] verificador;

	public bool guardar;
}

[System.Serializable]
public class Opciones
{
	public GameObject opcion; 			// Mesh
	public GameObject zonaOpcion;		// Collider
	public bool activo;
	public bool correcta;
	public Color colorOpcion;

	public int numeroOrdenOpcion;		// Si tiene un numero es el orden y si es 0 es porque es una opcion incorrecta

}

public class GestorDeActividades : MonoBehaviour {

	// Revisa el puzle despues de que se activa la zona y el personaje se encuentra en la zona del puzle

	public Actividades[] actividades;

	bool incorrecta;

	GestorDeDialogos gDialogos;
	ControladorDelJugador cJugador; // i indicador
	GameObject Planta;
	Vector3 posIniPlanta;

	//CargarYGuardarPartida
	CargarYGuardarPartida guardarPartida;

	void Awake()
	{
		Planta = GameObject.FindWithTag ("Planta");

	}
		
	void Start () {

		incorrecta = false;


		for (int i = 0; i < actividades.Length; i++) 
		{
			actividades[i].verificador = new bool[actividades [i].opciones.Length];
			for (int j = 0; j < actividades [i].opciones.Length; j++) 
			{

				actividades[i].verificador [j] = true;

			}
		}

		for (int i = 0; i < actividades.Length; i++) 
		{
			for (int j = 0; j < actividades [i].opciones.Length; j++) 
			{
				if(actividades[i].opciones[j].correcta == true)
				{
					actividades [i].numeroOpcionesCorrectas++;
				}

			}

			actividades [i].contOpCorrectas = 0;

			//Inicializar Guardado
			actividades[i].guardar = false;
		}


		gDialogos = GameObject.FindGameObjectWithTag ("Player").GetComponent<GestorDeDialogos> ();
		// para la planta
		cJugador = GameObject.FindGameObjectWithTag ("Player").GetComponent<ControladorDelJugador> ();
		// pos planta
		posIniPlanta = new Vector3(Planta.transform.position.x,Planta.transform.position.y,Planta.transform.position.z);

		//CargarYGuardarPartida
		guardarPartida = GameObject.Find("GuardaryCargarScript").GetComponent<CargarYGuardarPartida> ();
	}
		
	void Update () {

		for(int i=0; i < actividades.Length; i++)
		{
			if(actividades[i].puzleCompletado == false)
			{
				if(actividades[i].activarZonaPuzle == true)
				{
					if(actividades[i].revisarZonaPuzle == true)
					{
						//Revisando la actividad de Seleccion Multiple sin un orden
						if(actividades[i].tipo == 1 || actividades[i].tipo == 4 || actividades[i].tipo == 5)
						{

							for(int j=0; j < actividades[i].opciones.Length; j++)
							{

								if(actividades[i].opciones[j].activo == true && actividades[i].opciones[j].correcta == true)
								{
									if(actividades[i].verificador[j] == true)
									{
										actividades[i].contOpCorrectas++;
										print(actividades[i].contOpCorrectas);
										actividades[i].verificador[j] = false;
									}

								}

								if(actividades[i].opciones[j].activo == true && actividades[i].opciones[j].correcta == false)
								{
									incorrecta = true;	
								}
							}

							if(actividades[i].contOpCorrectas == actividades[i].numeroOpcionesCorrectas && incorrecta == false)
							{
								actividades[i].cuboEsfera.SetActive (false);

								//if(actividades[i].jugadorEsfera == true)
								//{
									print("Puzle Resuelto");
									actividades [i].puzleCompletado = true;
									actividades[i].contOpCorrectas = 0; // Se coloca porque sino cuando se va hacia otra actividad quedan guardadas el numero de opciones correctas
								//}
									
							}

							if(incorrecta == true)
							{
								print ("Incorrecto");

								for (int j = 0; j < actividades [i].opciones.Length; j++) 
								{
									actividades [i].opciones [j].activo = false;
									actividades[i].verificador[j] = true;
									actividades [i].opciones [j].opcion.GetComponent<Renderer> ().material.color = Color.white;
								}

								actividades[i].contOpCorrectas = 0;
								incorrecta = false;


							}
						} // Fin Tipo de Actividad 1


						//Revisando la actividad de Seleccion Multiple con un orden
						if(actividades[i].tipo == 2 || actividades[i].tipo == 3)
						{

							for(int j=0; j < actividades[i].opciones.Length; j++)
							{

								if(actividades[i].opciones[j].activo == true && actividades[i].opciones[j].correcta == true)
								{
									if(actividades[i].verificador[j] == true)
									{
										actividades[i].contOpCorrectas++;

										if(actividades[i].contOpCorrectas == actividades[i].opciones[j].numeroOrdenOpcion)
										{
											print(actividades[i].contOpCorrectas);
											actividades[i].verificador[j] = false;

										}
										else
										{
											incorrecta = true;
										}

									}

								}

								if(actividades[i].opciones[j].activo == true && actividades[i].opciones[j].correcta == false)
								{
									incorrecta = true;	
								}
							}

							if(actividades[i].contOpCorrectas == actividades[i].numeroOpcionesCorrectas && incorrecta == false)
							{
								print("Puzle Resuelto");
								actividades [i].puzleCompletado = true;
								actividades[i].cuboEsfera.SetActive (false);

								actividades[i].contOpCorrectas = 0; // Se coloca porque sino cuando se va hacia otra actividad quedan guardadas el numero de opciones correctas
							}

							if(incorrecta == true)
							{
								print ("Incorrecto");

								for (int j = 0; j < actividades [i].opciones.Length; j++) 
								{
									actividades [i].opciones [j].activo = false;
									actividades[i].verificador[j] = true;
									actividades [i].opciones [j].opcion.GetComponent<Renderer> ().material.color = Color.white;
								}

								actividades[i].contOpCorrectas = 0;
								incorrecta = false;


							}
						} // Fin Tipo de Actividad 2



					}
				}
			}

			// Si se completa la actividad
			if(actividades[i].puzleCompletado == true)
			{
				// ejecuta en 1 momento
				if (actividades [i].jugadorEsfera == true && actividades [i].guardar == true) 
				{
					//Antes siempre revisaba cuando un puzle se completaba entonces hay q revisar si se completo y si el duende correspondiente esta activo
					for(int d = 0; d < gDialogos.duendes.Length; d++)
					{

						if(gDialogos.duendes[d].idDuende == actividades [i].idDuende)
						{
							if(gDialogos.duendes[d].tipo == "Aprendizaje")
							{
								// Verifica si el puzle que se completo es el del duende q esta activo. Si es asi cambia su estado a repetir y el siguiente lo coloca activo
								if(gDialogos.duendes[d].estado == "Activo")
								{
									gDialogos.duendes[d].estado = "Repetir";
									gDialogos.duendes[d].colorGorroDuende = Color.blue;
									gDialogos.duendes[d].gorroDuende.GetComponent<Renderer>().material.color = gDialogos.duendes[d].colorGorroDuende;

									for(int n = 0; n < gDialogos.duendes.Length; n++)
									{
										if(gDialogos.duendes[n].idDuende == actividades [i].idDuende + 1)
										{

											gDialogos.duendes[n].estado = "Activo";
											gDialogos.duendes[n].colorGorroDuende = Color.green;
											gDialogos.duendes[n].gorroDuende.GetComponent<Renderer>().material.color = gDialogos.duendes[n].colorGorroDuende;


										}
									}


								}

								if(gDialogos.duendes[d].estado == "Repetir" && actividades[i].repetir == true)
								{
									//Reiniciar todos los valores para repetir la actividad

									for (int j = 0; j < actividades [i].opciones.Length; j++) 
									{
										actividades [i].opciones [j].activo = false;
										actividades[i].verificador[j] = true;
										actividades [i].opciones [j].opcion.GetComponent<Renderer> ().material.color = Color.white;
									}

									// Generar diamante cuando se repite la actividad
									Instantiate(actividades[i].recompensa, actividades[i].esfera.transform.position, actividades[i].esfera.transform.rotation);
									actividades [i].puzleCompletado = false;
									actividades[i].cuboEsfera.SetActive (true);
									actividades[i].repetir = false;

									if(actividades[i].tipo == 4)
									{

										for (int k = 0; k < cJugador.objetosClaves.Length; k++) 
										{
											if (cJugador.objetosClaves [k].tipo == 4) 
											{
												cJugador.objetosClaves [k].objetoReal.GetComponent<Rigidbody> ().constraints = RigidbodyConstraints.None;
												cJugador.objetosClaves [k].objetoReal.GetComponent<Rigidbody> ().constraints = RigidbodyConstraints.FreezeRotation;
												cJugador.objetosClaves [k].disponible = true;
												cJugador.objetosClaves [k].objetoReal.transform.position = posIniPlanta;


													
											}
										}

										
									}
								}

							}


						}



					}

					// Guardado automatico
					guardarPartida.GuardarPartida();
					actividades [i].guardar = false;
				}



			} // Fin de Puzle Completado


		}


	}
}
