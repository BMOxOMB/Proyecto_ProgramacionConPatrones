using UnityEngine;

public class EstrategiaMelee : IEstrategiaCombate
{
    private float tiempoAtaque = 1.5f;
    private float temporizador = 0f;
    private int danio = 20;

    public void Atacar(EnemigoIA contexto, UnidadMilitar objetivo)
    {
        temporizador -= Time.deltaTime;

        if (temporizador <= 0f)
        {
            float distancia = Vector3.Distance(contexto.transform.position, objetivo.transform.position);

            if (distancia < 1.5f)
            {
                objetivo.TakeDamage(danio);
                temporizador = tiempoAtaque;
            }
        }
    }
}