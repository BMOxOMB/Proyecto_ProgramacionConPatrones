using UnityEngine;
using System.Collections.Generic;

public class SeleccionMultiple : MonoBehaviour
{
    public Texture2D cajaSeleccionTexture;
    private Rect rectSeleccion;
    private Vector2 inicioMouse;

    public List<UnidadMilitar> unidadesSeleccionadas = new List<UnidadMilitar>();
    public ControladorUnidades controlador; // ← este lo conectas en Unity

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            inicioMouse = Input.mousePosition;
        }

        if (Input.GetMouseButton(0))
        {
            Vector2 mouseActual = Input.mousePosition;
            rectSeleccion = new Rect(
                Mathf.Min(inicioMouse.x, mouseActual.x),
                Screen.height - Mathf.Max(inicioMouse.y, mouseActual.y),
                Mathf.Abs(mouseActual.x - inicioMouse.x),
                Mathf.Abs(mouseActual.y - inicioMouse.y)
            );
        }

        if (Input.GetMouseButtonUp(0))
        {
            unidadesSeleccionadas.Clear();

            foreach (UnidadMilitar unidad in UnidadMilitar.unidadesAliadas)
            {
                Vector3 pantallaPos = Camera.main.WorldToScreenPoint(unidad.transform.position);
                pantallaPos.y = Screen.height - pantallaPos.y;

                if (rectSeleccion.Contains(pantallaPos, true))
                {
                    unidadesSeleccionadas.Add(unidad);
                    unidad.Seleccionar(true);
                }
                else
                {
                    unidad.Seleccionar(false);
                }
            }

            if (controlador != null)
            {
                controlador.ActualizarGrupo(unidadesSeleccionadas); // ← grupo actualizado aquí
            }

            rectSeleccion = new Rect();
        }
    }

    void OnGUI()
    {
        if (Input.GetMouseButton(0))
        {
            GUI.color = new Color(0, 0.5f, 1, 0.25f);
            GUI.DrawTexture(rectSeleccion, cajaSeleccionTexture);
            GUI.color = Color.white;
        }
    }
}
