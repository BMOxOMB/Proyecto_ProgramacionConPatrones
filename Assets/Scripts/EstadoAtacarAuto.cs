using UnityEngine;

public class EstadoAtacarAuto : IEstadoUnidadJugador
{
    public void Ejecutar(UnidadMilitar unidad)
    {
        Collider[] hits = Physics.OverlapSphere(unidad.transform.position, 4f);

        foreach (var hit in hits)
        {
            EnemigoIA enemigo = hit.GetComponent<EnemigoIA>();
            if (enemigo != null)
            {
                Debug.Log(unidad.tipoUnidad + " ataca automáticamente a " + enemigo.name);
                enemigo.RecibirDanio(10);
                return;
            }
        }

        unidad.CambiarEstado(new EstadoIdle());
    }
}