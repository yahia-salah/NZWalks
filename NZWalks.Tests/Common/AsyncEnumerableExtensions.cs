﻿using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Query;

public static class AsyncEnumerableExtensions
{
	public static IAsyncEnumerable<T> AsAsyncEnumerable<T>(this IEnumerable<T> source)
	{
		return new TestAsyncEnumerable<T>(source);
	}

	public static IAsyncEnumerator<T> GetAsyncEnumerator<T>(this IAsyncEnumerable<T> source, CancellationToken cancellationToken = default)
	{
		return source.GetAsyncEnumerator(cancellationToken);
	}

	private class TestAsyncEnumerable<T> : EnumerableQuery<T>, IAsyncEnumerable<T>, IQueryable<T>
	{
		public TestAsyncEnumerable(IEnumerable<T> enumerable) : base(enumerable) { }
		public TestAsyncEnumerable(Expression expression) : base(expression) { }

		public IAsyncEnumerator<T> GetAsyncEnumerator(CancellationToken cancellationToken = default)
		{
			return new TestAsyncEnumerator<T>(this.AsEnumerable().GetEnumerator());
		}

		IQueryProvider IQueryable.Provider => new TestAsyncQueryProvider<T>(this);
	}

	private class TestAsyncEnumerator<T> : IAsyncEnumerator<T>
	{
		private readonly IEnumerator<T> _inner;

		public TestAsyncEnumerator(IEnumerator<T> inner)
		{
			_inner = inner;
		}

		public ValueTask DisposeAsync()
		{
			_inner.Dispose();
			return ValueTask.CompletedTask;
		}

		public ValueTask<bool> MoveNextAsync()
		{
			return new ValueTask<bool>(_inner.MoveNext());
		}

		public T Current => _inner.Current;
	}

	private class TestAsyncQueryProvider<T> : IAsyncQueryProvider
	{
		private readonly IQueryProvider _inner;

		public TestAsyncQueryProvider(IQueryProvider inner)
		{
			_inner = inner;
		}

		public IQueryable CreateQuery(Expression expression)
		{
			return new TestAsyncEnumerable<T>(expression);
		}

		public IQueryable<TElement> CreateQuery<TElement>(Expression expression)
		{
			return new TestAsyncEnumerable<TElement>(expression);
		}

		public object Execute(Expression expression)
		{
			return _inner.Execute(expression);
		}

		public TResult Execute<TResult>(Expression expression)
		{
			return _inner.Execute<TResult>(expression);
		}

		public IAsyncEnumerable<TResult> ExecuteAsync<TResult>(Expression expression)
		{
			return new TestAsyncEnumerable<TResult>(expression);
		}

		public Task<TResult> ExecuteAsync<TResult>(Expression expression, CancellationToken cancellationToken)
		{
			return Task.FromResult(Execute<TResult>(expression));
		}

		TResult IAsyncQueryProvider.ExecuteAsync<TResult>(Expression expression, CancellationToken cancellationToken)
		{
			return Execute<TResult>(expression);
		}
	}
}
