﻿using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace makeITeasy.AppFramework.Models
{
    public abstract class BaseQuery<T> : IBaseQuery, ISpecification<T> where T : IBaseEntity
    {
        public Expression<Func<T, bool>> Criteria { get; set; }

        public List<Expression<Func<T, object>>> Includes { get; set; }

        public List<string> IncludeStrings { get; set; }

        public int? Take { get; set; }

        public int? Skip { get; set; }

        public bool IsPagingEnabled { get; set; }
        public string SortBy { get; set; }
        public bool SortDescending { get; set; }

        protected BaseQuery()
        {
        }

        protected BaseQuery(Expression<Func<T, bool>> criteria)
        {
            if (criteria != null)
            {
                Criteria = criteria;
            }
        }

        public abstract void BuildQuery();

        public enum FunctionType
        {
            And,
            Or
        }

        private class ReplaceExpressionVisitor : ExpressionVisitor
        {
            private readonly Expression _oldValue;
            private readonly Expression _newValue;
            public ReplaceExpressionVisitor(Expression oldValue, Expression newValue)
            {
                _oldValue = oldValue;
                _newValue = newValue;
            }
            public override Expression Visit(Expression node)
            {
                if (node == _oldValue)
                {
                    return _newValue;
                }
                return base.Visit(node);
            }
        }

        public void AddFunctionToCriteria(Expression<Func<T, bool>> funcToAdd, FunctionType type = FunctionType.And)
        {
            if (this.Criteria == null)
            {
                this.Criteria = funcToAdd;
            }
            else
            {
                if (type == FunctionType.And)
                {
                    this.Criteria = AndAlsoCriteria(funcToAdd, this.Criteria);
                }
                else
                {
                    this.Criteria = OrElseCriteria(funcToAdd, this.Criteria);
                }
            }
        }

        private static Expression<Func<T, bool>> AndAlsoCriteria(Expression<Func<T, bool>> funcToAdd, Expression<Func<T, bool>> criteria)
        {
            //TODO : remove this old implementation when all will be ok
            //ParameterExpression param = GetFirstCriteriaParameter(this.Criteria);
            //this.Criteria = Expression.Lambda<Func<T, bool>>(Expression.AndAlso(this.Criteria.Body, Expression.Invoke(funcToAdd, param)), param);

            return GetExpressionInfo(funcToAdd, criteria, Expression.AndAlso);
        }

        private static Expression<Func<T, bool>> OrElseCriteria(Expression<Func<T, bool>> funcToAdd, Expression<Func<T, bool>> criteria)
        {
            //TODO : remove this old implementation when all will be ok
            //ParameterExpression param = GetFirstCriteriaParameter(this.Criteria);
            //this.Criteria = Expression.Lambda<Func<T, bool>>(Expression.OrElse(this.Criteria.Body, Expression.Invoke(funcToAdd, param)), param);

            return GetExpressionInfo(funcToAdd, criteria, Expression.OrElse);
        }

        private static Expression<Func<T, bool>> GetExpressionInfo(
                        Expression<Func<T, bool>> funcToAdd,
                        Expression<Func<T, bool>> criteria,
                        Func<Expression, Expression, BinaryExpression> functionToApply
                        )
        {
            ParameterExpression parameter = Expression.Parameter(typeof(T));
            var leftVisitor = new ReplaceExpressionVisitor(criteria.Parameters[0], parameter);
            Expression left = leftVisitor.Visit(criteria.Body);
            var rightVisitor = new ReplaceExpressionVisitor(funcToAdd.Parameters[0], parameter);
            Expression right = rightVisitor.Visit(funcToAdd.Body);
            return Expression.Lambda<Func<T, bool>>(functionToApply(left, right), parameter);
        }
    }
}