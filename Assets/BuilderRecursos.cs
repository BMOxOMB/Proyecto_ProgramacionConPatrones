using UnityEngine;
/*
 *
 *El script que maneja la creacion de los grupos de recursos, define el valor numerico que va tener cada grupo y  
 *de cuantas figuras es el grupo, un grupo de 3 figuras del tipo comida va tener ABC recursos 
 * 
 */
public class BuilderRecursos
{
    public GameObject Prefab; // Sphere o Capsule
    public TipoRecurso Tiporecurso;//Metal o comida 

    public BuilderRecursos(GameObject prefab, TipoRecurso tipo)
    {
        this.Prefab = prefab;
        this.Tiporecurso = tipo;
    }

    public void SpawnResourceGroup(Vector3 centerPosition, int TamanhoGrupo, Transform parent)
    {
        //PARA MINADO, SE MINA AL Grupode: que es el GameObject que se crea a la hora de generar los grupos y contiene el valor de recurso
        //Para generar los soldados,edificios se puede usar un enum que guarde cuanto ocupo de X recursos para Z objeto y con un IF >= comparo 
        //comparo contra mis recursos disponibles y si cumplo hago spawn 
        GameObject group = new GameObject($"Grupode:_{Tiporecurso}_{TamanhoGrupo}");//El parent de los grupos, sale en el editor
        group.transform.parent = parent;
        //Defino cuantos recursos va tener un grupo de recursos
        int valoresRecursos = TamanhoGrupo switch
        {
            1 => 500, //Un grupo de 1 tiene 500 del tipo de recurso y asi con los demas
            3 => 1000,
            5 => 5000,
            _ => 0
        };
        //Creando los grupos de recursos
        GruposRecursos rg = group.AddComponent<GruposRecursos>(); 
        rg.tiporecurso = Tiporecurso;
        rg.CantidadRecurso = valoresRecursos;
//Los grupos aparacen como X cantidad de figuras individuales pero tienen un parent que representa el grupo y tiene los valores


        float spacing = 2f;//Mi espacio entre figuras
        //Genero los grupos
        for (int i = 0; i < TamanhoGrupo; i++)//Doy vueltas por cuantas figuras tenga el grupo
        {
            //Genero las figuras
            Vector3 offset = new Vector3(i * spacing, 0, 0);
            Vector3 spawnPos = centerPosition + offset;
            Quaternion rotation = Quaternion.Euler(270, 0f, 0f);
            GameObject instance = GameObject.Instantiate(Prefab, spawnPos, rotation , group.transform);
            Renderer renderer = instance.GetComponent<Renderer>();
            if (renderer != null)
            {
                float objectHeight = renderer.bounds.size.y;
                instance.transform.position = new Vector3(spawnPos.x, spawnPos.y, spawnPos.z);
            }
            else
            {
                instance.transform.position = new Vector3(spawnPos.x, 0f, spawnPos.z);
            }

        }
    }
}
