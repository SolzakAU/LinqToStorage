﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace LinqToStorage
{
  class StorageContext
  {
    public IQueryable<PatientRecord> Patient = new StorageQueryProvider<PatientRecord>();
    public IQueryable<RequestRecord> Request;
    public IQueryable<ReportInstance> Report;
    public IQueryable<PatientIdentifier> PatientIdentifier;
  }

  class StorageQueryProvider<T> : IQueryable<T>, IQueryProvider
  {
    #region IQueryable<T>
    IEnumerator<T> IEnumerable<T>.GetEnumerator()
    {
      throw new NotImplementedException();
    }

    System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
    {
      throw new NotImplementedException();
    }

    Type IQueryable.ElementType
    {
      get { return typeof(T); }
    }

    System.Linq.Expressions.Expression IQueryable.Expression
    {
      get { return Expression.Constant(null, typeof(PatientRecord)); }
    }

    IQueryProvider IQueryable.Provider
    {
      get { return this; }
    }
    #endregion

    #region IQueryProvider
    IQueryable<TElement> IQueryProvider.CreateQuery<TElement>(System.Linq.Expressions.Expression expression)
    {
      throw new NotImplementedException();
    }

    IQueryable IQueryProvider.CreateQuery(System.Linq.Expressions.Expression expression)
    {
      throw new NotImplementedException();
    }

    TResult IQueryProvider.Execute<TResult>(System.Linq.Expressions.Expression expression)
    {
      throw new NotImplementedException();
    }

    object IQueryProvider.Execute(System.Linq.Expressions.Expression expression)
    {
      throw new NotImplementedException();
    }
    #endregion
  }
}