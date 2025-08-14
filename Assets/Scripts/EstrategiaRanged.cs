using UnityEngine;

public class EstrategiaRanged : IEstrategiaCombate
{
    private float tiempoAtaque = 2f;
    private float temporizador = 0f;
    private int danio = 10;

    public void Atacar(EnemigoIA contexto, UnidadMilitar objetivo)
    {
        temporizador -= Time.deltaTime;

        if (temporizador <= 0f)
        {
            float distancia = Vector3.Distance(contexto.transform.position, objetivo.transform.position);

            if (distancia < 6f)
            {
                objetivo.TakeDamage(danio);
                Debug.Log("Ataque a distancia lanzado");
                temporizador = tiempoAtaque;
            }
        }
    }
}