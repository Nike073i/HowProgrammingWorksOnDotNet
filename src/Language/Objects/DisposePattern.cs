namespace HowProgrammingWorksOnDotNet.Language.Objects;

public class SomeClass : IDisposable
{
    private bool _disposed = false;

    public void Meth()
    {
        IsNotDisposed();
        // ...
    }

    private void IsNotDisposed() => ObjectDisposedException.ThrowIf(_disposed, this);

    public void Dispose()
    {
        // освобождаем неуправляемые ресурсы
        Dispose(true);
        // подавляем финализацию. GC.SuppressFinalize убирает объект (this) из очереди на финализацию. Ведь мы же уже очистили все.
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (_disposed)
            return;
        if (disposing)
        {
            // Освобождаем управляемые ресурсы. Например _myDbConnection.Save(), Dispose();
        }
        // освобождаем неуправляемые объекты (IntPtr)
        _disposed = true;
    }

    ~SomeClass() => Dispose(false);
}
