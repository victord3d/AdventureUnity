using UnityEngine;
using System.Collections;

public class MovimientoDePlataforma : MonoBehaviour {

	public GameObject plataforma;
	public Transform posicionInicial;
	public Transform posicionFinal;
	public Vector3 nuevaPosicion;
	public string estadoActual;
	public float suavidad;			// Smooth
	public float tiempo;			// Reset Time
	public int tipoDePlataforma;	// 1. posInicial y posFinal; 2. posInicial, posMedia y posFinal
	public Transform posicionMedia;
	public string estadoAnterior;


	void Start () 
	{
		CambiarDestino();
	}

	void FixedUpdate()
	{
		
		plataforma.transform.position = Vector3.Lerp(plataforma.transform.position, nuevaPosicion, suavidad*Time.deltaTime);

	}

	public void CambiarDestino()
	{
		if(tipoDePlataforma == 1)
		{
			if(estadoActual == "PosicionInicial")
			{
				estadoActual = "PosicionFinal";
				nuevaPosicion = posicionFinal.position;

			}
			else if(estadoActual == "PosicionFinal")
			{
				estadoActual = "PosicionInicial";
				nuevaPosicion = posicionInicial.position;
			}
			else if(estadoActual == "")
			{
				estadoActual = "PosicionFinal";
				nuevaPosicion = posicionFinal.position;
			}	
		}

		if(tipoDePlataforma == 2)
		{
			
			if(estadoActual == "PosicionInicial")
			{
				estadoAnterior = "PosicionInicial";
				estadoActual = "PosicionMedia";
				nuevaPosicion =  posicionMedia.position;
			}
			else if(estadoActual == "PosicionMedia")
			{
				if(estadoAnterior == "PosicionInicial")
				{
					estadoAnterior = "PosicionMedia";
					estadoActual = "PosicionFinal";
					nuevaPosicion = posicionFinal.position;
				}else
				{
					estadoAnterior = "PosicionMedia";
					estadoActual = "PosicionInicial";
					nuevaPosicion = posicionInicial.position;
				}

			}
			else if(estadoActual == "PosicionFinal")
			{
				estadoAnterior = "PosicionFinal";
				estadoActual = "PosicionMedia";
				nuevaPosicion = posicionMedia.position;
			}
			else if(estadoActual == "")
			{
				estadoAnterior = "PosicionInicial";
				estadoActual = "PosicionMedia";
				nuevaPosicion = posicionMedia.position;
			}
		}

		if(tipoDePlataforma == 3)
		{
			if(estadoActual == "PosicionInicial")
			{
				estadoActual = "PosicionFinal";
				nuevaPosicion = posicionFinal.position;

			}
			else if(estadoActual == "PosicionFinal")
			{
				estadoActual = "PosicionInicial";
				nuevaPosicion = posicionInicial.position;
				plataforma.transform.position = nuevaPosicion;

			}
			else if(estadoActual == "")
			{
				estadoActual = "PosicionFinal";
				nuevaPosicion = posicionFinal.position;
			}	
		}

		Invoke("CambiarDestino", tiempo);
	}
}
