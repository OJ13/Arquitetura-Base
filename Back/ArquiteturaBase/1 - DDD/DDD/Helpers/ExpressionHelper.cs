using System;
using System.Linq.Expressions;

namespace DDD.Helpers
{
    public static class ExpressionHelper
    {
        public static Expression<Func<T, bool>> And<T>(this Expression<Func<T, bool>> expr1, Expression<Func<T, bool>> expr2)
        {
            if (expr1 == null)
                return expr2;

            ParameterExpression param = expr1.Parameters[0];
            if (ReferenceEquals(param, expr2.Parameters[0]))
            {
                return Expression.Lambda<Func<T, bool>>(
                    Expression.And(expr1.Body, expr2.Body), param);
            }

            return Expression
                .Lambda<Func<T, bool>>(Expression.And(expr1.Body, Expression.Invoke(expr2, param)), param);
        }

        public static Expression<Func<T, bool>> Or<T>(this Expression<Func<T, bool>> expr1, Expression<Func<T, bool>> expr2)
        {
            if (expr1 == null)
                return expr2;

            ParameterExpression param = expr1.Parameters[0];
            if (ReferenceEquals(param, expr2.Parameters[0]))
            {
                return Expression.Lambda<Func<T, bool>>(
                    Expression.Or(expr1.Body, expr2.Body), param);
            }

            return Expression
                .Lambda<Func<T, bool>>(Expression.Or(expr1.Body, Expression.Invoke(expr2, param)), param);
        }
    }
}
