using UnityEngine;

public class EstadoAtacarAuto : IEstadoUnidadJugador
{
    public void Ejecutar(UnidadMilitar unidad)
    {
        // Primero, atacar enemigos cercanos
        EnemigoIA[] enemigos = GameObject.FindObjectsOfType<EnemigoIA>();

        foreach (var enemigo in enemigos)
        {
            float distancia = Vector3.Distance(unidad.transform.position, enemigo.transform.position);

            if (distancia < 4f)
            {
                enemigo.RecibirDanio(10);
                Debug.Log(unidad.tipoUnidad + " atacó a un enemigo.");
                return;
            }
        }

        // Si no hay enemigos, buscar recursos
        RecursoMineral[] recursos = GameObject.FindObjectsOfType<RecursoMineral>();

        foreach (var recurso in recursos)
        {
            float distancia = Vector3.Distance(unidad.transform.position, recurso.transform.position);

            if (distancia < 2f)
            {
                recurso.RecibirDanio(10);
                Debug.Log(unidad.tipoUnidad + " recolecta recurso.");
                return;
            }
        }

        unidad.CambiarEstado(new EstadoIdle());
    }
}