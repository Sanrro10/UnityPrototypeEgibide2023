using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

[AddComponentMenu("UI/Effects/Gradient")]
public class GradientHoriz : BaseMeshEffect
{
	public Color32 topColor = Color.white;
	public Color32 bottomColor = Color.black;
	public float posicionM;
	public override void ModifyMesh(VertexHelper helper)
	{
		if (!IsActive() || helper.currentVertCount == 0)
			return;
		
		List<UIVertex> vertices = new List<UIVertex>();
		helper.GetUIVertexStream(vertices);
		
		float bottomX = vertices[0].position.x;
		float topX = vertices[0].position.x;
		
		for (int i = 1; i < vertices.Count; i++)
		{
			float x = vertices[i].position.x;
			if (x > topX)
			{
				topX = x;
			}
			else if (x < bottomX)
			{
				bottomX = x;
			}
		}
		
		float uiElementHeight = topX - bottomX;
		
		UIVertex v = new UIVertex();
		
		for (int i = 0; i < helper.currentVertCount; i++)
		{
			helper.PopulateUIVertex(ref v, i);
			v.color = Color32.Lerp(bottomColor, topColor, (v.position.x - bottomX) / uiElementHeight/posicionM);
			helper.SetUIVertex(v, i);
		}
	}
}