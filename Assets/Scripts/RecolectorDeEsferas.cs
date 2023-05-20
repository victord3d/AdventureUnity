using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class RecolectorDeEsferas : MonoBehaviour {

	InventarioDelJugador inventarioJugador;

	public GameObject panelEsferas;	
	public Text textoEsferas; 			
	public string mensaje1;
	public string mensaje2;

	int esferasTotales = 5; 		// Numero de esferas que debe recolectar el jugador para completar el nivel

	void Start () {

		inventarioJugador = GameObject.FindGameObjectWithTag ("Player").GetComponent<InventarioDelJugador> ();

		DesactivarPanel ();

	}

	void OnTriggerEnter(Collider Otro)
	{

		ActivarPanel ();
		VerificarEsferas();
	}

	void OnTriggerExit(Collider Otro)
	{
		DesactivarPanel ();
	}

	public void ActivarPanel()
	{
		panelEsferas.SetActive (true);
	}

	public void DesactivarPanel()
	{
		panelEsferas.SetActive (false);
	}

	public void VerificarEsferas()
	{
		int esferasFaltantes;

		if(inventarioJugador.esferasAgarradas < esferasTotales)
		{
			esferasFaltantes = esferasTotales - inventarioJugador.esferasAgarradas;
			mensaje1 = "Te faltan "+ esferasFaltantes + " esferas para completar el nivel.";
			textoEsferas.text = mensaje1;
		}

		if(inventarioJugador.esferasAgarradas == esferasTotales)
		{
			textoEsferas.text = mensaje2;
		}
	}
}
