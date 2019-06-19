using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace CoEco.Core.Services
{
    public static class TypeExtensions
    {
        public static T GetAttribute<T>(this MemberInfo member, bool isRequired)
where T : Attribute
        {
            var attribute = member.GetCustomAttributes(typeof(T), false).SingleOrDefault();

            if (attribute == null && isRequired)
            {
                throw new ArgumentException(
                    string.Format(
                        CultureInfo.InvariantCulture,
                        "The {0} attribute must be defined on member {1}",
                        typeof(T).Name,
                        member.Name));
            }

            return (T)attribute;
        }

        public static string GetPropertyDisplayName<T>(Expression<Func<T, object>> propertyExpression)
        {
            var memberInfo = GetPropertyInformation(propertyExpression.Body);
            if (memberInfo == null)
            {
                throw new ArgumentException(
                    "No property reference expression was found.",
                    "propertyExpression");
            }

            var attr = memberInfo.GetAttribute<DisplayNameAttribute>(false);
            if (attr == null)
            {
                return memberInfo.Name;
            }

            return attr.DisplayName;
        }

        public static string GetDisplayName(this PropertyDescriptor propertyDescriptor)
        {
            var displayNameAttr = propertyDescriptor.Attributes[typeof(DisplayAttribute)];
            if (displayNameAttr != null)
            {
                var autoGenrated = (propertyDescriptor.Attributes[typeof(DisplayAttribute)] as DisplayAttribute).GetAutoGenerateField();
                if (autoGenrated == null)
                    return ((DisplayAttribute)propertyDescriptor.Attributes[typeof(DisplayAttribute)]).GetName();
            }

            return propertyDescriptor.DisplayName;
        }
        public static bool IsDisplayAttribute(this PropertyDescriptor propertyDescriptor)
        {
            if (propertyDescriptor.Attributes[typeof(DisplayAttribute)] != null)
            {
                var autoGenrated = ((DisplayAttribute)propertyDescriptor.Attributes[typeof(DisplayAttribute)]).GetAutoGenerateField();
                if (autoGenrated != null && (bool)!autoGenrated)
                    return false;
            }

            return true;
        }

        public static int GetDisplayOrder(this PropertyDescriptor propertyDescriptor)
        {
            if (propertyDescriptor.Attributes[typeof(DisplayAttribute)] != null)
            {
                return ((DisplayAttribute)propertyDescriptor.Attributes[typeof(DisplayAttribute)]).GetOrder().GetValueOrDefault(int.MaxValue);
            }
            return int.MaxValue;
        }

        public static string GetDisplay(this MemberInfo propertyInfo)
        {
            var declaring = propertyInfo.DeclaringType;
            var metaData = declaring.GetCustomAttribute<MetadataTypeAttribute>();
            DisplayAttribute attr = null;

            if (metaData != null)
            {
                var metaProperty =
                    metaData.MetadataClassType.GetProperties().FirstOrDefault(x => x.Name == propertyInfo.Name);
                if (metaProperty != null)
                    attr = metaProperty.GetAttribute<DisplayAttribute>(false);
            }
            if (attr == null)
            {
                attr = propertyInfo.GetAttribute<DisplayAttribute>(false);
            }

            if (attr == null)
            {
                return propertyInfo.Name;
            }

            return attr.GetName();
        }

        public static string GetPropertyDisplay<T>(Expression<Func<T, object>> propertyExpression)
        {
            var memberInfo = GetPropertyInformation(propertyExpression.Body);
            if (memberInfo == null)
            {
                throw new ArgumentException(
                    "No property reference expression was found.",
                    "propertyExpression");
            }

            return GetDisplay(memberInfo);
        }

        public static MemberInfo GetPropertyInformation(Expression propertyExpression)
        {
            var memberExpr = propertyExpression as MemberExpression;
            if (memberExpr == null)
            {
                var unaryExpr = propertyExpression as UnaryExpression;
                if (unaryExpr != null && unaryExpr.NodeType == ExpressionType.Convert)
                {
                    memberExpr = unaryExpr.Operand as MemberExpression;
                }
            }

            if (memberExpr != null && memberExpr.Member.MemberType == MemberTypes.Property)
            {
                return memberExpr.Member;
            }

            return null;
        }
    }
}
