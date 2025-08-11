using UnityEngine;
using UnityEngine.AI;

public class ColisionObConfig : IColisionConfiguracion
{

    public void Configurar(GameObject figura)
    {
        if (figura.GetComponent<Collider>()==null)
        {
            //Se añade el collider segun el tipo de figura a spawnear
            if (figura.name.ToLower().Contains("cilindro"))
            {
                figura.AddComponent<CapsuleCollider>();
            }
            else
            {
                figura.AddComponent<BoxCollider>();
            }
        }

        //Se agrega navmesh segun figura
        if (figura.GetComponent<UnityEngine.AI.NavMeshObstacle>()==null)
        {
            var Obstaculo = figura.AddComponent<UnityEngine.AI.NavMeshObstacle>();
            Obstaculo.carving = true;
            Obstaculo.shape = UnityEngine.AI.NavMeshObstacleShape.Box;
        }
        
    }

}
