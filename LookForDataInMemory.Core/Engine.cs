using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace LookForDataInMemory.Core
{
	/// <summary>
	/// Движок, который ищет указанный объект в указанном объекте и вложенных в него.
	/// </summary>
    public class Engine
    {
		public static List<Type> OmitTypes = new List<Type>()
		{
			typeof(Assembly),
			typeof(Type),
			/// чтобы поиск велся по другим полям датасета, более понятным юзеру
			typeof(System.Data.DataViewManager),
			typeof(System.Data.DataView),
			typeof(System.Data.DataRowView)
		};

		public static List<Type> AllowedTypes = new List<Type>()
		{
			typeof(DateTime),
			typeof(DataSet),
			typeof(DataTable),
			typeof(DataRow)
		};

		/// <summary>
		/// Ищет объект внутри объекта.
		/// Если два пути содержат одни и те же объекты в середине, то
		/// будет возвращен только один из них в целях оптимизации поиска.
		/// </summary>
		/// <param name="what">То, что ищем.</param>
		/// <param name="whereRoot">То, в чем ищем.</param>
		/// <param name="byRef">Сравнивать ли объекты по ссылке.</param>
		/// <param name="softSearch">Смягчить ли поиск, т.е. приводить ли 
		/// типы в том числе к string при сравнении и искать ли внутри текста
		/// (без учета регистра к тому же).</param>
		public static FindResult Find(object what, object whereRoot,
			int depth = 10,
			bool byRef = false,
			bool softSearch = true)
		{
			var result = FindInner(what, whereRoot, depth, byRef, softSearch);
			result.SearchValue = what;

			result.PrepareResults();

			return result;
		}

		/// <summary>
		/// Ищет объект внутри объекта.
		/// Если два пути содержат одни и те же объекты в середине, то
		/// будет возвращен только один из них в целях оптимизации поиска.
		/// </summary>
		/// <param name="what">То, что ищем.</param>
		/// <param name="whereRoot">То, в чем ищем.</param>
		/// <param name="byRef">Сравнивать ли объекты по ссылке.</param>
		/// <param name="softSearch">Смягчить ли поиск, т.е. приводить ли 
		/// типы в том числе к string при сравнении и искать ли внутри текста
		/// (без учета регистра к тому же).</param>
		/// <param name="path">Текущий путь. При первом вызове метода не указывается.</param>
		/// <param name="checkedObjects">Чтобы избежать зацикливания и повторов.
		/// При первом вызове метода не указывается.
		/// Заполняется внутри.</param>
		static FindResult FindInner(object what, object whereRoot, 
			int maxItemsCountInPath = 10, 
			bool byRef = false,
			bool softSearch = true,
            string path = "", List<object> checkedObjects = null)
		{
			if (checkedObjects == null)
				checkedObjects = new List<object>();

			if (string.IsNullOrEmpty(path))
			{
				/// создаем объект, содержащий текущий root для поиска в нем
				var res = FindInner(what, new { Root = whereRoot }, maxItemsCountInPath,
					byRef, softSearch, ".", checkedObjects);

				return res;
			}

			FindResult result = new FindResult();

			if (whereRoot != null && what != null && !checkedObjects.Contains(whereRoot)
				&& path.Split('.').Length <= maxItemsCountInPath &&
				IsAllowedType(whereRoot.GetType()))
			{
				checkedObjects.Add(whereRoot);

				var props = whereRoot.GetType().GetProperties().Where(p => p.CanRead);
				var fields = whereRoot.GetType().GetFields();
				List<MemberInfo> members = new List<MemberInfo>(props.Cast<MemberInfo>());
				members.AddRange(fields.Cast<MemberInfo>());

				foreach (var member in members)
				{
					object propVal = null;

					try
					{
						propVal = member is PropertyInfo ?
							 ((PropertyInfo)member).GetValue(whereRoot) :
							 ((FieldInfo)member).GetValue(whereRoot);
					}
					catch
					{
					}

					if (propVal != null && !checkedObjects.Contains(propVal))
					{
						var propType = propVal.GetType();
						string newPath = (path + "." + member.Name).TrimStart('.');

						var eRes = AreObjectsEqual(what, propVal, byRef, softSearch);

                        if (eRes.AreEqual)
						{
							result.Paths.Add(eRes.TunePath(newPath));
						}
						else
						{
							result.CheckedPaths.Add(newPath);

							if (IsAllowedType(propVal.GetType()))
							{
								var res = FindInner(what, propVal, maxItemsCountInPath,
									byRef, softSearch, newPath, checkedObjects);

								result.AddFrom(res);
							}
						}

						if (propVal is IEnumerable && !(propVal is string))
						{
							IEnumerable collection = (IEnumerable)propVal;

							int i = 0;
							foreach (var item in collection)
							{
								if (!checkedObjects.Contains(item))
								{
									var eRes2 = AreObjectsEqual(what, item, byRef,
										softSearch);
									var itemPath = newPath + string.Format("[{0}]", i);

									if (eRes2.AreEqual)
									{
										var res = eRes2.TunePath(itemPath);

										result.Paths.Add(res);
									}
									else
									{
										result.CheckedPaths.Add(itemPath);

										var res = FindInner(what, item, maxItemsCountInPath,
											byRef, softSearch,
											itemPath, checkedObjects);

										result.AddFrom(res);
									}
								}

								i++;
							}
						}
					}
				}
			}

			return result;
		}

		/// <summary>
		/// TODO: если ищется объект простого типа, то смысла сравнивать 
		/// сложный.ToString() с ним мало, но не знаю, улучшит ли это скорость алгоритма.
		/// </summary>
		public static ObjectsEqualResult AreObjectsEqual(
			object whatToFind, object whatWeHave, 
			bool byRef, bool softSearch)
		{
			if ((byRef && object.ReferenceEquals(whatToFind, whatWeHave)) ||
				(!byRef && object.Equals(whatToFind, whatWeHave)))
			{
				return new ObjectsEqualResult() { AreEqual = true };
			}
			else if (!byRef && softSearch)
			{
				bool res = Convert.ToString(whatToFind).ToLower() == 
					Convert.ToString(whatWeHave).ToLower();

				if (!res && whatWeHave is string)
				{
					res = ((string)whatWeHave).ToLower()
						.Contains(Convert.ToString(whatToFind).ToLower());

					return new ObjectsEqualResult()
					{
						AreEqual = res,
						ContainsText = res
					};
				}
				else
					return new ObjectsEqualResult()
					{
						AreEqual = res
					};
			}
			else
				return new ObjectsEqualResult() { AreEqual = false };
		}

		///// <summary>
		///// Поддерживаемые списочные типы (и их потомки).
		///// </summary>
		//public static List<Type> ListTypes = new List<Type>()
		//{
		//	typeof(IList),
		//	typeof(IEnumerable)
		//};

		public static bool IsAllowedType(Type type)
		{
			bool intelligenceMode = true;

			if (intelligenceMode)
				return
					(type.IsPrimitive ||
					type == typeof(DateTime) ||
					type.Namespace == null ||
					(!type.Namespace.StartsWith("System") &&
					type.Module.ScopeName != "CommonLanguageRuntimeLibrary") ||
					AllowedTypes.Any(t => t == type || t.IsAssignableFrom(type))) &&

					!OmitTypes.Any(t => t == type || t.IsAssignableFrom(type));
			else
				return !OmitTypes.Any(t => t == type || t.IsAssignableFrom(type));
		}

		public static bool IsOmittedType(Type type)
		{
			return !IsAllowedType(type);

			//bool powerfulFilter = true;

			//if (powerfulFilter)
			//	return
			//		!type.IsPrimitive &&
			//		type != typeof(DateTime) &&
			//		(type.Namespace != null &&
			//		type.Namespace.StartsWith("System")) ||
			//		type.Module.ScopeName == "CommonLanguageRuntimeLibrary" ||
			//		OmitTypes.Any(t => t == type || t.IsAssignableFrom(type));
			//else
			//	return OmitTypes.Any(t => t == type || t.IsAssignableFrom(type));
		}
	}
}
