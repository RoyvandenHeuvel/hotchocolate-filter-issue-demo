using System.Reflection;
using HotChocolate.Types.Descriptors;

var builder = WebApplication.CreateBuilder(args);
builder.Services
    .AddGraphQLServer()
    .AddFiltering()
    .AddQueryType<Query>();

var app = builder.Build();

app.MapGraphQL();

app.Run();

public class Book
{
    public string Title { get; set; }

    [AuthorResolver]
    public Author Author { get; set; }
}

public class AuthorResolverAttribute : ObjectFieldDescriptorAttribute
{
    public override void OnConfigure(IDescriptorContext context, IObjectFieldDescriptor descriptor, MemberInfo member)
    {
        descriptor.Resolve(context => { return new Author { Name = "Jon Skeet" }; });
    }
}

public class Author
{
    public string Name { get; set; }
}

public class Query
{
    [UseFiltering]
    public IEnumerable<Book> GetBooks() =>
        new List<Book>()
        {
            new Book
            {
                Title = "C# in depth.",
            },
        };
}