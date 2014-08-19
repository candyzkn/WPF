// ***********************************************************************
// Assembly         : MyWPF.MVVMBase
// Author           : ZhenKaining
// Created          : 08-14-2014
//
// Last Modified By : ZhenKaining
// Last Modified On : 08-14-2014
// ***********************************************************************
// <copyright file="NotifyObject.cs" company="Geoway">
//     Copyright (c) Geoway. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using System;
using System.ComponentModel;
using System.Linq.Expressions;

/// <summary>
/// The MVVMBase namespace.
/// </summary>
namespace MyWPF.MVVMBase
{
    /// <summary>
    /// ViewModel的基类,属性更新机制类
    /// </summary>
    public abstract class NotifyObject : INotifyPropertyChanged
    {
        /// <summary>
        /// 在更改属性值时发生。
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Called when [property changed].
        /// </summary>
        /// <param name="propertyName">Name of the property.</param>
        protected virtual void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
        /// <summary>
        /// Called when [property changed].
        /// </summary>
        /// <param name="propertyNames">The property names.</param>
        /// <exception cref="System.ArgumentNullException">propertyNames</exception>
        protected virtual void OnPropertyChanged(string[] propertyNames)
        {
            if (propertyNames == null) throw new ArgumentNullException("propertyNames");

            foreach (string item in propertyNames)
            {
                OnPropertyChanged(item);
            }
        }
        /// <summary>
        /// Called when [property changed].
        /// </summary>
        /// <param name="expression">The expression.</param>
        protected virtual void OnPropertyChanged(Expression<Func<object>> expression)
        {
            if (expression == null) throw new ArgumentNullException("propertyExpression");

            if (expression.NodeType == ExpressionType.Lambda)
            {
                MemberExpression body = null;
                if (expression.Body is UnaryExpression)
                {
                    var ue = expression.Body as UnaryExpression;
                    body = ue.Operand as MemberExpression;
                }
                else
                {
                    body = expression.Body as MemberExpression;
                }
                if (body != null)
                {
                    string propertyName = body.Member.Name;
                    OnPropertyChanged(propertyName);
                }
            }
        }
    }
}
