using UnityEngine;

public class RecursoMineral : MonoBehaviour
{
    public int vida = 50;
    public int oroPorRecurso = 25;

    public void RecibirDanio(int cantidad)
    {
        vida -= cantidad;
        Debug.Log("Recurso golpeado. Vida restante: " + vida);

        if (vida <= 0)
        {
            GameManager.Instance.GanarOro(oroPorRecurso);
            Debug.Log("Recurso recolectado. +" + oroPorRecurso + " oro.");
            Destroy(gameObject);
        }
    }
}
