using UnityEngine;
using System.Collections;

[System.Serializable]
public class Duende{

	public int idDuende;			// Indica el numero del duende
	public int idActividad;			// Identificador de la actividad que activa el duende
	public string tipo;				// Puede ser de habilidad, aprendizaje o acertijo
	public string estado;			// Puede ser Activo (Verde), Inactivo(Rojo) o Repetir(Azul)
	public GameObject zonaDuende;
	public Color colorDuende;

	public GameObject gorroDuende; 	// Gorro que muestra si el duende esta Activo, Inactivo o Repetir
	public Color colorGorroDuende;

	// Dialogos para los duendes de tipo Aprendizaje
	public string[] dialogoVerde;	// Dialogo que se muestra cuando el duende esta Activo
	public string[] dialogoRojo;	// Dialogo que se muestra cuando el duende esta Inactivo
	public string[] dialogoAzul;	// Dialogo que se muestra cuando el duende esta en Repetir

	// Dialogo para los duendes de tipo Habilidad
	public string[] dialogoHabilidad;

	// Dialogo para los duendes de tipo Acertijo
	public string[] dialogoAcertijo;

	public bool resuelveActividad;

}

public class GestorDeDialogos : MonoBehaviour {

	public Duende[] duendes;
	GestorDeActividades gActividades;
	ControladorDelJugador cJugador;

	GameObject casaDuende;

	void Awake()
	{
		casaDuende = GameObject.Find ("Casa");
	}
		
	void Start () {
		gActividades = GameObject.FindGameObjectWithTag ("Player").GetComponent<GestorDeActividades> ();
		cJugador = GameObject.FindGameObjectWithTag ("Player").GetComponent<ControladorDelJugador> ();

	}

	public void IniciarDuendes()
	{
		// Carga los valores de los duendes para una partida nueva. El primer duende de aprendizaje se activaria y los demas se colocarian Inactivos
		for(int i = 0; i < duendes.Length; i++)
		{
			if(duendes[i].tipo == "Aprendizaje" || duendes[i].tipo == "Habilidad")
			{
				if(duendes[i].idDuende == 1)
				{
					duendes[i].estado = "Activo";
					duendes[i].colorGorroDuende = Color.green;
					duendes[i].gorroDuende.GetComponent<Renderer>().material.color = duendes[i].colorGorroDuende;
				}
				else
				{
					duendes[i].estado = "Inactivo";
					duendes[i].colorGorroDuende = Color.red;
					duendes[i].gorroDuende.GetComponent<Renderer>().material.color = duendes[i].colorGorroDuende;

					casaDuende.SetActive (false);
				}
			}

		}
	}

	public void CargarDuendes(int cantDuendes)
	{
		print ("CargarDuendes"+cantDuendes);
		for (int i = 0; i < duendes.Length; i++) 
		{
			if (duendes [i].tipo == "Aprendizaje" || duendes [i].tipo == "Habilidad") 
			{
				if (cantDuendes == 0)
				{
					duendes [i].estado = "Repetir";
					duendes [i].colorGorroDuende = Color.blue;
					duendes [i].gorroDuende.GetComponent<Renderer> ().material.color = duendes [i].colorGorroDuende;

					if(duendes[i].resuelveActividad == true)
					{
						for(int j=0; j<gActividades.actividades.Length; j++)
						{
							if(duendes[i].idDuende == gActividades.actividades[j].idDuende)
							{
								gActividades.actividades [j].puzleCompletado = true;
								gActividades.actividades [j].jugadorEsfera = true;
								gActividades.actividades [j].esfera.SetActive (false);

								// Cuando se carga una partida y la actividad se completo
								// Colocar el objeto en su recolector  
								// Tipo 3 -- Selecion Multiple con orden y un objeto 
								if(gActividades.actividades[j].tipo == 3)
								{
									for(int k = 0; k<cJugador.objetosClaves.Length; k++)
									{
										// Tipo 1 -- Regadera
										if(cJugador.objetosClaves[k].tipo == 1)
										{
											cJugador.objetosClaves [k].objetoReal.transform.position = cJugador.objetosClaves [k].objetoTransparente.transform.position;
											cJugador.objetosClaves [k].objetoReal.GetComponent<Rigidbody> ().constraints = RigidbodyConstraints.FreezeAll;
											cJugador.objetosClaves [k].objetoTransparente.SetActive (false);
										}
									}
								}

								if(gActividades.actividades[j].tipo == 4)
								{
									for(int k = 0; k<cJugador.objetosClaves.Length; k++)
									{
										// Tipo 2 --  Muestras de terreno
										if (cJugador.objetosClaves [k].tipo == 2) 
										{
											cJugador.objetosClaves [k].objetoReal.transform.position = cJugador.objetosClaves [k].objetoTransparente.transform.position;
											cJugador.objetosClaves [k].objetoReal.GetComponent<Rigidbody> ().constraints = RigidbodyConstraints.FreezeAll;
											cJugador.objetosClaves [k].objetoTransparente.SetActive (false);
										}
									}
								}

								if (gActividades.actividades [j].tipo == 5) 
								{
									for (int k = 0; k < cJugador.objetosClaves.Length; k++) 
									{
										// Tipo 5 --  Troncos
										if (cJugador.objetosClaves [k].tipo == 5) 
										{
											cJugador.objetosClaves [k].objetoReal.SetActive (false);
										}
									}

									casaDuende.SetActive (true);
								}
							}
						}
					}
				}

				if(cantDuendes == 1)
				{
					if(duendes[i].idDuende == PlayerPrefs.GetInt("datosIdDuende"))
					{
						duendes[i].estado = "Activo";
						duendes[i].colorGorroDuende = Color.green;
						duendes[i].gorroDuende.GetComponent<Renderer>().material.color = duendes[i].colorGorroDuende;

						if (duendes [i].resuelveActividad == true) 
						{

							for (int j = 0; j < gActividades.actividades.Length; j++) 
							{
								if (duendes [i].idDuende == gActividades.actividades [j].idDuende) 
								{
									if (gActividades.actividades [j].tipo == 5) 
									{
										casaDuende.SetActive (false);
									}
								}
							}
						}

					}

					if(duendes[i].idDuende < PlayerPrefs.GetInt("datosIdDuende"))
					{
						duendes[i].estado = "Repetir";
						duendes[i].colorGorroDuende = Color.blue;
						duendes[i].gorroDuende.GetComponent<Renderer>().material.color = duendes[i].colorGorroDuende;

						if(duendes[i].resuelveActividad == true)
						{
							for(int j=0; j<gActividades.actividades.Length; j++)
							{
								if(duendes[i].idDuende == gActividades.actividades[j].idDuende)
								{
									gActividades.actividades [j].puzleCompletado = true;
									gActividades.actividades [j].jugadorEsfera = true;
									gActividades.actividades [j].esfera.SetActive (false);

									// Cuando se carga una partida y la actividad se completo
									// Colocar el objeto en su recolector  
									// Tipo 3 -- Selecion Multiple con orden y un objeto 
									if(gActividades.actividades[j].tipo == 3)
									{
										for(int k = 0; k<cJugador.objetosClaves.Length; k++)
										{
											// Tipo 1 -- Regadera
											if(cJugador.objetosClaves[k].tipo == 1)
											{
												cJugador.objetosClaves [k].objetoReal.transform.position = cJugador.objetosClaves [k].objetoTransparente.transform.position;
												cJugador.objetosClaves [k].objetoReal.GetComponent<Rigidbody> ().constraints = RigidbodyConstraints.FreezeAll;
												cJugador.objetosClaves [k].objetoTransparente.SetActive (false);
											}
										}
									}

									if(gActividades.actividades[j].tipo == 4)
									{
										for(int k = 0; k<cJugador.objetosClaves.Length; k++)
										{
											// Tipo 2 --  Muestras de terreno
											if (cJugador.objetosClaves [k].tipo == 2) 
											{
												cJugador.objetosClaves [k].objetoReal.transform.position = cJugador.objetosClaves [k].objetoTransparente.transform.position;
												cJugador.objetosClaves [k].objetoReal.GetComponent<Rigidbody> ().constraints = RigidbodyConstraints.FreezeAll;
												cJugador.objetosClaves [k].objetoTransparente.SetActive (false);
											}
										}
									}

									if (gActividades.actividades [j].tipo == 5) 
									{
										for (int k = 0; k < cJugador.objetosClaves.Length; k++) 
										{
											// Tipo 5 --  Troncos
											if (cJugador.objetosClaves [k].tipo == 5) 
											{
												cJugador.objetosClaves [k].objetoReal.SetActive (false);
											}
										}

										casaDuende.SetActive (true);
									}
								}
							}
						}
					}

					if(duendes[i].idDuende > PlayerPrefs.GetInt("datosIdDuende"))
					{
						duendes[i].estado = "Inactivo";
						duendes[i].colorGorroDuende = Color.red;
						duendes[i].gorroDuende.GetComponent<Renderer>().material.color = duendes[i].colorGorroDuende;

						//Desaparece la casa del duende cuando esta inactivo (Color rojo) luego de cargar una partida
						if (duendes [i].resuelveActividad == true) 
						{

							for (int j = 0; j < gActividades.actividades.Length; j++) 
							{
								if (duendes [i].idDuende == gActividades.actividades [j].idDuende) 
								{
									if (gActividades.actividades [j].tipo == 5) 
									{
										casaDuende.SetActive (false);
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
