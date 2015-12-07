using System;
using System.Collections.Generic;
using System.IO;

namespace ArmyAnt
{
	public class SQLDataFile : AConfigFile
	{
		/// <summary>
		/// 载入SQL数据文件
		/// </summary>
		/// <param name="filename">要载入的SQL数据文件</param>
		public SQLDataFile(string filename = null) : base(filename)
        {
		}
		/// <summary>
		/// 从另一个配置数据引用创建SQL格式配置数据对象
		/// </summary>
		/// <param name="value">要共享数据引用的另一个配置数据对象</param>
		public SQLDataFile(AConfigFile value) : base(value)
        {
		}
		/// <summary>
		/// 从SQL格式数据载入SQL配置
		/// <para><see cref="AConfigFile.LoadString(string)"/></para>
		/// <para><see cref="IConfigFile.LoadString(string)"/></para>
		/// </summary>
		/// <param name="text">要载入的SQL格式文本，该文本可由<c>byte[]</c>格式转换而来</param>
		/// <returns>成功返回<c>true</c></returns>
		override public bool LoadString(string text)
		{


			return true;
		}
		/// <summary>
		/// 将配置数据转化为SQL格式数据
		/// <para><see cref="AConfigFile.ToString()"/></para>
		/// <para><see cref="IConfigFile.ToString()"/></para>
		/// </summary>
		/// <returns>转化后的SQL格式数据，可直接转换为<c>byte[]</c></returns>
		override public string ToString()
		{
			List<byte> ret = new List<byte>();


			return BitConverter.ToString(ret.ToArray());
		}

	}
}
