using UnityEngine;

public class MilitarFactory : BuildingFactory
{
    private GameObject prefab;

    public MilitarFactory(GameObject prefab)
    {
        this.prefab = prefab;
    }

    public override int Cost => 50;

    public override GameObject CrearEdificio(Vector3 posicion)
    {
        return Object.Instantiate(prefab, posicion, Quaternion.identity);
    }
}