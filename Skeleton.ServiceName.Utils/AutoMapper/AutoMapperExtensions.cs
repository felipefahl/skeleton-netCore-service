using AutoMapper;
using System;
using System.Linq.Expressions;
using System.Reflection;

namespace Skeleton.ServiceName.Utils.AutoMapper
{
    public static class AutoMapperExtensions
    {
        /// <summary>
        /// Method to help on the Ignore fields on the AutoMapper.
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <typeparam name="TDestination"></typeparam>
        /// <param name="mapping"></param>
        /// <param name="property"></param>
        /// <returns></returns>
        public static IMappingExpression<TSource, TDestination> Ignore<TSource, TDestination>
            (this IMappingExpression<TSource, TDestination> mapping, Expression<Func<TDestination, object>> property)
        {
            return mapping.ForMember(property, opt => opt.Ignore());
        }

        /// <summary>
        /// Method to help on the Ignore fields on the AutoMapper.
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <typeparam name="TDestination"></typeparam>
        /// <param name="mapping"></param>
        /// <param name="properties"></param>
        /// <returns></returns>
        public static IMappingExpression<TSource, TDestination> Ignore<TSource, TDestination>
            (this IMappingExpression<TSource, TDestination> mapping, params Expression<Func<TDestination, object>>[] properties)
        {
            foreach (var property in properties)
                mapping = mapping.ForMember(property, opt => opt.Ignore());

            return mapping;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <typeparam name="TDestination"></typeparam>
        /// <typeparam name="TMember"></typeparam>
        /// <param name="mapping"></param>
        /// <param name="property"></param>
        /// <param name="mapFrom"></param>
        /// <returns></returns>
        public static IMappingExpression<TSource, TDestination> MapFrom<TSource, TDestination, TMember>
            (this IMappingExpression<TSource, TDestination> mapping, Expression<Func<TDestination, object>> property, Expression<Func<TSource, TMember>> mapFrom)
        {
            return mapping.ForMember(property, opt => opt.MapFrom(mapFrom));
        }

        public static IMappingExpression<TSource, TDestination> IgnoreAllNonExisting<TSource, TDestination>
             (this IMappingExpression<TSource, TDestination> expression)
        {
            var flags = BindingFlags.Public | BindingFlags.Instance;
            var sourceType = typeof(TSource);
            var destinationProperties = typeof(TDestination).GetProperties(flags);

            foreach (var property in destinationProperties)
            {
                if (sourceType.GetProperty(property.Name, flags) == null)
                {
                    expression.ForMember(property.Name, opt => opt.Ignore());
                }
            }
            return expression;
        }
    }
}
