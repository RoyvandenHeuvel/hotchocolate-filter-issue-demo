using System.Reflection;
using HotChocolate.Types.Descriptors;

var builder = WebApplication.CreateBuilder(args);
builder.Services
    .AddGraphQLServer()
    .AddFiltering((config) => config.AddDefaults().AllowOr(false).AllowAnd(false))
    .AddQueryType<Query>();

var app = builder.Build();

app.MapGraphQL();

app.Run();

public class Book
{
    public string Title { get; set; }

    [PublicationYearResolver]
    public int PublicationYear { get; set; }

    [AuthorResolver]
    public Author Author { get; set; }
}

public class PublicationYearResolverAttribute : ObjectFieldDescriptorAttribute
{
    public override void OnConfigure(IDescriptorContext context, IObjectFieldDescriptor descriptor, MemberInfo member)
    {
        descriptor.Resolve(context => 1999);
    }
}

public class AuthorResolverAttribute : ObjectFieldDescriptorAttribute
{
    public override void OnConfigure(IDescriptorContext context, IObjectFieldDescriptor descriptor, MemberInfo member)
    {
        descriptor.Resolve(context => new Author { Name = "Jon Skeet" });
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