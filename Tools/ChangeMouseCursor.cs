/********************************************************************************
** author：       Liang
** date：         2016-11-02 20:53:42
** description：  更换鼠标图片
** version:       V_1.0.0
*********************************************************************************/


using UnityEngine;
using System.Collections;

namespace ZF.DataDriveCom.Tools
{
	public class ChangeMouseCursor : MonoBehaviour
	{
		/// <summary>
		///  鼠标图片
		/// </summary>
		private Texture mouseTexture;


		public void SetMouseTexture(Texture texture)
		{
			mouseTexture = texture;
		}

		/// <summary>
		///  更换鼠标图片；
		/// </summary>
		private void OnGUI()
		{
			GUI.DrawTexture(new Rect(
				Input.mousePosition.x, Screen.height - Input.mousePosition.y, mouseTexture.width, mouseTexture.height), mouseTexture);
		}
	}
}