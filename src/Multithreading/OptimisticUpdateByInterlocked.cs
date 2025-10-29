namespace HowProgrammingWorksOnDotNet.Multithreading;

public record Data(int Version);

public class OptimisticUpdateByInterlocked
{
    // Версия - отражает кол-во модификаций. 1 изменение - 1 версия
    public static bool TryUpdateIfVersion(int expectedVersion, ref Data data, Data newData)
    {
        // Сохраняем ссылку на "текущий объект"
        Data current = data;

        // Выполняем предварительную проверку, что обновление имеет смысл
        if (current.Version != expectedVersion)
            return false;

        // Выполняем CAS-операцию, где проверяется, что текущий data ссылается на сохраненный нами current.
        // Если другой поток обновит data, то будет уже ссылка на другой объект.
        // Возвращаем результат проверки, что старое значение (которое возвращает CE) - это наш сохраненный
        return Interlocked.CompareExchange(ref data, newData, current) == current;
    }

    // Версия - независимый конкурентный токен. В рамках 1 версии могут быть N изменений (других полей)
    public static bool TryUpdateWhileVersion(int expectedVersion, ref Data data, Data newData)
    {
        Data current;
        do
        {
            current = data;
            if (current.Version != expectedVersion)
                return false;
        } while (Interlocked.CompareExchange(ref data, newData, current) != current);
        return true;
    }
}
