using System;
using Remouse.Shared.Utils.Log;

namespace Remouse.Shared.Utils.Buffer
{
    public class RoundBuffer<T> where T : class, new()
    {
        private int _capacity;
        private T[] _values;

        private int _current;
        
        public RoundBuffer(int capacity)
        {
            _capacity = capacity;
            _values = new T[capacity];
            for (int i = 0; i < _values.Length; i++)
            {
                _values[i] = new T();
            }
        }

        public void MoveToNext()
        {
            _current++;
            _current = _current % _capacity;
        }

        public T GetCurrent()
        {
            return _values[_current];
        }

        public T GetAt(int index)
        {
            if (index > _capacity)
            {
                Logger.Current.LogException(this, new ArgumentOutOfRangeException($"Next value is more than capacity of buffer. capacity {_capacity} , index {index}"));
            }

            return _values[(_current + index) % _capacity];
        }
    }
}