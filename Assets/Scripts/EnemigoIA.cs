using UnityEngine;

public class EnemigoIA : MonoBehaviour, IDamageable
{
    public IEstadoUnidadIA estadoActual;
    public Transform[] puntosPatrulla;
    public float velocidad = 2f;

    private int indicePatrulla = 0;
    public int vida = 100;

    public void TakeDamage(int cantidad)
    {
        RecibirDanio(cantidad);
    }

    public void RecibirDanio(int cantidad)
    {
        vida -= cantidad;
        Debug.Log(name + " recibió " + cantidad + " de daño. Vida restante: " + vida);

        if (vida <= 0)
        {
            Debug.Log(name + " ha sido destruido.");
            GameManager.Instance.UnregisterEnemy();
            Destroy(gameObject);
        }
    }

    void Start()
    {
        estadoActual = new EstadoPatrullar();
        CambiarColor(Color.black); // color base
        estrategiaCombate = new EstrategiaMelee(); // Puedes cambiar por EstrategiaRanged()
    }

    void Update()
    {
        // Detectar enemigos cercanos antes de ejecutar el estado actual
        UnidadMilitar unidadCercana = BuscarUnidadCercana();

        if (unidadCercana != null && !(estadoActual is EstadoAtacar))
        {
            CambiarEstado(new EstadoAtacar(unidadCercana));
        }

        estadoActual?.Ejecutar(this);
    }

    public UnidadMilitar BuscarUnidadCercana()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, 10f);
        UnidadMilitar masCercana = null;
        float minDistancia = Mathf.Infinity;

        foreach (Collider col in colliders)
        {
            UnidadMilitar unidad = col.GetComponent<UnidadMilitar>();
            if (unidad != null)
            {
                float distancia = Vector3.Distance(transform.position, unidad.transform.position);
                if (distancia < minDistancia)
                {
                    minDistancia = distancia;
                    masCercana = unidad;
                }
            }
        }

        return masCercana;
    }


    // Estrategia de movimiento simple
    public void MoverA(Vector3 destino)
    {
        transform.position = Vector3.MoveTowards(transform.position, destino, velocidad * Time.deltaTime);
    }

    // Cambio de estado externo
    public void CambiarEstado(IEstadoUnidadIA nuevoEstado)
    {
        estadoActual = nuevoEstado;
    }

    // Para patrullar
    public Transform ObtenerSiguientePunto()
    {
        Transform punto = puntosPatrulla[indicePatrulla];
        indicePatrulla = (indicePatrulla + 1) % puntosPatrulla.Length;
        return punto;
    }
    
    public void CambiarColor(Color color)
    {
        Renderer r = GetComponent<Renderer>();
        if (r != null)
            r.material.color = color;
    }
    
    public IEstrategiaCombate estrategiaCombate;

    public void SetEstrategiaCombate(IEstrategiaCombate nuevaEstrategia)
    {
        estrategiaCombate = nuevaEstrategia;
    }

}