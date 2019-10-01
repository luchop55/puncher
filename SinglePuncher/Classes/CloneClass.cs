using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace SinglePuncher.Classes
{
    public static class CloneClass
    {
        /// <summary>
        /// Clones a object via shallow copy
        /// </summary>
        /// <typeparam name="T">Object Type to Clone</typeparam>
        /// <param name="obj">Object to Clone</param>
        /// <returns>New Object reference</returns>
        public static T CloneObject<T>(this T obj) where T : class
        {
            if (obj == null) return null;
            System.Reflection.MethodInfo inst = obj.GetType().GetMethod("MemberwiseClone",
                System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic);
            if (inst != null)
                return (T)inst.Invoke(obj, null);
            else
                return null;
        }

        public static List<T> CopyList<T>(this List<T> lst)
        {
            List<T> lstCopy = new List<T>();
            foreach (var item in lst)
            {
                lstCopy.Add(item);                
            }
            return lstCopy;
        }
    }
}
