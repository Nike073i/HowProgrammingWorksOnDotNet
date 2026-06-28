// namespace HowProgrammingWorksOnDotNet.Patterns.FactoryMethod;

// /*
//     Мб это вообще не про этот паттерн, а про паттерн "Команда"
//     Есть проблема - команда создается лишь раз, но некоторые параметры - определяются на момент вызова. Какие есть идеи решения:
//     1. Создание команды с generic-параметром "Контекст". И тогда постоянные параметры - передаются в конструкторе, переменные параметры - в параметре вызова (контексте)
//     2. Использование паттерна "Рецепт". Постоянные параметры хранятся в рецепте (фабрике), переменные передаются в конструктор команды. Рецепт достает параметры из сам достает из постоянных -  внешних сервисов / контекста выполнения. Например через IHttpContextAccessor или IUserService.
//     3. Object параметр.
//     Другая проблема - унификация возвращаемого значения...
// */

// public abstract class BaseCommand<TResult, TArg> : ICommand
// {
//     protected abstract TResult HandleMethod(TArg arg);

//     public void Handle(IExecutionContext executionContext)
//     {
//         var result = HandleMethod(default!); // executionContext.Read();
//         executionContext.Write(result.ToString());
//     }
// }

// public interface ICommand
// {
//     void Handle(IExecutionContext executionContext);
// }

// public interface IExecutionContext
// {
//     void Write(string data);
//     string Read();
// }

// public class ExecutionContext : IExecutionContext
// {
//     public string Read()
//     {
//         throw new NotImplementedException();
//     }

//     public void Write(string data)
//     {
//         throw new NotImplementedException();
//     }
// }

// public interface ICommandRecipe
// {
//     ICommand Create(IExecutionContext context);
// }

// public record Todo(Guid Id, string Name);

// public record GetTodoCommandResult(Guid TodoId, string TodoName);

// public class GetTodoCommandRecipe(List<Todo> todos) : ICommandRecipe
// {
//     public ICommand Create(IExecutionContext context)
//     {
//         var idPar = context.GetParam("id");
//         return new GetTodoCommand(Guid.Parse(idPar), todos);
//     }
// }

// public class GetTodoCommand(List<Todo> todos) : BaseCommand<GetTodoCommandResult, Guid>
// {
//     public GetTodoCommandResult Handle(Guid id)
//     {
//         var todo = todos.FirstOrDefault(t => t.Id == id);
//         if (todo == null)
//             return null;
//         return new(todo.Id, todo.Name);
//     }
// }

// public class CreateTodoCommandRecipe(List<Todo> todos) : ICommandRecipe<Guid>
// {
//     public ICommand<Guid> Create(IExecutionContext context)
//     {
//         var name = context.GetParam("name");
//         return new CreateTodoCommand(name, todos);
//     }
// }

// public class CreateTodoCommand(string name, List<Todo> todos) : ICommand<Guid>
// {
//     public Guid Handle()
//     {
//         var id = new Guid();
//         todos.Add(new(id, name));

//         return id;
//     }
// }

// public class ConsoleExample
// {
//     [Fact]
//     public void Usage()
//     {
//         var todos = new List<Todo>();
//         var getCommandRecipe = new GetTodoCommandRecipe(todos);
//         var createCommandRecipe = new CreateTodoCommandRecipe(todos);

//         void Invoke(string commandName, string args)
//         {
//             var pairs = args.Split(";");
//             var dict = new Dictionary<string, string>();
//             foreach (var pair in pairs)
//             {
//                 var kv = pair.Split('=');
//                 dict[kv[0]] = kv[1];
//             }
//             var context = new ExecutionContext(dict);
//         }
//     }
// }
