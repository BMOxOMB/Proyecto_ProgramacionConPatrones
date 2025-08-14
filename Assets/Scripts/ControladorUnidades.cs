using UnityEngine;
using System.Collections.Generic;

public class ControladorUnidades : MonoBehaviour
{
    public SeleccionMultiple selector;

    private GrupoUnidades grupoActual = new GrupoUnidades();
    private Camera mainCamera;

    void Start()
    {
        mainCamera = Camera.main;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(1)) // clic derecho
        {
            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                Vector3 destino = hit.point;
                grupoActual.MoverA(destino);
            }
        }
    }

    public void ActualizarGrupo(List<UnidadMilitar> seleccionadas)
    {
        grupoActual.LimpiarGrupo();
        foreach (UnidadMilitar unidad in seleccionadas)
        {
            grupoActual.AgregarUnidad((IUnidadEjecutable)unidad);
        }
    }
}
