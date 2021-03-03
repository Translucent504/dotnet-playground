# Notes

Entity framework keeps track of objects in relation to whatever context we have.
The basic tracking state of an object can be seen by using `context.Entry(myObject).State;`
These states include:

- Detached (Not being tracked)
- Added (Object has been added to the database)
- Unchanged (Object is saved to database and not been modified)
- Modified (Object has been modified)
- Removed (Object deleted)

## Summary

- Experiments with Change tracking
  - `context.Entry()` is used very often to check what EFCore thinks of an object.
  - EFCore also keeps track of all the object's original value that it got from the database and that is the source of truth it uses to
  determine what the tracking state is.
- RawSql Statements
  - Reading
    `context.Dishes.FromRawSql()` for constant fixed queries.
    `context.Blogs.FromSqlInterpolated($"SELECT * FROM [dbo].[SearchBlogs]({userSuppliedSearchTerm})")` to pass in parameters.
  - Writing
    `context.Dishes.ExecuteRawSql()`
  - Dont do sql injections
- Transactions with EF
  - `using var transaction = context.database.begintransaction()`
- Expression Trees
  - How EF determines what SQL to write from C# Code.
    There is a difference in how the compiler compiles the code. some lambdas are defined as expressions which can be read at runtime as
    an object tree which reads like the way we have written the c# code itself and what method we called on what guy. While the other
    usual way to compile a normal lambda (NOT ANE EXPRESSION) would be to compile it to machine code normally without any refernce to the
    original c# code that we had written to generate it.
