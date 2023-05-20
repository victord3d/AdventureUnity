using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class IniciarPartida : MonoBehaviour {

	public static bool cargarPartida;
	public Button botonContinuar;

	//Panel de Controles
	public GameObject panelControles;
	public Button botonNuevaPartida;
	public Button botonSalir;
	public Button botonControles;
	public Button botonRegresar;

	void Start () 
	{
		if(PlayerPrefs.HasKey("datosEsferas") == false)
		{
			botonContinuar.interactable = false;
		}

		//PanelControles
		panelControles.SetActive(false);

	}

	public void CargarPartida()
	{
		cargarPartida = true;
		SceneManager.LoadScene(1);
	}

	public void NuevaPartida()
	{
		cargarPartida = false;
		SceneManager.LoadScene(1);
	}

	public void Salir()
	{
		Application.Quit ();
	}

	public void VerControles()
	{
		botonContinuar.interactable = false;
		botonNuevaPartida.interactable = false;
		botonSalir.interactable = false;
		botonControles.interactable = false;
		panelControles.SetActive(true);
	}

	public void Regresar()
	{
		botonContinuar.interactable = true;
		botonNuevaPartida.interactable = true;
		botonSalir.interactable = true;
		botonControles.interactable = true;

		if(PlayerPrefs.HasKey("datosEsferas") == false)
		{
			botonContinuar.interactable = false;
		}

		panelControles.SetActive(false);

	}
}
