using Microsoft.Win32.SafeHandles;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace HuanweiDZ.Components
{
    public class Ledger : IList<LedgerItem>
    {
        private LedgerItem[] _contents = new LedgerItem[999];
        private int _count;
        public int Count => _count;
        public Ledger()
        {
            _count = 0;
        }
        public LedgerItem this[int index]
        {
            get
            {
                return _contents[index];
            }

            set
            {
                _contents[index] = value;
            }
        }


        public bool IsReadOnly => false;

        public void Add(LedgerItem item)
        {
            _contents[_count] = item;
            _count++;
        }

        public void Clear()
        {
            _count = 0;
        }

        public bool Contains(LedgerItem item)
        {
            for (int i = 0; i < Count; i++)
            {
                if (_contents[i] == item)
                {
                    return true;
                }
            }
            return false;
        }

        public void CopyTo(LedgerItem[] array, int arrayIndex)
        {
            throw new NotImplementedException();
        }

        public IEnumerator<LedgerItem> GetEnumerator()
        {
            return new LedgerItemEnumerator(_contents);
        }

        public int IndexOf(LedgerItem item)
        {
            for (int i = 0; i < Count; i++)
            {
                if (_contents[i] == item)
                {
                    return i;
                }
            }
            return -1;
        }

        public void Insert(int index, LedgerItem item)
        {
            if ((_count + 1 <= _contents.Length) && (index < Count) && (index >= 0))
            {
                _count++;
                for (int i = Count - 1; i > index; i--)
                {
                    _contents[i] = _contents[i - 1];
                }
                _contents[index] = item;
            }
        }

        public bool Remove(LedgerItem item)
        {
            if (IndexOf(item) == -1) return false;
            RemoveAt(IndexOf(item));
            return true;
        }

        public void RemoveAt(int index)
        {
            if ((index >= 0) && (index < Count))
            {
                for (int i = index; i < Count - 1; i++)
                {
                    _contents[i] = _contents[i + 1];
                }
            }
            _count--;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            throw new NotImplementedException();
        }

    }

    internal class LedgerItemEnumerator : IEnumerator<LedgerItem>
    {
        private LedgerItem[] _contents;
        int position = -1;
        public LedgerItemEnumerator(LedgerItem[] contents)
        {
            _contents = contents;
        }

        object IEnumerator.Current => Current;

        public bool MoveNext()
        {
            position++;
            return (position < _contents.Length);
        }

        public void Reset()
        {
            position = -1;
        }

        private bool _disposed = false;
        private SafeHandle _safeHandle = new SafeFileHandle(IntPtr.Zero, true);

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (_disposed)
            {
                return;
            }

            if (disposing)
            {
                _safeHandle?.Dispose();
            }
            _disposed = true;

        }
        public LedgerItem Current
        {
            get
            {
                try
                {
                    return _contents[position];
                }
                catch (IndexOutOfRangeException)
                {

                    throw new InvalidOperationException();
                }
            }
        }

    }
}
