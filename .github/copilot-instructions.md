## Code Conventions

- Do not use `var` keyword. Whenever possible, use the actual type.
- When using Linq, prefer using the name `x` for the parameter representing the item.
- When instantiating objects, prefer using the `new()` approach.
- When using the object initializer syntax, if there are more than one property to be initialized, write each property initialization on a different line.

## Code Documentation

- Do not create xml documentation for the types that are used only inside the current solution.
- Only create xml documentation for public types that are exposed as a NuGet package.

## Unit Tests

- When using `Assert.Throws` method, always use a block body for the lambda expression.

- For each public method that is tested (including the constructor), create a different test file.
  - Ex: For a method called `Query()` create test file `QueryTests`
- All the test files for a single class should be placed in a directory with the name of the class.
- Use the naming pattern `Having<...>_When<...>_Then<...>` for the tests. Where:
  - `Having` describes the most important setup details.
  - `When` describes the action tested.
  - `Then` describes the expected result.