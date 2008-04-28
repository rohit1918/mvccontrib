using System;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace MvcContrib.NHamlViewEngine.Utilities
{
  public static class Extensions
  {
    [SuppressMessage("Microsoft.Naming", "CA1720")]
    public static string RenderAttributes(this object obj)
    {
      if(obj != null)
      {
        PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(obj);

        if(properties.Count > 0)
        {
          var attributes = new StringBuilder();

          var value = Convert.ToString(properties[0].GetValue(obj));

          if(!string.IsNullOrEmpty(value))
          {
            attributes.Append(properties[0].Name.Replace('_', '-') + "=\"" + value + "\"");
          }

          for(int i = 1; i < properties.Count; i++)
          {
            value = Convert.ToString(properties[i].GetValue(obj));

            if (!string.IsNullOrEmpty(value))
            {
              attributes.Append(" " + properties[i].Name + "=\"" + value + "\"");
            }
          }

          return attributes.ToString();
        }
      }

      return null;
    }
  }
}