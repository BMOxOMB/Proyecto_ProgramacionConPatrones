using UnityEngine;

using UnityEngine.AI;

using System.Collections.Generic;


[RequireComponent(typeof(NavMeshAgent))]
public class UnidadMilitar : MonoBehaviour, IUnidad, IUnidadEjecutable, IDamageable
{
    public static List<UnidadMilitar> unidadesAliadas = new List<UnidadMilitar>();

    public string tipoUnidad;
    public int vida = 100;
    public float velocidad = 3f;
    public int costoEntrenamiento = 50;

    private Vector3? destino = null;

    private NavMeshAgent agent;


    void OnEnable()
    {
        unidadesAliadas.Add(this);
    }

    void OnDisable()
    {
        unidadesAliadas.Remove(this);
    }

    
    public void TakeDamage(int cantidad)
    {
        vida -= cantidad;
        Debug.Log(tipoUnidad + " recibió " + cantidad + " de daño. Vida restante: " + vida);

        if (vida <= 0)
        {
            Debug.Log(tipoUnidad + " ha sido destruido.");
            Destroy(gameObject);
        }
    }

    public void RecibirDanio(int cantidad)
    {
        TakeDamage(cantidad);
    }
    
    public IUnidad Clonar()
    {
        GameObject clon = Instantiate(this.gameObject);
        return clon.GetComponent<IUnidad>();
    }

    public void EjecutarAccion()
    {
        Debug.Log(tipoUnidad + " está lista para combatir.");
    }

    public void MoverA(Vector3 nuevoDestino)
    {
        nuevoDestino.y = transform.position.y;
        destino = nuevoDestino;
        agent.SetDestination(nuevoDestino);
        CambiarEstado(null);
        Debug.Log(tipoUnidad + " se dirige a: " + destino);
    }


    public void Seleccionar(bool estado)
    {
        Renderer r = GetComponent<Renderer>();
        if (r != null)
            r.material.color = estado ? Color.green : Color.white;
    }

    private IEstadoUnidadJugador estadoActual;

    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.speed = velocidad;
    }

    public void CambiarEstado(IEstadoUnidadJugador nuevoEstado)
    {
        estadoActual = nuevoEstado;
    }

    void Update()
    {
        if (destino != null)
        {
            if (!agent.pathPending && agent.remainingDistance <= agent.stoppingDistance)
            {
                destino = null;
                CambiarEstado(new EstadoAtacarAuto()); // solo entra al estado automático cuando termina de moverse
            }
        }
        else
        {
            estadoActual?.Ejecutar(this); // solo si no hay destino
        }
    }

}