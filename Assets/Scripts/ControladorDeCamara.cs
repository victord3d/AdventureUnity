using UnityEngine;
using System.Collections;

public class ControladorDeCamara : MonoBehaviour 
{
	public Transform jugador;
	private Rigidbody rbCamara;

	public float distancia = 3.0f;
	public float distanciaMin = 2.0f;
	public float distanciaMax = 20f;

	public float inclinacionMinEnY = 1f;
	public float inclinacionMaxEnY = 80f;

	public float velocidadX = 50.0f;
	public float velocidadY = 50.0f;

	float x = 0.0f;
	float y = 0.0f;

	float acercarCamara = 0.0f;
	float alejarCamara = 0.0f;

	void Start () 
	{
		Vector3 angulo = transform.eulerAngles;
		x = angulo.y;
		y = angulo.x;

		rbCamara = GetComponent<Rigidbody>();

		// congelar rotacion del rigidbody
		if (rbCamara != null)
		{
			rbCamara.freezeRotation = true;
		}
	}

	void LateUpdate () 
	{
		if (jugador) 
		{
			x += Input.GetAxis("CamaraHorizontal") * velocidadX * distancia * 0.02f;
			y -= Input.GetAxis("CamaraVertical") * velocidadY * 0.02f;

			// Calcula la inclinacion de la camara en Y
			y = CalcularAngulo(y, inclinacionMinEnY, inclinacionMaxEnY);

			Quaternion rotacion = Quaternion.Euler(y, x, 0);

			// Zoom de la camara 
			if(Input.GetKeyDown(KeyCode.Alpha1))
			{
				alejarCamara = -0.1f;
				distancia = Mathf.Clamp(distancia - alejarCamara*5, distanciaMin, distanciaMax);
			}
			else if(Input.GetKeyDown(KeyCode.Alpha2)) 
			{
				acercarCamara = 0.1f;
				distancia = Mathf.Clamp(distancia - acercarCamara*5, distanciaMin, distanciaMax);
			}
			else
			{
				distancia = Mathf.Clamp(distancia, distanciaMin, distanciaMax);
			}
				

			Vector3 distanciaNegativa = new Vector3(0.0f, 0.0f, -distancia);
			Vector3 posicion = rotacion * distanciaNegativa + jugador.position;

			transform.rotation = rotacion;
			transform.position = posicion;
		}
	}

	public static float CalcularAngulo(float angulo, float min, float max)
	{
		if (angulo < -360F)
			angulo += 360F;
		if (angulo > 360F)
			angulo -= 360F;
		return Mathf.Clamp(angulo, min, max);
	}

}
