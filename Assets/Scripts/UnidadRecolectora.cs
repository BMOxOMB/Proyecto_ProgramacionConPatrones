using UnityEngine;

public class UnidadRecolectora : UnidadMilitar
{
    public int capacidadMaxima = 50;
    public int cantidadRecolectada = 0;

    private float tiempoEntreRecoleccion = 1f;
    private float temporizador = 0f;

    void Update()
    {
        base.Update(); // mantener movimiento y lógica base

        if (destino == null) // cuando no tiene órdenes
        {
            RecursoMineral recurso = BuscarRecursoCercano();
            if (recurso != null)
            {
                float distancia = Vector3.Distance(transform.position, recurso.transform.position);

                if (distancia < 2f)
                {
                    temporizador -= Time.deltaTime;
                    if (temporizador <= 0f)
                    {
                        recurso.RecibirDanio(10);
                        cantidadRecolectada += 10;
                        Debug.Log(tipoUnidad + " recolectó oro. Total: " + cantidadRecolectada);

                        if (cantidadRecolectada >= capacidadMaxima)
                        {
                            // Transfiere al GameManager
                            GameManager.Instance.GanarOro(cantidadRecolectada);
                            Debug.Log("Recolector entregó " + cantidadRecolectada + " de oro.");
                            cantidadRecolectada = 0;
                        }

                        temporizador = tiempoEntreRecoleccion;
                    }
                }
            }
        }
    }

    private RecursoMineral BuscarRecursoCercano()
    {
        RecursoMineral[] recursos = GameObject.FindObjectsOfType<RecursoMineral>();
        foreach (var recurso in recursos)
        {
            float distancia = Vector3.Distance(transform.position, recurso.transform.position);
            if (distancia < 4f)
            {
                return recurso;
            }
        }

        return null;
    }
}
