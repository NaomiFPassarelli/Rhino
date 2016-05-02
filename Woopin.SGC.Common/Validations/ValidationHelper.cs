using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Woopin.SGC.Common.Validations
{
    public static class ValidationHelper
    {
        public static string[] GetNotValidateKeys(object model)
        {
            List<string> r = new List<string>();
            string modelAttribute = "";

            foreach(var property in model.GetType().GetProperties())
            {

                // Keys a sacar por objeto interno, salvo el id para el requerido.
                if(property.GetCustomAttributes(typeof(DoNotValidateOnlyId),false).Count() > 0)
                {
                    modelAttribute = property.Name;
                    foreach(var innerProperty in property.PropertyType.GetProperties())
                    {
                        if(innerProperty.Name != "Id")
                        {
                            r.Add(modelAttribute + "." + innerProperty.Name);
                            r.Add(model.GetType().Name + "." + modelAttribute + "." + innerProperty.Name);
                        }
                    }
                }
                else if (property.GetCustomAttributes(typeof(DoNotValidate), false).Count() > 0)
                {
                    modelAttribute = property.Name;
                    foreach (var innerProperty in property.PropertyType.GetProperties())
                    {
                        r.Add(modelAttribute + "." + innerProperty.Name);
                        r.Add(model.GetType().Name + "." + modelAttribute + "." + innerProperty.Name);
                    }
                }
                //else if (property.PropertyType.GetInterface("IList") != null)
                //{
                //    List<property.PropertyType.GetType()> list = new List<property.PropertyType.GetType()>();  
                //    for(var i = 0; ; i++)
                //    {
                //        ValidationHelper.GetNotValidateKeys()
                //    }
                //}
            }

            return r.ToArray();
        }
    }
}
