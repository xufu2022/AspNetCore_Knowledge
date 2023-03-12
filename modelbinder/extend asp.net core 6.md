# What Is Model Binder

It’s a Standard Class That Inherits From IModelBinder
There Are 3 or 4 in Every App by Default.
It:
- Has Access to Everything Posted to an Action.
- It Can Do anything Needed to the Input Data.
- Is Responsible for Creating a Correct Data Object to Provide to the Action

## The Model Binder Class

It’s a simple standard C# class
It Inherits from IModelBinder
The Inherited Class:
- Only Requires One Method Override
- Needs to use async/await
- Only Returns Task (IE: is void)
- Data Return is Via a Binding Context
- Is Associated with a Model Class Using Attributes
