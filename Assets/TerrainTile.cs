using System;
using UnityEngine;
using Mapzen;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class TerrainTile : MonoBehaviour
{
    public int TileZ = 14;

    public TileAddress Address;
    public Texture2D ElevationTexture;
    public Texture2D NormalTexture;
        
    public void Start()
    {
        this.Address = new TileAddress(0, 0, TileZ);
        this.data = new TerrainTileData(Address);
        this.data.ElevationTexture = this.ElevationTexture;
        this.data.NormalTexture = this.NormalTexture;

        var mesh = new Mesh();

        GetComponent<MeshFilter>().mesh = mesh;

        this.data.GenerateElevationGrid(mesh, 128, new Vector3(-0.5f, 0.0f, -0.5f));
        // this.data.GenerateElevationGrid(mesh, 32, Vector3.zero);

        this.data.ApplyElevation(mesh, 0.5f);

        var material = GetComponent<MeshRenderer>().material;

        mesh.RecalculateNormals();
        // this.data.ApplyNormalTexture(material);

    }

    public void Update()
    {
        transform.Rotate(Vector3.up, Time.deltaTime * 12.0f);
    }

    private TerrainTileData data;
}

