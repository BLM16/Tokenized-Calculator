using System;
using System.Collections;
using System.Collections.Generic;

namespace BLM16.Util.Calculator;

/// <summary>
/// Represents a strongly typed list of unique objects according to a <see cref="comparator"/>
/// </summary>
/// <typeparam name="T">The type of unique elements in the list</typeparam>
public class UniqueList<T> : IEnumerable<T>
{
    /// <summary>
    /// Contains the list of unique values
    /// </summary>
    private readonly List<T> _values = new();

    /// <summary>
    /// Compares the equality between 2 values to determine if they are unique
    /// </summary>
    /// <remarks>
    /// Returns the equality of <typeparamref name="T"/>1 and <typeparamref name="T"/>2
    /// </remarks>
    private readonly Func<T, T, bool> comparator;

    /// <summary>
    /// Creates a new <see cref="UniqueList{T}"/> with an equality comparator
    /// </summary>
    /// <param name="comparator">A delegate that returns the equality of 2 <typeparamref name="T"/> values</param>
    public UniqueList(Func<T, T, bool> comparator)
    {
        this.comparator = comparator;
    }

    /// <summary>
    /// Adds a value to the list that is unique according to the <see cref="comparator"/>
    /// </summary>
    /// <param name="value">The unique value to be added</param>
    /// <exception cref="ArgumentException">Thrown when <paramref name="value"/> is not unique</exception>
    public void Add(T value)
    {
        foreach (var val in _values)
        {
            if (comparator(value, val))
                throw new ArgumentException($"{typeof(T)} must be unique");
        }

        _values.Add(value);
    }

    /// <summary>
    /// Adds a range of values to the list that are unique according to the <see cref="comparator"/>
    /// </summary>
    /// <param name="enumerable">The range of unique values to be added</param>
    /// <returns><see langword="this"/></returns>
    /// <exception cref="ArgumentException">Thrown when <paramref name="enumerable"/> contains a value that is not unique</exception>
    public UniqueList<T> With(IEnumerable<T> enumerable)
    {
        foreach (var val in enumerable)
            Add(val);

        return this;
    }

    public IEnumerator<T> GetEnumerator()
    {
        foreach (var val in _values)
            yield return val;
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}
