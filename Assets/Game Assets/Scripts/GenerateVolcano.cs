using UnityEngine;
using System.Collections;

public class GenerateVolcano : MonoBehaviour {

	// Use this for initialization
	void Start () {
        TerrainData tdata = new TerrainData();
		tdata.SetDetailResolution (1024, 16);
		tdata.size = new Vector3 (31.25f, 600f, 31.25f);
		tdata.thickness = 1;
		tdata.heightmapResolution = 513;
		SplatPrototype[] splats = new SplatPrototype[1];
		splats [0] = new SplatPrototype ();
		splats [0].texture = (Texture2D)Resources.Load ("VolcanoTextures/ground2_Specular");
		tdata.splatPrototypes = splats;

		int h = tdata.heightmapHeight;
		int w = tdata.heightmapWidth;
		float[,] data = new float[h, w];
		using (System.IO.FileStream file = System.IO.File.OpenRead("Assets/Resources/TerrainHeightMaps/terrain_windows.raw"))
		using (System.IO.BinaryReader reader = new System.IO.BinaryReader(file))
		{
			for (int y = 0; y < h; y++)
			{
				for (int x = 0; x < w; x++)
				{
					float v = (float)reader.ReadUInt16() / 0xFFFF;
					data[y, x] = v;
				}
			}
		}
		tdata.SetHeights(0, 0, data);

		Terrain.CreateTerrainGameObject(tdata).name = "Volcano";
	}
}
