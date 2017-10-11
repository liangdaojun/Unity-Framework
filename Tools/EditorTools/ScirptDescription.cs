/********************************************************************************
** auther： Liang
** date：   2016-10-08 13:42:18
** descrition： 添加对脚本的描述信息
** Version:  V_1.0.0
*********************************************************************************/

#if UNITY_EDITOR_WIN && UNITY_EDITOR

/*using System;
using System.IO;
using System.Text;
using UnityEditor;


namespace ZF.DataDriveCom.Tools
{

	/// <summary>  
	///  添加对脚本的描述信息，包括 [作者， 创建时间， 脚本描述， 版本号]；  
	/// </summary>  
	public class ScirptDescription : UnityEditor.AssetModificationProcessor
	{
		private static void OnWillCreateAsset(string path)
		{
			path = path.Replace(".meta", "");

			// 修改脚本文件，更改脚本内容；

			if (path.EndsWith(".cs"))
			{
				string strContent = File.ReadAllText(path);

				strContent = strContent.Replace("#AuthorName#", "Liang")

					.Replace("#CreateDate#", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"))

					.Replace("#Descrition#", "********").Replace("#Version#", "V_1.0.0")
                   
                    .Replace("#Namespace#", "ZF");

				File.WriteAllText(path, strContent,Encoding.UTF8);

				AssetDatabase.Refresh();

			}
		}
	}
}*/

#endif

